using System;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class CollectionPageCardInitilazor : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform cardParent;
        [SerializeField] private Book book;
        
        
        private readonly List<Card> initedCards = new();
        

        private void Awake()
        {
            LocalUser.OnCardDatasLoaded += OnCardDatasLoaded;
            LocalUser.OnCardDatasChanged += OnCardDatasChanged;
        }

        private void Start()
        {
            book.OnActivePageChanged += OnActivePageChanged;
        }

        private void OnActivePageChanged(object sender, string e)
        {
            foreach (var card in initedCards)
            {
                card.gameObject.SetActive(card.GetCardData().subTitle == e);
            }
        }

        private void OnDestroy()
        {
            LocalUser.OnCardDatasLoaded -= OnCardDatasLoaded;
            LocalUser.OnCardDatasChanged -= OnCardDatasChanged;
            book.OnActivePageChanged -= OnActivePageChanged;
        }

        private void OnCardDatasLoaded(DatabaseAPI.CardPostResponseData[] cardDatas)
        {
            SyncPrefabsWithData(cardDatas);

            var pages = book.GetAllCardPages();
            var cards = initedCards;
            foreach (var card in cards)
            {
                if (card.GetCardData().inCollection == 1)
                {
                    foreach (var page in pages)
                    {
                        if (page.IsCardBelongThisPage(card))
                        {
                            PutCardToPageSlot(card, page);
                        }
                    }
                }
            }
        }


        public Card GetCard(int index)
        {
            return initedCards[index];
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
                var card = Instantiate(cardPrefab, cardParent);
            
                initedCards.Add(card);
                
                card.OnCardClicked += OnCardClicked;
            }
        }

        private void OnCardClicked(object sender, Card e)
        {
            if (book.GetActivePage().TryGetComponent(out CardPage cardPage))
            {
                if (cardPage.IsCardInPage(e))
                {
                    RemoveCardFromPageSlot(e, cardPage);
                    return;
                }
                
                PutCardToPageSlot(e, cardPage);
                
            }
        }

        private void PutCardToPageSlot(Card c, CardPage p)
        {
            p.AddCardToPageSlot(c);

            var trans = p.GetNextFitCardSlot();  //TODO KARTLARIN AYNI YUVAYA GİTME BUG'I
            var slot = trans.GetComponent<CardSlot>();
            slot.SetIsFull(true);
            c.SetMyCardSlot(slot);
            
            c.transform.DOMove(trans.position, .2f).OnComplete((() =>
            {
                c.transform.SetParent(trans);
                c.transform.localPosition = Vector3.zero;
            }));

            var data = c.GetCardData();
            data.inCollection = 1;
            LocalUser.UpdateCard(c.GetCardId(), data, () =>
            {
                Debug.Log("SAYFAYA KOYDUM DATASI GİTTİ");
            });
            Debug.Log("SAYFAYA KOYDUM");
        }

        private void RemoveCardFromPageSlot(Card e, CardPage p)
        {
            p.RemoveCardFromPageSlot(e);
            e.GetMyCardSlot().SetIsFull(false);
                
            e.transform.DOMove(cardParent.position, .2f).OnComplete((() =>
            {
                e.transform.SetParent(cardParent);
            }));
                
            var dat = e.GetCardData();
            dat.inCollection = 0;
            LocalUser.UpdateCard(e.GetCardId(), dat);
            return;
        }

        private void RemoveCardByCount(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var card = initedCards[0];
                
                card.OnCardClicked -= OnCardClicked;
                
                Destroy(card.gameObject);
                initedCards.RemoveAt(0);
            }
        }

        public List<Card> GetInitedCards()
        {
            return initedCards;
        }
    }
}