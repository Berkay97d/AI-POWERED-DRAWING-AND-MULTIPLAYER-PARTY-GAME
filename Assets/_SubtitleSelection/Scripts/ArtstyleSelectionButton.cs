using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _SubtitleSelection.Scripts
{
    public class ArtstyleSelectionButton : CardTypeSelectionButton
    {
        [SerializeField] private ArtStyle artStylePrefab;
        [SerializeField] private Transform artStyleParent;
        [SerializeField] private string[] artStyles;

        [SerializeField] private TitleSelectionButton titleSelectionButton;

        [SerializeField] private TMP_Text selectedTitleText;

        private string prompt;
        private readonly List<ArtStyle> initedArtStyles = new();
        

        private void Start()
        {
            titleSelectionButton.OnCardTypeSelectionButtonClick += OnNoneMeButtonClick;
        }

        private void OnDestroy()
        {
            titleSelectionButton.OnCardTypeSelectionButtonClick -= OnNoneMeButtonClick;
        }

        private void OnNoneMeButtonClick(object sender, OnCardTypeSelectionButtonClickEventArgs e)
        {
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
            for (int i = 0; i < artStyles.Length; i++)
            {
                if (i % 2 == 0)
                {
                    var title = Instantiate(artStylePrefab, artStyleParent);
                    
                    title.OnArtStyleButtonClick += TitleOnOnArtStyleButtonClick;
                    
                    title.transform.localPosition = Vector3.zero + new Vector3(-90,0,0);
                    
                    initedArtStyles.Add(title);
                }
                else
                {
                    var title = Instantiate(artStylePrefab, artStyleParent);
                    
                    title.OnArtStyleButtonClick += TitleOnOnArtStyleButtonClick;
                    
                    title.transform.localPosition = Vector3.zero + new Vector3(300,0,0);
                    
                    initedArtStyles.Add(title);
                }
            }

            for (var i = 0; i < initedArtStyles.Count; i++)
            {
                var artStyle = initedArtStyles[i];
                artStyle.SetText(artStyles[i]);
            }

            for (int i = 0; i < artStyles.Length; i++)
            {
                var pos = transform.position;
                int a = i / 2;
                pos.y -= ((a + 1) * 0.75f) + 0.25f  ;

                var movePos = new Vector3(initedArtStyles[i].transform.position.x, pos.y, pos.z);

                initedArtStyles[i].transform.DOMove(movePos,10f).SetSpeedBased();
            }
        }

        private void TitleOnOnArtStyleButtonClick(object sender, ArtStyle e)
        {
            foreach (var initedArtStyle in initedArtStyles)
            {
                initedArtStyle.SetButtonDisable();
            }
            
            SetIsSelected(true);
            prompt = e.GetArtStyle();
            OnSelected?.Invoke(prompt);
            selectedTitleText.text ="Art Style: " + e.GetArtStyle();
            TakeOnClickAction();
        }

        protected override void GetClose()
        {
            foreach (var mainTitle in initedArtStyles)
            {
                var movePos = new Vector3(mainTitle.transform.position.x, transform.position.y, transform.position.z);

                mainTitle.transform.DOMove(movePos, 10f).SetSpeedBased().OnComplete((() =>
                {
                    mainTitle.OnArtStyleButtonClick -= TitleOnOnArtStyleButtonClick;
                    Destroy(mainTitle.gameObject);
                }));
            }
            
            initedArtStyles.Clear();
        }

        public string GetPrompt()
        {
            return prompt;
        }


    }
}