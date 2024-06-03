using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EnterAndExitUI_Competation.Scripts
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameField;
        [SerializeField] private TMP_Text levelField;
        [SerializeField] private Image banner;
        [SerializeField] private Image profilePhoto;
        [SerializeField] private CardPreview[] cardPreviews;
        [SerializeField] private Sprite[] banners;
        [SerializeField] private TMP_Text cardCountField;
        [SerializeField] private GameObject betMainObject;
        [SerializeField] private GameObject gainMainObject;
        [SerializeField] private RectTransform gainSlotsParent;

        private CardData[] bet;
        private readonly List<CardPreview> m_Gains = new();


        public void SetName(string _name)
        {
            nameField.text = _name;
        }

        public void SetLevel(int level)
        {
            levelField.text = level.ToString();
        }

        public void SetProfilePhoto(Sprite sprite)
        {
            profilePhoto.sprite = sprite;
        }

        public CardData[] GetBet()
        {
            return bet;
        }

        public void SetBet(CardData[] cardDatas)
        {
            bet = cardDatas;
            
            for (var i = 0; i < cardPreviews.Length; i++)
            {
                var isActive = i < cardDatas.Length;
                cardPreviews[i].SetActive(isActive);
                
                if (!isActive) continue;

                cardPreviews[i].SetCardData(cardDatas[i]);
                
            }

            SetCardCount(cardDatas.Length);
        }

        public void SetCardCount(int value)
        {
            cardCountField.text = $"x{value}";
        }

        public void AddGain(CardPreview cardPreview)
        {
            var slot = gainSlotsParent.GetChild(m_Gains.Count);
            var position = slot.position;
            
            cardPreview.MoveTo(position, 0.5f, () =>
            {
                cardPreview.SetParent(slot);
            });
            m_Gains.Add(cardPreview);
            SetCardCount(m_Gains.Count);
        }

        public void SetActiveBetObject(bool value)
        {
            betMainObject.SetActive(value);
        }

        public void SetActiveGainObject(bool value)
        {
            gainMainObject.SetActive(value);
        }

        public void SetBannerIndex(int index)
        {
            banner.sprite = banners[index];
        }

        public void CollectCardPreviews(Vector3 position, float duration)
        {
            foreach (var cardPreview in cardPreviews)
            {
                cardPreview.Collect(position, duration);
            }
        }

        public CardPreview GetCardPreview(int index)
        {
            return cardPreviews[index];
        }

        public int GetCardPreviewCount()
        {
            return cardPreviews.Length;
        }

        public int GetGainCount()
        {
            return gainSlotsParent.childCount;
        }

        public void SetSiblingIndex(int index)
        {
            transform.SetSiblingIndex(index);
        }
        
        
    }
}