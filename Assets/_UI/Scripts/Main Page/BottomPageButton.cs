using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class OnPageButtonClickEventArgs : EventArgs
    {
        public MenuPage CurrentMenuPage;
        public MenuPage ClickedMenuPage;
        public bool İsDifferent;
    }
    
    public class BottomPageButton : MonoBehaviour
    {
        [SerializeField] private MenuPage menuPage;
        [SerializeField] private Sprite activeImage;
        [SerializeField] private Sprite passiveImage;
        
        
        private Button button;

        public event EventHandler<OnPageButtonClickEventArgs> OnPageButtonClick;
        
        
        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void AddOnClickListener(UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
        }
        
        
        private void RaiseOnPageButtonClick()
        {
            OnPageButtonClickEventArgs args = new OnPageButtonClickEventArgs();
            
            args.CurrentMenuPage = MenuManager.GetCurrentMenuPage();
            args.ClickedMenuPage = menuPage;
            args.İsDifferent = args.ClickedMenuPage != args.CurrentMenuPage;
            
            OnPageButtonClick?.Invoke(this, args);
        }

        public void SetActiveImage()
        {
            GetComponent<Image>().sprite = activeImage;
        }

        public void SetPassiveImage()
        {
            GetComponent<Image>().sprite = passiveImage;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}