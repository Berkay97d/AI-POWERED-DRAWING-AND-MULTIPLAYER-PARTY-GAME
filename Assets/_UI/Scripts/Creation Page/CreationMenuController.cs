using System;
using System.Drawing;
using _DatabaseAPI.Scripts;
using _Painting;
using _PaintToCard.Scripts;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public enum CreationGameMode
    {
        PartPuzzle,
        MemoryMatch
    }
    
    public class CreationMenuController : MonoBehaviour
    {
        [SerializeField] private DailyCardGenerationController dailyCardGenerationController;
        [SerializeField] private GameObject[] alwaysCanvases;
        [SerializeField] private Transform cardTypeSelectionPage;
        [SerializeField] private Transform gameModesMainTransform;
        [SerializeField] private Button backButton;
        [SerializeField] private PromptManager promptManager;
        [SerializeField] private GameObject[] dailyGenerationReturnOpenCanvases;
        
        
        private void Awake()
        {
            PaintToCardAnimator.OnGeneratedCardClaimed += OnGeneratedCardClaimed;
        }

        private void Start()
        {
            GameModeController.Instance.OnModeSelected += OnModeSelected; //TODO CREATION'I SECİNCE
            
            backButton.onClick.AddListener(()=>
            {
                if (dailyCardGenerationController.GetIsDailyGenerating())
                {
                    GoBackCustomization();
                    return;
                }
                GoBackModeSelection();
            });
        }

        private void OnDestroy()
        {
            PaintToCardAnimator.OnGeneratedCardClaimed -= OnGeneratedCardClaimed;
            GameModeController.Instance.OnModeSelected -= OnModeSelected;
        }

        private void GoBackCustomization()
        {
            cardTypeSelectionPage.localScale = Vector3.zero;

            foreach (var canvase in dailyGenerationReturnOpenCanvases)
            {
                canvase.SetActive(true);
            }
            
            dailyCardGenerationController.SetIsDailyGenerating(false);
            
            cardTypeSelectionPage.gameObject.SetActive(false);
            
        }
        
        private void GoBackModeSelection()
        {
            cardTypeSelectionPage.localScale = Vector3.zero;
            gameModesMainTransform.localScale = Vector3.zero;
            gameModesMainTransform.gameObject.SetActive(true);
                
            gameModesMainTransform.DOScale(Vector3.one, 0.35f).OnComplete(() =>
            {
                cardTypeSelectionPage.gameObject.SetActive(false);
                alwaysCanvases[1].SetActive(true);
            });
        }
        
        private void GetPaintingPage()
        {
            foreach (var canvas in alwaysCanvases)
            {
                canvas.SetActive(false);
            }
            
            cardTypeSelectionPage.localScale = Vector3.zero;
            cardTypeSelectionPage.gameObject.SetActive(true);
            cardTypeSelectionPage.DOScale(Vector3.one, 0.35f);
        }


        // TODO matchmaking olacak
        private void OnGeneratedCardClaimed()
        {
            
            var rarity = dailyCardGenerationController.GetCurrentGenerationRarity();
            var score = CardData.GetRandomScoreByRarity(rarity);
            Debug.Log(promptManager.GetSubtitle());
            var cdata = new CardData
            {
                score = score,
                subTitle = promptManager.GetSubtitle(),
                imageID = PaintToCardConverter.GetGeneratedImageID(),
                inCollection = 0
            };

            LocalUser.AddCard(cdata);

            SceneManager.LoadScene(0);
        }
        
        
        private void OnModeSelected(object sender, bool e)
        {
            if (!e)
            {
                GetPaintingPage();
            }
        }

        

        
        
        
    }
}