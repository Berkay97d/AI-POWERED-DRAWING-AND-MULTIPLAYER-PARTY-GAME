using System;
using _CardPacks.Scripts;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GenerationArea : MonoBehaviour
    {
        [SerializeField] private CardRarity cardRarity;
        [SerializeField] private Image imagePlace;
        [SerializeField] private Sprite unreadySprite;
        [SerializeField] private Sprite readySprite;
        [SerializeField] private TMP_Text waitHoursText;
        [SerializeField] private Button generateButton;

        public event EventHandler<CardRarity> OnGenerateButtonClick;

        private bool isReadyToGenerate;


        private void Start()
        {
            generateButton.onClick.AddListener(RaiseOnGenerateButtonClick);
        }

        public void SetTimeLeft(TimeSpan timeLeft)
        {
            waitHoursText.text = timeLeft.ToCountdownString();
            
            if (timeLeft.TotalMilliseconds <= 0d && !isReadyToGenerate)
            {
                isReadyToGenerate = true;
                ActivateReadyToGenerate();
                return;
            }

            if (timeLeft.TotalMilliseconds > 0d && isReadyToGenerate)
            {
                ActivateNotReadyToGenerate();
                isReadyToGenerate = false;
            }
        }

        private void ActivateReadyToGenerate()
        {
            imagePlace.sprite = readySprite;
            generateButton.gameObject.SetActive(true);
            
            waitHoursText.gameObject.SetActive(false);
            
        }
        
        private void ActivateNotReadyToGenerate()
        {
            imagePlace.sprite = unreadySprite;
            generateButton.gameObject.SetActive(false);
            
            waitHoursText.gameObject.SetActive(true);
        }

        private void RaiseOnGenerateButtonClick()
        {
            OnGenerateButtonClick?.Invoke(this, cardRarity);
        }

        public bool GetIsReadyToGenerate()
        {
            return isReadyToGenerate;
        }
        
    }
}