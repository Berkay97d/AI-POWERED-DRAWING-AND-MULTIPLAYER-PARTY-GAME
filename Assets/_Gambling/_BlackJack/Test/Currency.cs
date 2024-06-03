using TMPro;
using UnityEngine;

namespace BlackJack
{
    public class Currency : MonoBehaviour
    {
        [SerializeField] private int currency;
        [SerializeField] private TMP_Text currencyText;

        public static Currency Instance { get; private set; }


        private void Awake()
        {
            Instance = this;
            UpdateCurrencyText();
        }

        private void Start()
        {
            //BetController.Instance.OnBetWin += OnBetWin;
        }

        private void OnBetWin(object sender, int e)
        {
            IncreaseCurrency(e);
        }

        private void Update()
        {
            UpdateCurrencyText();
        }

        public int GetCurrenct()
        {
            return currency;
        }

        public void DecreaseCurrency(int amount)
        {
            currency -= amount;
        }

        private void IncreaseCurrency(int amount)
        {
            currency += amount;
        }

        private void UpdateCurrencyText()
        {
            currencyText.text = "Currency: " + currency;
        }
    }
}