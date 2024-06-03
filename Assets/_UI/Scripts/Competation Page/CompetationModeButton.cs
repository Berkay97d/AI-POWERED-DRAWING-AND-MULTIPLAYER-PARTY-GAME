using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnterAndExitUI_Competation.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum GameMode
    {
        Stack,
        DontFall,
        FastEyes,
        Snow,
        Katana,
        Laser
    }
    
    public class CompetationModeButton : MonoBehaviour
    {
        [SerializeField] private GameMode gameMode;
        [SerializeField] private Transform selectionMainTransform;
        [SerializeField] private Transform cardBettingMainTransform;
        [SerializeField] private GameObject upInfoCanvas;
        [SerializeField] private CompetationMenuController competationMenuController;
        [SerializeField] private EnterTest_Script enterTest;
        


        private Button button;
        
        
        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(GetBettingPage);
        }

        private void GetBettingPage()
        {
            CompetationBettingController.isBettingPageActive = true;   
            
            competationMenuController.SetSelecetedGameMode(gameMode);
            enterTest.SetGameMode(gameMode);
            ExitTest_Script.SetGameMode(gameMode);
            
            selectionMainTransform.localScale = Vector3.zero;
            cardBettingMainTransform.localScale = Vector3.zero;
            cardBettingMainTransform.gameObject.SetActive(true);
            cardBettingMainTransform.DOScale(Vector3.one, .4f).SetEase(Ease.OutBack);
            upInfoCanvas.SetActive(false);
            selectionMainTransform.gameObject.SetActive(false);
        }

        public GameMode GetGameMode()
        {
            return gameMode;
        }
        
    }
}