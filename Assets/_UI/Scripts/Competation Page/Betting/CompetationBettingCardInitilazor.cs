using System;
using System.Collections.Generic;
using System.Linq;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using UnityEngine;

namespace UI
{
    public class CompetationBettingCardInitilazor : MonoBehaviour, ICardList
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform unbettedCardParent;
        [SerializeField] private Transform bettedCardTransform;

        public event EventHandler<int> OnBetCardsChanged; 

        private readonly List<Card> bettedCards = new();

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


        public Card GetCard(int index)
        {
            return initedCards[index];
        }
        
        
        private void OnCardDatasLoaded(DatabaseAPI.CardPostResponseData[] cardDatas)
        {
            SyncPrefabsWithData(cardDatas);
        }

        private void OnCardDatasChanged(DatabaseAPI.CardPostResponseData[] cardDatas)
        {
            SyncPrefabsWithData(cardDatas);
        }

        private void SyncPrefabsWithData(DatabaseAPI.CardPostResponseData[] cardDatas)
        {
            FixCardCount(cardDatas);
            SetCardDatas(cardDatas);
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
                var card = Instantiate(cardPrefab, unbettedCardParent);
            
                initedCards.Add(card);
                
                card.OnCardClicked += OnCardClicked;
            }
        }
        
        private void RemoveCardByCount(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var card = initedCards[0];
                Destroy(card.gameObject);
                initedCards.RemoveAt(0);

                card.OnCardClicked -= OnCardClicked;
            }
        }
        
        private void TakeCardToBet(Card ca)
        {
            ca.transform.SetParent(bettedCardTransform);
            bettedCards.Add(ca);
            
            OnBetCardsChanged?.Invoke(this, bettedCards.Count);
        }

        private void TakeCardBackFromBet(Card ca)
        {
            ca.transform.SetParent(unbettedCardParent);
            bettedCards.Remove(ca);
            
            OnBetCardsChanged?.Invoke(this, bettedCards.Count);
        }

        public void TakeAllCardsBackFromBet()
        {
            foreach (var card in initedCards)
            {
                if (bettedCards.Contains(card))
                {
                    TakeCardBackFromBet(card);
                }
            }
        }
        
        private void OnCardClicked(object sender, Card e)
        {
            if (!bettedCards.Contains(e))
            {
                if (bettedCards.Count >= 1)
                {
                    return;
                }
                
                TakeCardToBet(e);
                return;
            }
            
            TakeCardBackFromBet(e);
        }
        
        public string[] GetBet()
        {
            return bettedCards
                .Select(card => card.GetCardId())
                .ToArray();
        }
        
        public List<Card> GetCards()
        {
            return initedCards;
        }
    }
}