using System;
using System.Linq;
using _Compatetion.Jumping.Scripts.Contesters;
using _Compatetion.Jumping.Scripts.Obstacles;
using _SoundSystem.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using General;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Compatetion.Jumping.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private Contester[] contesterArray;
        [SerializeField] private Transform camera;
        [SerializeField] private Vector3 zoomOffset;
        [SerializeField] private GameObject winGroup;
        [SerializeField] private TMP_Text winnerText;
        [SerializeField] private GameObject winnerConfetti;
        [SerializeField] private Vector3 confettiOffset;
        [SerializeField] private float confettiScale;

        private bool isZoomStarted = false;
        public event Action<Contester[]> OnGameOver;
        
        private int m_Point;
        
        private void Awake()
        {
            Instance = this;

            for (int i = 0; i < contesterArray.Length; i++)
            {
                contesterArray[i].SetIndex(i);
            }
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public void AddPoint(int number)
        {
            m_Point += number;
        }

        public void TryGameOver()
        {
            if (m_Point != 7) return;
            foreach (var obstacle in ObstacleManager.Instance.GetObstacleArray())
            {
                obstacle.UndoScale();
            }
            
            foreach (var contester in contesterArray)
            {
                if (!contester.GetIsDead())
                {
                    m_Point++;
                    
                    contester.SetPoint(m_Point);
                    contester.SetText(m_Point.ToString());
                    contester.SetActiveKingIcon(true);

                    var sortedContesters = GetSortContesterArrayByPoints();
                        
                    GameOver(sortedContesters);
                    ZoomWinner(sortedContesters[0]);

                    if (contester.GetContesterType() == ContesterType.Player)
                    {
                        SoundManager.Instance.PlayCompetitionWinSound();
                        
                        var lookAtCamera = contester.AddComponent<LookAtCamera>();
                        lookAtCamera.SetMode(LookAtCamera.Mode.LookForward);
                        lookAtCamera.SetInvert(true);
                    }
                    else
                    {
                        SoundManager.Instance.PlayCompetitionLoseSound();
                        
                        var lookAtCamera = contester.AddComponent<LookAtCamera>();
                        lookAtCamera.SetMode(LookAtCamera.Mode.LookForward);
                        lookAtCamera.SetInvert(true);
                    }
                }
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
            if (wi == contesterArray[0])
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
            
            LazyCoroutines.WaitForSeconds(.5f, () =>
            {
                var targetPos = w.transform.position + zoomOffset;
            
                camera.DOMove(targetPos, .5f).OnComplete(() =>
                {
                    LetTheConfettiRain(w);
                });
            });
        }


        public int GetPoint()
        {
            return m_Point;
        }


        private Contester[] GetSortContesterArrayByPoints()
        {
            var sortContesterArray = contesterArray
                .OrderByDescending(contester => contester.GetPoint())
                .ToArray();

            foreach (var contester in sortContesterArray)
            {
            }
            
            return sortContesterArray;
        }

        private void GameOver(Contester[] e)
        {
            var playerOrder = e
                .Select(contester => contester.GetIndex())
                .ToArray();
                
            LazyCoroutines.WaitForSeconds(5f, () =>
            {
                ExitTest_Script.SetPlayerOrder(playerOrder);
                SceneManager.LoadScene(6);
            });
            
            OnGameOver?.Invoke(e);
        }


        public Contester[] GetContesterArray()
        {
            return contesterArray;
        }
    }
}