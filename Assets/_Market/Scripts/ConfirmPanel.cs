using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Market.Scripts
{
    public class ConfirmPanel : MonoBehaviour
    {
        [SerializeField] private Sprite[] currencySprites;
        [SerializeField] private TMP_Text titleField;
        [SerializeField] private TMP_Text amountField;
        [SerializeField] private TMP_Text costField;
        [SerializeField] private Image currencyImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject freeText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button confirmButton;


        public UnityEvent onOpened;
        public UnityEvent onClosed;
        

        private Action m_OnConfirmed;
        

        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
            confirmButton.onClick.AddListener(() =>
            {
                m_OnConfirmed?.Invoke();
                Hide();
            });
        }


        public void OnNotEnoughCoins()
        {
            ScreenLogger.Log(LogMessageContainer.NotEnoughCoin);
        }

        public void OnNotEnoughGems()
        {
            ScreenLogger.Log(LogMessageContainer.NotEnoughGem);
        }
        
        public void SyncWithBuyButton(MarketBuyButton buyButton)
        {
            SetTitle(buyButton.GetTitle());
            SetIcon(buyButton.GetIcon());
            SetIconColor(buyButton.GetIconColor());
            SetAmount(buyButton.GetAmount());
            SetCost(buyButton.GetCost());
            SetCostCurrency(buyButton.GetCostCurrency());
            SetIsFree(buyButton.IsFree());
            SetOnConfirmedCallback(buyButton.GetOnConfirmedCallback());
        }

        public void Show()
        {
            gameObject.SetActive(true);
            onOpened?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            onClosed?.Invoke();
        }


        private void SetTitle(string value)
        {
            titleField.text = value;
        }

        private void SetIcon(Sprite icon)
        {
            iconImage.sprite = icon;
        }

        private void SetIconColor(Color color)
        {
            iconImage.color = color;
        }

        private void SetAmount(int value)
        {
            amountField.text = $"x{value}";
        }

        private void SetCost(int value)
        {
            costField.text = value.ToString();
        }

        private void SetCostCurrency(CurrencyType currency)
        {
            currencyImage.sprite = currencySprites[(int) currency];
        }

        private void SetIsFree(bool value)
        {
            currencyImage.gameObject.SetActive(!value);
            costField.gameObject.SetActive(!value);
            freeText.SetActive(value);
        }

        private void SetOnConfirmedCallback(Action action)
        {
            m_OnConfirmed = action;
        }
    }
}