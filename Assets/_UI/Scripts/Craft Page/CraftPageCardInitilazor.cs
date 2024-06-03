using System;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CraftPageCardInitilazor : MonoBehaviour, ICardList
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform cardParent;
        [SerializeField] private TMP_Text allCardsCountText;
        [SerializeField] private RectTransform content;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private CardSorter cardSorter;
        [SerializeField] private float rowHeight = 358f;


        public event EventHandler<Card> OnCardAddedToCraftPage;
        public event EventHandler<Card> OnCardRemovedFromCraftPage;

        
        private readonly List<Card> initedCards = new();


        private void Awake()
        {
            LocalUser.OnCardDatasLoaded += OnCardDatasLoaded;
            LocalUser.OnCardDatasChanged += OnCardDatasChanged;
        }

        private void OnDestroy()
        {
            LocalUser.OnCardDatasLoaded -= OnCardDatasLoaded;
            LocalUser.OnCardDatasChanged -= OnCardDatasChanged;
        }

        private void OnCardDatasLoaded(DatabaseAPI.CardPostResponseData[] cardDatas)
        {
            SyncPrefabsWithData(cardDatas);
            cardSorter.SortCards();
        }

        private void OnCardDatasChanged(DatabaseAPI.CardPostResponseData[] cardDatas)
        {
            SyncPrefabsWithData(cardDatas);
            cardSorter.SortCards();
        }


        public Card GetCard(int index)
        {
            return initedCards[index];
        }
        
        public List<Card> GetCards()
        {
            return initedCards;
        }
        

        private void SyncPrefabsWithData(DatabaseAPI.CardPostResponseData[] cardDatas)
        {
            FixCardCount(cardDatas);
            SetCardDatas(cardDatas);

            allCardsCountText.text = "ALL CARDS: " + cardDatas.Length;
        }

        private void SetCardDatas(DatabaseAPI.CardPostResponseData[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                initedCards[i].SetCardData(arr[i].card_data);
                initedCards[i].SetCardId(arr[i].card_id);
            }
        }

        private void FixCardCount(DatabaseAPI.CardPostResponseData[] arr)
        {
            var cardDataCount = arr.Length;
            var count = cardDataCount - initedCards.Count;

            if (count > 0)
            {
                AddCardByCount(count);
                return;
            }

            RemoveCardByCount(-count);
        }

        private void AddCardByCount(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var card = Instantiate(cardPrefab, cardParent);
            
                initedCards.Add(card);
                
                OnCardAddedToCraftPage?.Invoke(this, card);
            }
            
            UpdateContentHeight();
        }

        private void RemoveCardByCount(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var card = initedCards[0];
                
                OnCardAddedToCraftPage?.Invoke(this, card);
                
                Destroy(card.gameObject);
                initedCards.RemoveAt(0);
            }
            
            UpdateContentHeight();
        }

        private void UpdateContentHeight()
        {
            var sizeDelta = content.sizeDelta;
            sizeDelta.y = GetContentHeight();
            content.sizeDelta = sizeDelta;
        }

        private int GetContentHeight()
        {
            var cardPerRow = gridLayoutGroup.constraintCount;
            var isWhole = initedCards.Count % cardPerRow == 0;
            var columnCount = (initedCards.Count / cardPerRow);
            columnCount += isWhole ? 0 : 1;
            var height = columnCount * rowHeight;
            return Mathf.RoundToInt(height + 625f);
        }
    }
}