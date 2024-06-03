using System;
using System.Collections;
using System.Collections.Generic;
using _MenuSwipe.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{


    public class GamblingButton : MonoBehaviour
    {
        [SerializeField] private MenuSwipeManager menuSwipeManager;
        [SerializeField] private Transform mainTransform;

        public static GamblingButton Instance { get; private set; }

        private Button button;
        private bool isGamblePageActive;


        private void Awake()
        {
            Instance = this;

            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }


        private void OnClick()
        {
            menuSwipeManager.SnapToMenuPanel(6);
            menuSwipeManager.SetDragable(false);
        }
        

        private void MoveGamblePageMiddle()
        {
            MenuManager.GetCurrentMenuPage().MoveDown();
            mainTransform.DOMove(MenuManager.GetCurrentMenuPage().GetMiddlePageTransform().position, .6f);

            MenuManager.GetCurrentMenuPage().ButtonDisactive();
            isGamblePageActive = true;
        }

        public void MoveGamblePageUp()
        {
            mainTransform.DOMove(MenuManager.GetCurrentMenuPage().GetUpPageTransform().position, .6f);
            isGamblePageActive = false;
        }

        public bool GetIsGamblePageActive()
        {
            return isGamblePageActive;
        }
    }
}