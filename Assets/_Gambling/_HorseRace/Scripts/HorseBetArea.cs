using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HorseRace
{
    public class HorseBetArea : MonoBehaviour
    {
        [SerializeField] private Horse horse;

        [SerializeField] private Image selectOrNotImage;
        [SerializeField] private Sprite selectedUI;
        [SerializeField] private Sprite unselectedUI;

        [SerializeField] private TMP_Text horseNameText;
        [SerializeField] private TMP_Text horseNumberText;
        [SerializeField] private TMP_Text horseMultiplierText;
        [SerializeField] private TMP_Text horsePlacedBetCountText;
        [SerializeField] private TMP_Text horsePayoutCountText;

        [SerializeField] private Button button;

        public event EventHandler<HorseBetArea> OnBetAreaButtonClicked;
        
        private int placedBetCount;

        private void Awake()
        {
            button.onClick.AddListener(() => { OnBetAreaButtonClicked?.Invoke(this, this); });
        }

        private void Start()
        {
            BetController.Instance.OnBetChanged += OnBetChanged;
            
            SetHorseInfoUI();
            UpdateBetTexts();
        }

        private void OnDestroy()
        {
            BetController.Instance.OnBetChanged -= OnBetChanged;
        }

        private void OnBetChanged(object sender, EventArgs e)
        {
            UpdateBetTexts();
        }

        private void SetHorseInfoUI()
        {
            horseNameText.text = horse.GetHorseName();
            horseNumberText.text = horse.GetHorseNumber().ToString();
            horseMultiplierText.text = "x" + horse.GetMultiplier();
        }

        private void UpdateBetTexts()
        {
            horsePlacedBetCountText.text = placedBetCount.ToString();
            horsePayoutCountText.text = (placedBetCount * horse.GetMultiplier()).ToString();
        }
        
        public void ActivateSelectedUi()
        {
            selectOrNotImage.sprite = selectedUI;
        }
        
        public void ActivateUnselectedUi()
        {
            selectOrNotImage.sprite = unselectedUI;
        }
        
        public int GetPlacedBetCount()
        {
            return placedBetCount;
        }

        public void SetPlacedBetCount(int count)
        {
            placedBetCount = count;
        }
        
        
        
    }
}