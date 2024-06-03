using System;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CardPage : Page
    {
        [SerializeField] private CardSlot[] cardSlot;
        [SerializeField] private TMP_Text pageTitleText;
        [SerializeField] private TMP_Text pagePointText;
        [SerializeField] private NavigationPageTransferButton navigationPageTransferButton;
        
        public event EventHandler OnPagePointChanged;
        
        private readonly List<Card> cardsInPage = new();
        private int pagePoint = 0;
        


        protected void Awake()
        {
            LocalUser.OnCardDatasChanged += OnUserDatasChanged;
            CalculatePagePoint();
        }

        private void OnDestroy()
        {
            LocalUser.OnCardDatasLoaded -= OnUserDatasChanged;
        }
        
        private void Start()
        {
            pageTitleText.text = subTitle.Name;
        }
        
        private void OnUserDatasChanged(DatabaseAPI.CardPostResponseData[] obj)
        {
            CalculatePagePoint();
        }

        public Transform GetNextFitCardSlot()
        {
            foreach (var slot in cardSlot) //TODO Kartların aynı yuvaya gitme bug'ı
            {
                if (!slot.GetIsFull())
                {
                    return slot.transform;
                }
            }

            throw new Exception("THERE IS NOT SUITABLE SLOT");
        }
        
        
        public void AddCardToPageSlot(Card c)
        {
            cardsInPage.Add(c);
            
            CalculatePagePoint();
        }

        public void RemoveCardFromPageSlot(Card c)
        {
            cardsInPage.Remove(c);
            
            CalculatePagePoint();
        }

        public bool IsCardInPage(Card c)
        {
            return cardsInPage.Contains(c);
        }

        public int GetPagePoint()
        {
            return pagePoint;
        }
        
        private void CalculatePagePoint()
        {
            var startPoint = pagePoint;
            
            pagePoint = 0;
            
            foreach (var card in cardsInPage)
            {
                pagePoint += card.GetCardData().score;
            }

            pagePointText.text = pagePoint.ToString();

            var pointChange = startPoint - pagePoint;
            OnPagePointChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public bool IsCardBelongThisPage(Card card)
        {
            if (card.GetCardData().subTitle == subTitle.Name)
            {
                return true;
            }

            return false;
        }

        public void SetInteractableNavigationTransferButton(bool value)
        {
            navigationPageTransferButton.SetInteractable(value);
        }

        public NavigationPageTransferButton GetNavigationPageTransferButton()
        {
            return navigationPageTransferButton;
        }
        
    }
}