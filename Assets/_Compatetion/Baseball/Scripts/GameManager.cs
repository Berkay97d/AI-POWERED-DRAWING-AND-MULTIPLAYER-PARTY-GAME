using System;
using System.Linq;
using _SoundSystem.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using General;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Compatetion.Baseball.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private Contester[] contesterArray;
        [SerializeField] private int round;
        [SerializeField] private Transform camera;
        [SerializeField] private Vector3 zoomOffset;
        [SerializeField] private GameObject winGroup;
        [SerializeField] private TMP_Text winnerText;
        [SerializeField] private GameObject winnerConfetti;
        [SerializeField] private Vector3 confettiOffset;
        [SerializeField] private float confettiScale;

        private bool isZoomStarted = false;
        
        
        private void Awake()
        {
            Instance = this;

            for (var i = 0; i < contesterArray.Length; i++)
            {
                contesterArray[i].SetIndex(i);
            }
        }


        private void Start()
        {
            SoundManager.Instance.PlayKatanaBackgroundSound();
        }


        public int GetRound()
        {
            return round;
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public void GameOver()
        {
            var contesterOrder = GetSortContesterArrayByPoints();
            
            if (contesterOrder[0].GetContesterType() == ContesterType.Player)
            {
                SoundManager.Instance.PlayCompetitionWinSound();
            }
            else
            {
                contesterOrder[0].AddComponent<LookAtCamera>();
                SoundManager.Instance.PlayCompetitionLoseSound();
            }
            
            ZoomWinner(contesterOrder[0]);

            var playerOrder = contesterOrder
                .Select(contester => contester.GetIndex())
                .ToArray();

            LazyCoroutines.WaitForSeconds(6f, () =>
            {
                ExitTest_Script.SetPlayerOrder(playerOrder);
                SceneManager.LoadScene(6);
            });
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


        private Contester[] GetSortContesterArrayByPoints()
        {
            return contesterArray
                .OrderByDescending(contester => contester.GetPoint())
                .ToArray();
        }
    }
}