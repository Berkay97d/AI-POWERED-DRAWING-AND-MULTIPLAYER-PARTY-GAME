using System;
using _MenuSwipe.Scripts;
using UnityEngine;

namespace UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private BottomPageButton[] bottomPageButtons;
        [SerializeField] private MenuPage[] menuPages;
        [SerializeField] private Transform mainMenuCanvasesTransform;
        
        
        private static MenuManager Instance { get; set; }
        private static MenuSwipeManager menuSwapManager;
        private MenuPage currentMenuPage;
        
        private void Awake()
        {
            Instance = this;
            menuSwapManager = FindObjectOfType<MenuSwipeManager>();

            foreach (var button in bottomPageButtons)
            {
                button.OnPageButtonClick += OnPageButtonClick;
            }

            currentMenuPage = menuPages[2];
            currentMenuPage.ButtonActive();
        }

        private void Start()
        {
            GameModeController.Instance.OnStartGameButtonClicked += OnStartGameButtonClicked;
        }

        private void OnDestroy()
        {
            GameModeController.Instance.OnStartGameButtonClicked -= OnStartGameButtonClicked;
        }

        private void OnStartGameButtonClicked(object sender, EventArgs eventArgs)
        {
            mainMenuCanvasesTransform.gameObject.SetActive(false);
        }

        private void OnPageButtonClick(object sender, OnPageButtonClickEventArgs e)
        {
            if (currentMenuPage == menuPages[3] && e.ClickedMenuPage != menuPages[3])
            {
                CollectionMenuController.RaiseOnCollectionPageGone();
            }
            if (currentMenuPage != menuPages[3] && e.ClickedMenuPage == menuPages[3])
            {
                CollectionMenuController.RaiseOnCollectionPageArrive();
            }
            
            
            if (GamblingButton.Instance.GetIsGamblePageActive())
            {
                GamblingButton.Instance.MoveGamblePageUp();
                e.ClickedMenuPage.TakePositionAtDown();
                e.ClickedMenuPage.MoveMiddleNoEase();
                
                e.ClickedMenuPage.ButtonActive();
                currentMenuPage = e.ClickedMenuPage;
                return;
            }
            
            if (!e.İsDifferent) return;

            if (e.CurrentMenuPage.GetPageNumber() > e.ClickedMenuPage.GetPageNumber())
            {
                e.ClickedMenuPage.TakePositionAtLeft();
                e.CurrentMenuPage.MoveRight();
                e.ClickedMenuPage.MoveMiddle();
                
                e.CurrentMenuPage.ButtonDisactive();
                e.ClickedMenuPage.ButtonActive();
            }
            else
            {
                e.ClickedMenuPage.TakePositionAtRight();
                e.CurrentMenuPage.MoveLeft();
                e.ClickedMenuPage.MoveMiddle();
                
                e.CurrentMenuPage.ButtonDisactive();
                e.ClickedMenuPage.ButtonActive();
            }

            currentMenuPage = e.ClickedMenuPage;
        }

        public static MenuPage GetCurrentMenuPage()
        {
            return Instance.currentMenuPage;
        }

        public static MenuPage GetMiddlePage()
        {
            Instance.currentMenuPage = Instance.menuPages[2];
            Instance.currentMenuPage.ButtonActive();
            return Instance.menuPages[2];
        }

        public static MenuPage GetCollectionPage()
        {
            return Instance.menuPages[3];
        }

        public static bool IsCraftMenuActive()
        {
            return menuSwapManager.GetActivePanelIndex() == 1;
        }

        public static bool IsCollectionMenuActive()
        {
            return menuSwapManager.GetActivePanelIndex() == 3;

            //craft 1 collection 3
        }

        public static bool IsMainMenuActive()
        {
            return menuSwapManager.GetActivePanelIndex() == 2;
        }
        
        
    }
}