using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class NavigationElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text pointText;
        [SerializeField] private RectTransform rect;
        [SerializeField] private NotificationImage notificationImage;
        [SerializeField] private new Collider collider;
        
        public event EventHandler<CardPage> OnClick;
        
        private CardPage cardPage;


        public void SetInteractable(bool value)
        {
            collider.enabled = value;
        }
        
        public void SetCardPage(CardPage c)
        {
            cardPage = c;
            cardPage.OnPagePointChanged += OnCardPagePointChanged;
            
            UpdateCardPageInfo(c);
        }

        public RectTransform GetRect()
        {
            return rect;
        }

        private void OnCardPagePointChanged(object sender, EventArgs e)
        {
            UpdateCardPageInfo(cardPage);
        }

        private void UpdateCardPageInfo(CardPage c)
        {
            titleText.text = c.GetSubTitle().Name;
            pointText.text = c.GetPagePoint().ToString();
        }

        private void OnMouseDown()
        {
            OnClick?.Invoke(this, cardPage);
        }

        public CardPage GetCardPage()
        {
            return cardPage;
        }

        public NotificationImage GetNotificationImage()
        {
            return notificationImage;
        }
    }
}