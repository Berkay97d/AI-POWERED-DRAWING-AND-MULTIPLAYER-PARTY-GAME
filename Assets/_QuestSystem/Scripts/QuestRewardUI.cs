using _Market.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _QuestSystem.Scripts
{
    public class QuestRewardUI : MonoBehaviour
    {
        [SerializeField] private Sprite[] currencySprites;
        [SerializeField] private Color[] currencyColors;
        [SerializeField] private Image currencyIcon;
        [SerializeField] private TMP_Text countField;
        
        
        public void SetReward(int count, CurrencyType currencyType)
        {
            SetCount(count);
            SetCountTextColor(GetCurrencyTextColor(currencyType));
            SetCurrencyType(currencyType);
        }


        private void SetCount(int count)
        {
            countField.text = count.ToString();
        }

        private void SetCountTextColor(Color color)
        {
            countField.color = color;
        }
        
        private void SetCurrencyType(CurrencyType currencyType)
        {
            currencyIcon.sprite = GetCurrencySprite(currencyType);
        }

        private Sprite GetCurrencySprite(CurrencyType currencyType)
        {
            return currencySprites[(int) currencyType];
        }

        private Color GetCurrencyTextColor(CurrencyType currencyType)
        {
            return currencyColors[(int) currencyType];
        }
    }
}