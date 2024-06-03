using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using General;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class GameModeController : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button createCardButton;
        [SerializeField] private Button competationButton;
        [SerializeField] private GameObject downCanvases;
        [SerializeField] private Transform gameModeSelectionMainTransform;
        [SerializeField] private Transform mainCanvasesMainTransform;
        [SerializeField] private Camera mainCamera;
        
        
        public event EventHandler OnStartGameButtonClicked;
        public event EventHandler<bool> OnModeSelected; 

        public static GameModeController Instance { get; private set; }

        private bool isCompetetionSelected = true;

        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            startGameButton.onClick.AddListener(RaiseOnStartGameButtonClicked);
            backButton.onClick.AddListener(GoBackMainPage);
            
            createCardButton.onClick.AddListener(()=>
            {
                RaiseOnModeSelected(false);
            });
            competationButton.onClick.AddListener(()=>
            {
                RaiseOnModeSelected(true);
            });
        }

        private void RaiseOnModeSelected(bool e)
        {
            gameModeSelectionMainTransform.gameObject.SetActive(false);
            OnModeSelected?.Invoke(this, e);
        }

        private void RaiseOnStartGameButtonClicked()
        {
            OnStartGameButtonClicked?.Invoke(this, EventArgs.Empty );
            gameModeSelectionMainTransform.localScale = Vector3.zero;
            gameModeSelectionMainTransform.gameObject.SetActive(true);
            gameModeSelectionMainTransform.DOScale(Vector3.one, .25f);
            
            downCanvases.SetActive(false);
        }


        private void GoBackMainPage()
        {
            gameModeSelectionMainTransform.localPosition = Vector3.zero;
            gameModeSelectionMainTransform.gameObject.SetActive(false);
            
            mainCanvasesMainTransform.localScale = Vector3.zero;
            mainCanvasesMainTransform.gameObject.SetActive(true);
            mainCanvasesMainTransform.DOScale(Vector3.one, 0.35f).OnComplete(() =>
            {
                downCanvases.gameObject.SetActive(true);
            });
        }
        
            
            
    }
}
