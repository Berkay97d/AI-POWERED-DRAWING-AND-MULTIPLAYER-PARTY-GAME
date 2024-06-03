using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SubtitleSelection.Scripts
{
    public class StartPaintingButton : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform cardTypeSelectionMainTransform;
        [SerializeField] private Transform paintingPage;
        [SerializeField] private ArtstyleSelectionButton artstyleSelectionButton;
        [SerializeField] private TitleSelectionButton titleSelectionButton;
        [SerializeField] private TMP_Text paintingPageText;
        
        
        public event EventHandler<string> OnPaintingStart;

        private Button button;
        private bool flag = false;


        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Update()
        {
            if (artstyleSelectionButton.GetIsSelected() && titleSelectionButton.GetIsSelected() && !flag)
            {
                flag = true;
                button.interactable = true;
            }
        }

        private void Start()
        {
            button.onClick.AddListener(GetPaintingPage);
            button.interactable = false;
        }

        private void GetPaintingPage()
        {
            var prompt = "((" + titleSelectionButton.GetPrompt() +"))" + ", " + artstyleSelectionButton.GetPrompt();  
            OnPaintingStart?.Invoke(this, prompt);
            paintingPageText.text = "DRAW SOMETHING ABOUT " + titleSelectionButton.GetPrompt().ToUpper();

            mainCamera.orthographic = true;
            
            cardTypeSelectionMainTransform.gameObject.SetActive(false);
            
            paintingPage.transform.localScale = Vector3.zero;
            paintingPage.gameObject.SetActive(true);
            paintingPage.DOScale(Vector3.one, 0.35f);
        }

        public string GetSubTitle()
        {
            return titleSelectionButton.GetPrompt();
        }
    }
}