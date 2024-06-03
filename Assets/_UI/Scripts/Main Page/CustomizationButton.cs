using _MenuSwipe.Scripts;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace UI
{
    public class CustomizationButton : MonoBehaviour
    {
        [SerializeField] private BottomPageButtonManager bottomPageButtonManager;
        [SerializeField] private MenuSwipeManager menuSwipeManager;
        [SerializeField] private Transform mainTransform;
        [SerializeField] private GameObject downPageCanvases;
        [SerializeField] private Button closeButton;


        public static CustomizationButton Instance { get; private set; }

        private Button button;
        private bool isCustomizationPageActive;


        private void Awake()
        {
            Instance = this;

            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(OnClick);
            closeButton.onClick.AddListener(OnClickClose);
        }


        private void OnClick()
        {
            menuSwipeManager.SnapToMenuPanel(5);
            menuSwipeManager.SetDragable(false);
            bottomPageButtonManager.SetActiveButtons(false);
        }

        private void OnClickClose()
        {
            menuSwipeManager.SnapToMenuPanel(2);
            menuSwipeManager.SetDragable(true);
            bottomPageButtonManager.SetActiveButtons(true);
        }
        
        
        private void GetBackMainPage()
        {
            if (GetIsCustomizationPageActive())
            {
                MoveCustomizationPageUp();
                MenuManager.GetMiddlePage().MoveMiddle();
                downPageCanvases.SetActive(true);
            }
        }
        
        private void MoveCustomizationPageMiddle()
        {
            MenuManager.GetCurrentMenuPage().MoveDown();
            
            mainTransform.DOMove(MenuManager.GetCurrentMenuPage().GetMiddlePageTransform().position, .4f);

            downPageCanvases.SetActive(false);
            
            MenuManager.GetCurrentMenuPage().ButtonDisactive();
            isCustomizationPageActive = true;
        }

        public void MoveCustomizationPageUp()
        { 
            mainTransform.DOMove(MenuManager.GetCurrentMenuPage().GetUpPageTransform().position, .4f);

            downPageCanvases.SetActive(false);
            
            isCustomizationPageActive = false;
        }

        public bool GetIsCustomizationPageActive()
        {
            return isCustomizationPageActive;
        }
    }
}