using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SubtitleSelection.Scripts
{
    public class ArtStyle : MonoBehaviour
    {
        [SerializeField] private TMP_Text type;

        public event EventHandler<ArtStyle> OnArtStyleButtonClick;
        
        private string artStyle;
        private Button button;


        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(RaiseOnArtStyleButtonClick);
        }

        private void RaiseOnArtStyleButtonClick()
        {
            OnArtStyleButtonClick?.Invoke(this, this);
        }
        

        public void SetText(string t)
        {
            type.text = t;
            artStyle = t;
        }

        public string GetArtStyle()
        {
            return artStyle;
        }

        public void SetButtonDisable()
        {
            button.enabled = false;
        }
        
    }
}