using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;

namespace _SubtitleSelection.Scripts
{
    public class TitleSelectionButton : CardTypeSelectionButton
    {
        
        [SerializeField] private Transform mainTitleParent;
        [SerializeField] private MainTitle mainTitlePrefab;



        private readonly List<MainTitle> initedMainTitles = new();
        private string prompt;

        [SerializeField] private ArtstyleSelectionButton artstyleSelectionButton;
        [SerializeField] private TMP_Text selectedMainTitleText;
        
        
        
        private void Start()
        {
            artstyleSelectionButton.OnCardTypeSelectionButtonClick += OnNoneMeButtonClick;
        }

        private void OnDestroy()
        {
            artstyleSelectionButton.OnCardTypeSelectionButtonClick -= OnNoneMeButtonClick;
        }

        private void OnNoneMeButtonClick(object sender, OnCardTypeSelectionButtonClickEventArgs e)
        {
            Debug.Log(this.name);
            
            BeUnInteractable();
            
            if (e.isOpened)
            {
                GoLeft();
                return;
            }
            
            GoRight();
        }
        
        
        protected override void GetOpen()
        {
            for (int i = 0; i < Book.SUBTITLES.Length; i++)
            {
                if (i % 2 == 0)
                {
                    var title = Instantiate(mainTitlePrefab, mainTitleParent);
                    
                    title.OnMainTitleButtonClick += TitleOnOnMainTitleButtonClick;
                    
                    title.transform.localPosition = Vector3.zero + new Vector3(-90,0,0);
                    
                    initedMainTitles.Add(title);
                }
                else
                {
                    var title = Instantiate(mainTitlePrefab, mainTitleParent);
                    
                    title.OnMainTitleButtonClick += TitleOnOnMainTitleButtonClick;
                    
                    title.transform.localPosition = Vector3.zero + new Vector3(300,0,0);
                    
                    initedMainTitles.Add(title);
                }
            }
            
            for (var i = 0; i < initedMainTitles.Count; i++)
            {
                var artStyle = initedMainTitles[i];
                artStyle.SetText(Book.SUBTITLES[i]);
            }

            for (int i = 0; i < Book.SUBTITLES.Length; i++)
            {
                var pos = transform.position;
                int a = i / 2;
                pos.y -= ((a + 1) * 0.75f) + 0.25f  ;

                var movePos = new Vector3(initedMainTitles[i].transform.position.x, pos.y, pos.z);
                
                initedMainTitles[i].OpenMainTitle(movePos, 10f);
            }
        }

        private void TitleOnOnMainTitleButtonClick(object sender, MainTitle e)
        {
            foreach (var mainTitle in initedMainTitles)
            {
                mainTitle.SetButtonDisable();
            }
            
            prompt = e.GetMainTitle();
            OnSelected?.Invoke(prompt);
            SetIsSelected(true);
            selectedMainTitleText.text = "Drawing Subject: " + e.GetMainTitle();
            TakeOnClickAction();
        }

        protected override void GetClose()
        {
            foreach (var mainTitle in initedMainTitles)
            {
                var movePos = new Vector3(mainTitle.transform.position.x, transform.position.y, transform.position.z);
                
                mainTitle.CloseMainTitle(movePos, 10f, () =>
                {
                    mainTitle.OnMainTitleButtonClick -= TitleOnOnMainTitleButtonClick;
                    Destroy(mainTitle.gameObject);
                });
            }
            
            initedMainTitles.Clear();
        }
        
        public string GetPrompt()
        {
            return prompt;
        }

        
    }
}