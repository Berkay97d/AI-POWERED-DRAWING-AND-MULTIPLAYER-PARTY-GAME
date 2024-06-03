using System;
using System.Collections.Generic;
using System.Linq;
using _Compatetion.GameOfSnows.Ozer.Scripts;
using _Compatetion.GameOfSnows.Scripts.Provider;
using _QuestSystem.Scripts;
using _SoundSystem.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using General;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Compatetion.GameOfSnows.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Contester[] contesterArray;
        [SerializeField] private int maxRound;
        [SerializeField] private Image[] tourImageArray;
        [SerializeField] private new Transform camera;
        [SerializeField] private Vector3 zoomOffset;
        [SerializeField] private GameObject winGroup;
        [SerializeField] private TMP_Text winnerText;
        [SerializeField] private GameObject winnerConfetti;
        [SerializeField] private Vector3 confettiOffset;
        [SerializeField] private float confettiScale;

        private bool m_IsZoomStarted = false;
        public static GameManager Instance;
        public static event EventHandler<Contester[]> OnGameOver;
        public static event EventHandler<Contester> OnTurnOver;
        public static event Action OnGameFinish;
        
        private int m_Point = 1;
        private int m_CurrentTurn = 1;


        private void Awake()
        {
            Instance = this;

            for (var i = 0; i < contesterArray.Length; i++)
            {
                contesterArray[i].SetIndex(i);
            }
        }

        private void LetTheConfettiRain(Contester wi)
        {
            var targetPos = wi.GetZoomPoint().position + confettiOffset;

            var a = Instantiate(winnerConfetti);
            a.transform.position = targetPos;
            a.transform.localScale = Vector3.one * confettiScale;
        }
        
        private void ShowWinnerText(Contester wi)
        {
            if (wi == contesterArray[0])
            {
                var data = LocalUser.GetUserDataFromCache();
                winGroup.gameObject.SetActive(true);
                winnerText.color = Color.blue;
                winnerText.text = data.name;
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
            if (m_IsZoomStarted) return;

            m_IsZoomStarted = true;
            
            ShowWinnerText(w);
            
            LazyCoroutines.WaitForSeconds(.5f, () =>
            {
                var targetPos = w.GetZoomPoint().position + zoomOffset;
            
                camera.DOMove(targetPos, .5f).OnComplete(() =>
                {
                    LetTheConfettiRain(w);
                });
                camera.DORotate(new Vector3(30, 0, 0), .5f);
            });
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void TryOverTheGame()
        {
            if (m_CurrentTurn == maxRound)
            {
                GetSortContesterArrayByPoints();
                OnGameOver?.Invoke(this, GetSortContesterArrayByPoints());
                tourImageArray[m_CurrentTurn - 1].gameObject.SetActive(true);

                var sortContester = GetSortContesterArrayByPoints();

                Contester winnerContester = GetSortContesterArrayByPoints()[0];
                
                OnGameFinish?.Invoke();
                
                ZoomWinner(sortContester[0]);
                
                if (sortContester[0].GetContesterMovement().GetInputProvider() is PlayerInputProvider)
                {
                    sortContester[0].gameObject.SetActive(true);
                    sortContester[0].AddComponent<global::LookAtCamera>().SetMode(global::LookAtCamera.Mode.LookForward);
                    
                    SoundManager.Instance.PlayCompetitionWinSound();
                    
                    LocalUser.AddQuestProgress(QuestType.Win_Competition);
                    LocalUser.AddQuestProgress(QuestType.Win_SnowButt);
                }
                else
                {
                    sortContester[0].gameObject.SetActive(true);
                    sortContester[0].AddComponent<global::LookAtCamera>().SetMode(global::LookAtCamera.Mode.LookForward);

                    SoundManager.Instance.PlayCompetitionTourLoseSound();
                }

                return;
            }
            
            
            tourImageArray[m_CurrentTurn - 1].gameObject.SetActive(true);
            OnTurnOver?.Invoke(this, GetSortContesterArrayByPoints()[0]);

            if (GetSortContesterArrayByPoints()[0].GetContesterMovement().GetInputProvider() is PlayerInputProvider)
            {
                SoundManager.Instance.PlayCompetitionTourWinSound();
            }
            else
            {
                SoundManager.Instance.PlayCompetitionTourLoseSound();
            }
            
            
            m_Point = 1;
            m_CurrentTurn++;
            
            foreach (var contester in contesterArray)
            {
                contester.gameObject.SetActive(true);
                contester.GetContesterMovement().SetVelocity(Vector3.zero);
                contester.DestroySnowball();
                contester.SetIsDead(false);
                contester.SetActiveDeadIcon(false);
                contester.SetActiveKingIcon(false);
                contester.GetContesterMovement().SetCanMove(true);

                contester.transform.position = contester.GetFirstPosition();
            }
            
            CountdownManager.Instance.Countdown();
            SoundManager.Instance.PlayCompetitionCountdownSound();

            return;
        }


        public int GetPoint()
        {
            return m_Point;
        }


        public void AddPoint(int point)
        {
            m_Point = m_Point + point;
        }
        
        
        public Contester[] GetSortContesterArrayByPoints()
        {
            var sortContesterArray = contesterArray.OrderByDescending(contester => contester.GetPoint()).ToArray();

            
            
            return sortContesterArray;
        }


        public Contester[] GetContesterArray()
        {
            return contesterArray;
        }


        public List<Contester> GetAliveContesterList()
        {
            List<Contester> contesterList = new List<Contester>();

            foreach (var contester in contesterArray)
            {
                if (!contester.IsDead())
                {
                    contesterList.Add(contester);
                }
            }

            return contesterList;
        }
    }
}