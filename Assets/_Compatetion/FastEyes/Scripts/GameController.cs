using UnityEngine;
using System;
using System.Collections;
using General;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using _SoundSystem.Scripts;
using _UserOperations.Scripts;
using Cinemachine;
using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using UnityEngine.SceneManagement;

namespace FastEyes
{
    public enum GameState
    {
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    
    public class GameController : MonoBehaviour
    {
        [SerializeField] private int maxTurnCount;
        [SerializeField] private Contester[] contesters;
        [SerializeField] private GameObject winGroup;
        [SerializeField] private TMP_Text winnerText;
        [SerializeField] private GameObject winnerConfetti;
        [SerializeField] private Vector3 confettiOffset;
        [SerializeField] private float confettiScale;
        [SerializeField] private CinemachineVirtualCamera playCam;
        [SerializeField] private CinemachineVirtualCamera gecisCam;
        [SerializeField] private CinemachineVirtualCamera winnerCam;
        [SerializeField] private MinigameTutorial minigameTutorial;
        
        
        public event EventHandler<GameState> OnGameStateChanged;
        public static GameController Instance { get; private set; }
        
        private GameState gameState;
        private int turnCounter = 0;
        private float countdownTimer = 3f;
        private bool isGameStarted = false;
        private bool isGameOver = false;
        private bool isZoomStarted = false;

        private void Awake()
        {
            Instance = this;

            for (var i = 0; i < contesters.Length; i++)
            {
                contesters[i].SetIndex(i);
            }
        }

        private void Start()
        {
            ChangeGameState(GameState.CountdownToStart);
            OnGameStateChanged += InternalOnGameStateChanged;
            minigameTutorial.OnTutorialOver += OnTutorialOver;
        }

        private void OnTutorialOver(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayCompetitionCountdownSound();
        }

        private void OnDestroy()
        {
            OnGameStateChanged -= InternalOnGameStateChanged;
            minigameTutorial.OnTutorialOver -= OnTutorialOver;
        }

        private void OverTheGame()
        {
            StartCoroutine(InnerRoutine());
            
            IEnumerator InnerRoutine()
            {
                if (isGameOver)
                {
                    yield break;
                }
                
                if (GetRemainingTurnCount() == 0)
                {
                    yield return new WaitForSeconds(3f);
                    ChangeGameState(GameState.GameOver);

                    if (!isGameOver)
                    {
                        OnGameOver();
                    }
                    
                    isGameOver = true;
                }
            }
        }
        
        private void Update()
        {
            OverTheGame();
            
            if (isGameOver)
            {
                return;
            }
            
            if (gameState == GameState.CountdownToStart)
            {
                countdownTimer -= Time.deltaTime;

                if (countdownTimer <= 0)
                {
                    ChangeGameState(GameState.GamePlaying);
                    countdownTimer = 3f;
                }
            }

            if (gameState == GameState.GamePlaying)
            {
                
                
            }
        }

        private void OnGameOver()
        {
            var playerOrder = GetWinnerOrder()
                .Select(contester => contester.GetIndex())
                .ToArray();
                
            ZoomWinner(GetWinnerOrder()[0]);
            
            LazyCoroutines.WaitForSeconds(6f, () =>
            {
                ExitTest_Script.SetPlayerOrder(playerOrder);
                SceneManager.LoadScene(6);
            });
        }
        
        private void InternalOnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GamePlaying)
            {
                isGameStarted = true;
            }
        }
        
        private void LetTheConfettiRain(Contester wi)
        {
            var targetPos = wi.transform.position + confettiOffset;

            var a = Instantiate(winnerConfetti);
            a.transform.position = targetPos;
            a.transform.localScale = Vector3.one * confettiScale;
        }
        
        private void ShowWinnerText(Contester wi)
        {
            if (wi is PlayerController)
            {
                LocalUser.GetUserData((data =>
                {
                    winGroup.gameObject.SetActive(true);
                    winnerText.color = Color.blue;
                    winnerText.text = data.name;
                }));
            }
            else
            {
                winGroup.gameObject.SetActive(true);
                winnerText.color = Color.red;
                winnerText.text = EnterTest_Script.GetBotProfile(0).name;
            }
        }

        private void ZoomWinner(Contester w)
        {
            Target.TotalShotOnTargets = 0;
            
            if (isZoomStarted) return;

            isZoomStarted = true;
            
            ShowWinnerText(w);
            
            LazyCoroutines.WaitForSeconds(.75f, () =>
            {
                var targetPos = winnerCam.transform.position;
                targetPos.x = 0;
                targetPos.x = w.transform.position.x;

                winnerCam.transform.position = targetPos;
                
                playCam.Priority = 0;

                LazyCoroutines.WaitForSeconds(.4f, () =>
                {
                    gecisCam.Priority = 0;
                    LetTheConfettiRain(w);
                });
            });
        }

        
        private void ChangeGameState(GameState state)
        {
            gameState = state;
            OnGameStateChanged?.Invoke(this, state);
        }

        public float GetCountdownTimer()
        {
            return countdownTimer;
        }

        public bool GetIsGameStarted()
        {
            return isGameStarted;
        }

        public GameState GetGameState()
        {
            return gameState;
        }
        
        public bool GetIsGameOver()
        {
            return isGameOver;
        }

        public void IncreaseTurnCounter()
        {
            turnCounter++;
        }

        public bool IsTargetLimitFull()
        {
            return turnCounter >= maxTurnCount;
        }

        public int GetRemainingTurnCount()
        {
            return maxTurnCount - turnCounter;
        }

        public List<Contester> GetWinnerOrder()
        {
            var sortedContesters = new List<Contester>(contesters);

            sortedContesters.Sort();
            sortedContesters.Reverse();

            for (int i = 0; i < sortedContesters.Count; i++)
            {
                Debug.Log(i + " " + sortedContesters[i]);
            }

            return sortedContesters;
        }

        public Contester GetWinner()
        {
            var a = GetWinnerOrder();

            return a[0];
        }

    }
}