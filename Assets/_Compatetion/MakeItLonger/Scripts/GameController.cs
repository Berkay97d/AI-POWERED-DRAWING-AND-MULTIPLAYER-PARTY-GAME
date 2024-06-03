using System;
using System.Collections;
using System.Collections.Generic;
using _SoundSystem.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using General;
using TMPro;
using UnityEngine;

namespace MakeItLonger
{
    public class OnBoxInitEventArgs : EventArgs
    {
        public Box initedBox;
        public Contester contester;
    }

    public class OnRoundOverArgs : EventArgs
    {
        public Contester WinnerContester;
        public int OveredRoundNum;
    }
    
    public enum GameState
    {
        CountdownToStart,
        GamePlaying,
        MiddleGameWait,
        GameOver
    }
    
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Box boxPrefab;
        [SerializeField] private Contester[] contesters;
        [SerializeField] private TMP_Text[] names;
        [SerializeField] private Transform camera;
        [SerializeField] private Vector3 zoomOffset;
        [SerializeField] private GameObject winGroup;
        [SerializeField] private TMP_Text winnerText;
        [SerializeField] private GameObject winnerConfetti;
        [SerializeField] private Vector3 confettiOffset;
        [SerializeField] private float confettiScale;
        [SerializeField] private MinigameTutorial minigameTutorial;
        
        
        
        public event EventHandler<OnBoxInitEventArgs> OnBoxInit;
        public event EventHandler<GameState> OnGameStateChanged;
        public event EventHandler<OnRoundOverArgs> OnRoundOver; 
        

        private Box playerBox;
        private Box aiBox;
        private GameState gameState;
        private float countdownTimer = 3f;
        private float roundTimer = 10f;
        private bool isGameStarted = false;
        private int currentRoundNumber = 0;
        private bool isGameOver = false;
        private Contester winner;
        private bool isZoomStarted = false;

        
        private void Awake()
        {
            ChangeGameState(GameState.CountdownToStart);
            names[0].text = LocalUser.GetUserDataFromCache().name;
            names[1].text = EnterTest_Script.GetBotProfile(0).name;
        }

        private void Start()
        {
            OnGameStateChanged += InternalOnGameStateChanged;
            Box.OnBoxesDestroy += OnBoxesDestroy;
            minigameTutorial.OnTutorialOver += OnTutorialOver;
        }

        private void OnTutorialOver(object sender, EventArgs e)
        {
            SoundManager.Instance.PlayCompetitionCountdownSound();
        }

        private void OnDestroy()
        {
            OnGameStateChanged -= InternalOnGameStateChanged;
            Box.OnBoxesDestroy -= OnBoxesDestroy;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void OnBoxesDestroy(object sender, EventArgs e)
        {
            if (currentRoundNumber != 3 && !GetIsGameOver())
            {
                ChangeGameState(GameState.CountdownToStart);
            }
        }

        private void Update()
        {
            Debug.Log(gameState);
            
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
                roundTimer -= Time.deltaTime;

                if (roundTimer <= 0)
                {
                    roundTimer = 0;
                }
                
                if (roundTimer <= 0 &&  contesters[0].GetContesterGrid().GetHeightOfStack() != contesters[1].GetContesterGrid().GetHeightOfStack())
                {
                    if (contesters[0].GetContesterGrid().GetHeightOfStack() > contesters[1].GetContesterGrid().GetHeightOfStack())
                    {
                        RaiseOnRoundOver(contesters[0], currentRoundNumber);
                        roundTimer = 10;
                    }
                    else
                    {
                        RaiseOnRoundOver(contesters[1], currentRoundNumber);
                        roundTimer = 10;
                    }

                    ChangeGameState(GameState.MiddleGameWait);
                    
                    foreach (var contester in contesters)
                    {
                        Destroy(contester.GetCurrentBox().gameObject);
                    }
                }
            }
        }

        private void InternalOnGameStateChanged(object sender, GameState e)
        {
            if (e == GameState.GamePlaying)
            {
                InitStartBoxes();
                isGameStarted = true;
                currentRoundNumber++;
            }
        }

        private void InitStartBoxes()
        {
            StartCoroutine(InitRoutune());
            
            IEnumerator InitRoutune()
            {
                yield return new WaitForSeconds(.5f);
                foreach (var contester in contesters)
                {
                    InitBox(contester);
                }
            }
        }
        
        public void InitBox(Contester contester)
        {
            var contesterGrid = contester.GetContesterGrid();
            var randomGrid = contesterGrid.GetAllGrid()[contesterGrid.GetRandomColumnIndex(), contesterGrid.GetProperLineIndex()];
            var box = Instantiate(boxPrefab, 
                                  randomGrid.transform.position,
                                  Quaternion.identity,
                                  contesterGrid.transform
                                 );
            box.IncreaseSpeed(contesterGrid.GetProperLineIndex());
            box.SetContester(contester);
            box.SetContesterGrid(contesterGrid);
            box.SetBoxPositionFromGrid(randomGrid);
            contester.SetCurrentBox(box);
            
            
            RaiseOnBoxInit(box, contester);
        }

        private void RaiseOnBoxInit(Box box, Contester contester)
        {
            OnBoxInitEventArgs eventArgs = new OnBoxInitEventArgs();
            eventArgs.initedBox = box;
            eventArgs.contester = contester;
            OnBoxInit?.Invoke(this, eventArgs);
        }

        private void RaiseOnRoundOver(Contester contester, int round)
        {
            OnRoundOverArgs eventArgs = new OnRoundOverArgs();
            eventArgs.WinnerContester = contester;
            eventArgs.OveredRoundNum = round;
            
            OnRoundOver?.Invoke(this, eventArgs);
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

        public float GetRoundTimer()
        {
            return roundTimer;
        }

        public void OverTheGame(Contester c)
        {
            winner = c;
            isGameOver = true;
            ZoomWinner(c);
    
            var playerOrder = c is Player
                ? new[] {0, 1}
                : new[] {1, 0};
            
            ExitTest_Script.SetPlayerOrder(playerOrder);
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
            if (wi is Player)
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
            if (isZoomStarted) return;

            isZoomStarted = true;
            
            ShowWinnerText(w);
            
            LazyCoroutines.WaitForSeconds(1f, () =>
            {
                var targetPos = w.transform.position + zoomOffset;
            
                camera.DOMove(targetPos, .5f).OnComplete(() =>
                {
                    LetTheConfettiRain(w);
                });
            });
        }

        public bool GetIsGameOver()
        {
            return isGameOver;
        }

        public Contester GetWinner()
        {
            return winner;
        }
    }
}