using System;
using System.Collections.Generic;
using DG.Tweening;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SubtitleSelection.Scripts
{
    public class MainTitle : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;

        public event EventHandler<MainTitle> OnMainTitleButtonClick;
        
        private string mainTitle;
        private Button button;


        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(RaiseOnMainTitleButtonClick);
        }

        private void RaiseOnMainTitleButtonClick()
        {
            OnMainTitleButtonClick?.Invoke(this,this);
        }

        public void SetButtonDisable()
        {
            button.enabled = false;
        }
        
        public void SetText(string t)
        {
            title.text = t;
            mainTitle = t;
        }

        public string GetMainTitle()
        {
            return mainTitle;
        }
        public void OpenMainTitle(Vector3 pos, float speed)
        {
            transform.DOMove(pos, speed).SetSpeedBased();
        }
        
        public void CloseMainTitle(Vector3 pos, float speed, Action onComplete)
        {
            transform.DOMove(pos, speed).SetSpeedBased().OnComplete(onComplete.Invoke);
        }
        
    }
}