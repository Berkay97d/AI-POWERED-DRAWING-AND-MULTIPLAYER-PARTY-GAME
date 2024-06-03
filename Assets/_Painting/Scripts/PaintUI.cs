using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Painting
{
    public class PaintUI : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private RectTransform confirmPaintPanel;
        [SerializeField] private Image paintPreviewImage;
        [SerializeField] private Button confirmPaintButton;
        [SerializeField] private Button cancelPaintButton;
        [SerializeField] private float togglePanelDuration = 0.25f;
        
        
        private void Awake()
        {
            confirmPaintButton.onClick.AddListener(() =>
            {
                background.gameObject.SetActive(false);
                confirmPaintPanel.gameObject.SetActive(false);
                confirmPaintButton.interactable = false;
                PaintManager.CompletePaint();
                PaintManager.SetIsPaintable(false);
            });
            cancelPaintButton.onClick.AddListener(HideConfirmPaintPanel);
        }


        public void SetPaintPreview(Sprite sprite)
        {
            paintPreviewImage.sprite = sprite;
        }
        
        public void ShowConfirmPaintPanel()
        {
            confirmPaintButton.interactable = true;
            
            PaintManager.SetIsPaintable(false);
            
            background.gameObject.SetActive(true);
            confirmPaintPanel.gameObject.SetActive(true);
            
            background.DOColor(new Color(0f, 0f, 0f, 0.8f), togglePanelDuration)
                .SetEase(Ease.OutSine);

            confirmPaintPanel.DOScale(Vector3.one, togglePanelDuration)
                .SetEase(Ease.OutBack);
        }

        private void HideConfirmPaintPanel()
        {
            background.DOColor(new Color(0f, 0f, 0f, 0.0f), togglePanelDuration)
                .SetEase(Ease.InSine);

            confirmPaintPanel.DOScale(Vector3.zero, togglePanelDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    PaintManager.SetIsPaintable(true);
                    
                    background.gameObject.SetActive(false);
                    confirmPaintPanel.gameObject.SetActive(false);
                });
        }
    }
}