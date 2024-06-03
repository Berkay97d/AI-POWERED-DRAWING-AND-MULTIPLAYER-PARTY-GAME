using System;
using System.Collections;
using _DatabaseAPI.Scripts;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using _MasterUser;
using _UserOperations.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace EnterAndExitUI_Competation.Scripts
{
    public class ExitTest_Script : MonoBehaviour
    {
        [SerializeField] private Card cardPrefab;
        [SerializeField] private CardPreview cardPreviewPrefab;
        [SerializeField] private PlayerInfoTableUI table;
        [SerializeField] private CardScrollview scrollview;
        [SerializeField] private Test_Randomizer_Script randomizer;
        [SerializeField] private RectTransform collectParent;
        [SerializeField] private Button claimButton;
        [SerializeField] private GameObject banner;
        [SerializeField] private GameObject collectBox;


        public event Action<MinigameResult> OnMinigameResult;
        public event Action OnPreviewPrizes;


        private static GameMode ms_GameMode;
        private static int[] ms_PlayerOrder;
        private readonly List<CardData> m_GainedCards = new();


        private void Awake()
        {
            collectBox.SetActive(true);
            
            claimButton.onClick.AddListener(OnClickClaimButton);
        }

        private void Start()
        {
            scrollview.SetActive(false);
            banner.SetActive(false);
            claimButton.gameObject.SetActive(false);
            
            table.SetPlayerCount(GetPlayerCount());
            table.Sort(ms_PlayerOrder);

            ApplyProfile(0, new Profile
            {
                name = LocalUser.GetUserDataFromCache().name
            });
            
            for (var i = 1; i < table.GetPlayerCount(); i++)
            {
                var profile = EnterTest_Script.GetBotProfile(i - 1);
                ApplyProfile(i, profile);
            }

            var prizePool = EnterTest_Script.allBettedCardDatas
                .OrderByDescending(data => data.card_data.score)
                .ToList();
            
            DealPrizePool(prizePool, () =>
            {
                collectBox.SetActive(false);
                scrollview.SetActive(true);
                banner.SetActive(true);
                claimButton.gameObject.SetActive(true);
                
                foreach (var gainedCard in m_GainedCards)
                {
                    var card = Instantiate(cardPrefab);
                    card.SetCardData(gainedCard);
                    scrollview.AddCard(card);
                }
                
                EnterTest_Script.allBettedCardDatas.Clear();
                
                OnPreviewPrizes?.Invoke();
            });
        }


        private void OnClickClaimButton()
        {
            SceneManager.LoadScene(0);
        }
        

        private void DealPrizePool(List<DatabaseAPI.CardPostResponseData> prizePool, Action onComplete = default)
        {
            StartCoroutine(Routine());
            
            IEnumerator Routine()
            {
                var playerIndex = -1;

                for (var i = 0; i < ms_PlayerOrder.Length; i++)
                {
                    if (ms_PlayerOrder[i] != 0) continue;
                    
                    playerIndex = i;
                    break;
                }
                
                OnMinigameResult?.Invoke(new MinigameResult
                {
                    placement = playerIndex + 1,
                    playerCount = GetPlayerCount()
                });

                var savedCards = 0;

                for (var i = 0; i < prizePool.Count; i++)
                {
                    yield return new WaitForSeconds(0.2f);

                    var cardData = prizePool[i].card_data;
                    var cardPreview = CreateCardPreview(cardData);
                    var index = (i - savedCards) % (table.GetPlayerCount() / 2);

                    var saveObtainedCard = playerIndex == 0 && prizePool[i].user_id == LocalUser.GetUserID();
                    var isGained = index == playerIndex || saveObtainedCard;
                    var userId = isGained ? LocalUser.GetUserID() : MasterUser.MasterUserId;

                    table.GetOrderedPlayerInfoUI(isGained ? playerIndex : index).AddGain(cardPreview);

                    if (saveObtainedCard)
                    {
                        savedCards += 1;
                    }
                    
                    if (isGained)
                    {
                        m_GainedCards.Add(cardData);
                    }
                    
                    DatabaseAPI.UpdateCard(prizePool[i].user_id, prizePool[i].card_id, userId,prizePool[i].card_data, null);
                    //TODO DATABESE UPDATED EVENT
                }
                
                yield return new WaitForSeconds(3f);
                
                table.Hide();

                yield return new WaitForSeconds(0.5f);
                
                onComplete?.Invoke();
            }
        }
        
        private void ApplyProfile(int index, Profile profile)
        {
            var info = table.GetPlayerInfoUI(index);
            info.SetName(profile.name);
            info.SetLevel(randomizer.GetRandomLevel());
            info.SetProfilePhoto(profile.photo);
            info.SetActiveBetObject(false);
            info.SetActiveGainObject(true);
            info.SetCardCount(0);
        }

        private CardPreview CreateCardPreview(CardData cardData)
        {
            var cardPreview = Instantiate(cardPreviewPrefab, collectParent);
            cardPreview.SetLocalPosition(Vector3.zero);
            cardPreview.SetCardData(cardData);
            return cardPreview;
        }

        private int GetPlayerCount()
        {
            return CompetationMenuController.GetPlayerCount(ms_GameMode);
        }


        public static void SetGameMode(GameMode gameMode)
        {
            ms_GameMode = gameMode;
        }

        public static void SetPlayerOrder(int[] playerOrder)
        {
            ms_PlayerOrder = playerOrder;
        }
    }
}