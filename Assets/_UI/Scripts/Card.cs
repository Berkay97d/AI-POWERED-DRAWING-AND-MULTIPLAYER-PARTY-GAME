using System;
using System.Collections;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using General;
using ImageUtils;
using NaughtyAttributes;
using StableDiffusionAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public enum CardRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
    
    public class Card : MonoBehaviour
    {
        [Foldout("Outline Types")] [SerializeField] private Sprite commonOutline;
        [Foldout("Outline Types")] [SerializeField] private Sprite rareOutline;
        [Foldout("Outline Types")] [SerializeField] private Sprite epicOutline;
        [Foldout("Outline Types")] [SerializeField] private Sprite legendaryOutline;
        [SerializeField] private Image outlinePlace;
        [SerializeField] private Image artImagePlace;
        [SerializeField] private TMP_Text pointText;
        [SerializeField] private RectTransform tappedTransform;
        [SerializeField] private RectTransform normalRect;

        public event EventHandler<Card> OnCardClicked;
        
        private string cardId;
        private CardData cardData;
        private Button button;
        private Sprite cardSprite;
        private Coroutine coroutine;
        private CardSlot myCardSlot;
        
        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(RaiseOnCardClicked);
        }

        private void OnDestroy()
        {
            LazyCoroutines.StopCoroutine(coroutine);
        }


        public RectTransform GetNormalRect()
        {
            return normalRect;
        }

        public int GetUpgradeCoinCost()
        {
            return cardData.Rarity switch
            {
                CardRarity.Common => 500,
                CardRarity.Rare => 1000,
                CardRarity.Epic => 2000,
                CardRarity.Legendary => 4000
            };
        }

        public int GetUpgradeTokenCost()
        {
            return cardData.Rarity switch
            {
                CardRarity.Common => 1,
                CardRarity.Rare => 1,
                CardRarity.Epic => 1,
                CardRarity.Legendary => 1
            };
        }

        public int GetDemolishCoinValue()
        {
            return cardData.Rarity switch
            {
                CardRarity.Common => 250,
                CardRarity.Rare => 500,
                CardRarity.Epic => 1000,
                CardRarity.Legendary => 2000
            };
        }

        public int GetDemolishTokenValue()
        {
            return cardData.Rarity switch
            {
                CardRarity.Common => 1,
                CardRarity.Rare => 1,
                CardRarity.Epic => 1,
                CardRarity.Legendary => 1
            };
        }
        

        private void RaiseOnCardClicked()
        {
            OnCardClicked?.Invoke(this, this);
        }
        
        private void AdjustOutline()
        {
            switch (cardData.Rarity)
            {
                case CardRarity.Common:
                    outlinePlace.sprite = commonOutline;
                    break;
                case CardRarity.Rare:
                    outlinePlace.sprite = rareOutline;
                    break;
                case CardRarity.Epic:
                    outlinePlace.sprite = epicOutline;
                    break;
                case CardRarity.Legendary:
                    outlinePlace.sprite = legendaryOutline;
                    break;
                
            }
        }


        public RectTransform GetTappedTransform()
        {
            return tappedTransform;
        }


        public void ChangeOutlineVisual(CardRarity rarity)
        {
            switch (rarity)
            {
                case CardRarity.Common:
                    outlinePlace.sprite = commonOutline;
                    break;
                case CardRarity.Rare:
                    outlinePlace.sprite = rareOutline;
                    break;
                case CardRarity.Epic:
                    outlinePlace.sprite = epicOutline;
                    break;
                case CardRarity.Legendary:
                    outlinePlace.sprite = legendaryOutline;
                    break;
                
            }
        }

        private void AdjustCardImage()
        {
            coroutine = LazyCoroutines.WaitForFrame(() =>
            {
                DatabaseAPI.GetImageByID(cardData.imageID, data =>
                {
                    var a = TextureUtils.FromBase64String(data.base64_image, 320, 320);
                    artImagePlace.sprite = a.ToSprite();
                });
            });
        }

        private void AdjustCardPoint()
        {
            pointText.text = cardData.score.ToString();
        }

        public void ChangeCardPointVisual(int earnPoint)
        {
            pointText.text = (cardData.score + earnPoint).ToString();
        }

        public void SetCardData(CardData cData)
        {
            cardData = cData;
            
            
            AdjustOutline();
            AdjustCardPoint();
            AdjustCardImage();
        }

        public CardData GetCardData()
        {
            return cardData;
        }

        public void SetCardId(string id)
        {
            cardId = id;
        }

        public string GetCardId()
        {
            return cardId;
        }

        public void SetMyCardSlot(CardSlot slot)
        {
            myCardSlot = slot;
        }

        public CardSlot GetMyCardSlot()
        {
            return myCardSlot;
        }
        

        
        
        
    }
}