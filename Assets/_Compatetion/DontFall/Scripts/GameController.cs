using System;
using System.Collections;
using System.Collections.Generic;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using General;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace DontFall
{
    
    
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Contester[] contesters;
        [SerializeField] private Transform camera;
        [SerializeField] private Vector3 zoomOffset;
        [SerializeField] private GameObject winGroup;
        [SerializeField] private TMP_Text winnerText;
        [SerializeField] private GameObject winnerConfetti;
        [SerializeField] private Vector3 confettiOffset;
        [SerializeField] private float confettiScale;
        public event EventHandler<List<Contester>> OnGameOver;
        public static GameController Instance { get; private set; }
        
        private List<Contester> siralama = new List<Contester>();
        private bool isGameOver = false;
        private bool isSiralamaDone = false;
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
            foreach (var contester in contesters)
            {
                contester.OnContesterDrop += OnContesterDrop;
            }
        }

        private void OnContesterDrop(object sender, Contester e)
        {
            if (isSiralamaDone)
            {
                return;
            }
            
            e.SetIsDead(true);
            siralama.Add(e);

            var deadNumber = 0;
            foreach (var contester in contesters)
            {
                if (contester.GetIsDead())
                {
                    deadNumber++;
                }
            }

            if (deadNumber >= 7)
            {
                if (deadNumber == 7)
                {
                    foreach (var VARIABLE in contesters)
                    {
                        if (!VARIABLE.GetIsDead())
                        {
                            siralama.Add(VARIABLE);
                        }
                    }
                }
                
                isSiralamaDone = true;
                isGameOver = true;
                var list = DoSiralama();
                if (list[0] is PlayerController)
                {
                    OnWin();
                }
                else
                {
                    OnLose();
                }
                OnGameOver?.Invoke(this, list);
            }
            
        }

        private void OnWin()
        {
            LocalUser.AddQuestProgress(QuestType.Win_Competition);
            LocalUser.AddQuestProgress(QuestType.Win_DontFall);
        }

        private void OnLose()
        {
            
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

        public void ZoomWinner(Contester w)
        {
            if (isZoomStarted) return;

            isZoomStarted = true;
            
            ShowWinnerText(w);
            
            LazyCoroutines.WaitForSeconds(.5f, () =>
            {
                var targetPos = w.transform.position + zoomOffset;
            
                camera.DOMove(targetPos, 1.25f).OnComplete(() =>
                {
                    LetTheConfettiRain(w);
                });
            });
        }

        public bool GetIsGameOver()
        {
            return isGameOver;
        }

        private List<Contester> DoSiralama()
        {
            siralama.Reverse();
            return siralama;
        }
        
        
    }
}