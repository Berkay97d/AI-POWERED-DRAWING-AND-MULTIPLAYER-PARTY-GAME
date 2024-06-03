using System;
using System.Collections.Generic;
using System.Linq;
using _DatabaseAPI.Scripts;
using _MasterUser;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using General;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace EnterAndExitUI_Competation.Scripts
{
    public class EnterTest_Script : MonoBehaviour
    {
        
        [SerializeField] private Card cardPrefab;
        [SerializeField] private PlayerInfoTableUI table;
        [SerializeField] private CardScrollview scrollview;
        [SerializeField] private Test_Randomizer_Script randomizer;
        [SerializeField] private RectTransform collectParent;
        [SerializeField] private RectTransform collectBox;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private GameObject banner;


        private GameMode gameMode;
        private DatabaseAPI.CardPostResponseData[] masterUserCards;
        private DatabaseAPI.CardPostResponseData[] playerBets;
        private List<DatabaseAPI.CardPostResponseData[]> botsBets = new();
        private Coroutine countdownRoutine;

        public static readonly List<DatabaseAPI.CardPostResponseData> allBettedCardDatas = new();


        private static Profile[] botProfiles;
        

        private void LoadGameScene(GameMode mode)
        {
            if (mode == GameMode.Stack)
            {
                LocalUser.AddQuestProgress(QuestType.Play_Competition);
                LocalUser.AddQuestProgress(QuestType.Play_Stack);
                SceneManager.LoadScene(1);
                return;
            }
            
            if (mode == GameMode.FastEyes)
            {
                LocalUser.AddQuestProgress(QuestType.Play_Competition);
                LocalUser.AddQuestProgress(QuestType.Play_FastEye);
                SceneManager.LoadScene(2);
                return;
            }
            
            if (mode == GameMode.Snow)
            {
                LocalUser.AddQuestProgress(QuestType.Play_Competition);
                LocalUser.AddQuestProgress(QuestType.Play_SnowButt);
                SceneManager.LoadScene(4);
                return;
            }

            if (mode == GameMode.DontFall)
            {
                LocalUser.AddQuestProgress(QuestType.Play_Competition);
                LocalUser.AddQuestProgress(QuestType.Play_DontFall);
                SceneManager.LoadScene(3);
                return;
            }

            if (mode == GameMode.Katana)
            {
                LocalUser.AddQuestProgress(QuestType.Play_Competition);
                LocalUser.AddQuestProgress(QuestType.Play_Katana);
                SceneManager.LoadScene(5);
                return;
            }

            if (mode == GameMode.Laser)
            {
                LocalUser.AddQuestProgress(QuestType.Play_Competition);
                //LocalUser.AddQuestProgress(QuestType.Play_Katana);  //TODO Play Laser Quest
                SceneManager.LoadScene(7);
                return;
            }
        }


        public void InitMatchMaking(int playerCount, string[] playerBetIds)
        {
            scrollview.SetActive(false);
            banner.SetActive(false);
            
            table.SetPlayerCount(playerCount);
            botProfiles = new Profile[playerCount - 1];
            
            LocalUser.GetCards(datas =>
            {
                playerBets = datas
                    .Where(data => playerBetIds.Contains(data.card_id))
                    .ToArray();

                var averagePlayerScore = Mathf.RoundToInt((float) playerBets.Average(data => data.card_data.score));
                
                MasterUser.GetCards(cards =>
                {
                    var cardsList = cards.ToList();
                
                    for (int i = 1; i < playerCount; i++)
                    {
                        masterUserCards = cards;
                        var bet = randomizer.GetRandomBet(cardsList);

                        foreach (var card in bet)
                        {
                            cardsList.Remove(card);
                        }

                        for (var j = 0; j < bet.Length; j++)
                        {
                            var cardData = bet[j].card_data;
                            cardData.score = Mathf.Clamp(averagePlayerScore + Random.Range(-15, 15), 10, 100);
                            bet[j].card_data = cardData;
                        }
                    
                        botsBets.Add(bet);
                    }
                
                    Test(playerCount);
                });
            });
        }

        private void Test(int playerCount)
        {
            table.GetPlayerInfoUI(0).SetName(LocalUser.GetUserDataFromCache().name);
            table.GetPlayerInfoUI(0).SetBet(playerBets.Select(data => data.card_data).ToArray());
            
            // LOOKING FOR RANDOM PLAYER, FAKE
            var routine = LazyCoroutines.EverySeconds(() => 0.1f, () =>
            {
                for (var i = 1; i < table.GetPlayerCount(); i++)
                {
                    RandomizePlayer(i);
                }
            });
            
            
            LazyCoroutines.WaitForSeconds(5, () =>
            {
                LazyCoroutines.StopCoroutine(routine);
            });

            
            LazyCoroutines.WaitForSeconds(5.25f, () =>
            {


                for (int i = 1; i < table.GetPlayerCount(); i++)
                {
                    table.GetPlayerInfoUI(i).SetBet(botsBets[i - 1].Select(data => data.card_data).ToArray()); //*
                }

                for (var i = 0; i < table.GetPlayerCount(); i++)
                {
                    var infoUI = table.GetPlayerInfoUI(i);

                    for (var j = 0; j < infoUI.GetCardPreviewCount(); j++)
                    {
                        var cardPreview = infoUI.GetCardPreview(j);
                        cardPreview.SetParent(collectParent);
                    }
                }

                table.CollectPlayerCardPreviews(collectBox.position, 1);
            });

            LazyCoroutines.WaitForSeconds(6f, () =>
            {
                table.Hide();
            });

            LazyCoroutines.WaitForSeconds(7f, () =>
            {
                scrollview.SetActive(true);
                banner.SetActive(true);
                
                for (int i = 0; i < table.GetPlayerCount(); i++)
                {
                    var playerInfo = table.GetPlayerInfoUI(i);
                    
                    for (int j = 0; j < playerInfo.GetBet().Length; j++)
                    {
                        var card = Instantiate(cardPrefab);
                        if (i == 0)
                        {
                            card.SetCardData(playerBets[j].card_data);  //*
                        }
                        else
                        {
                            card.SetCardData(botsBets[i-1][j].card_data);
                        }
                        scrollview.AddCard(card);
                    }    
                }

                var countdown = 5;

                countdownText.gameObject.SetActive(true);
                countdownText.text = $"Game start in {countdown} secs";

                countdownRoutine = LazyCoroutines.EverySeconds(() => 1f, () =>
                {
                    if (countdown <= 0)
                    {
                        LoadGameScene(gameMode);
                        StopCoroutine(countdownRoutine);
                        return;
                    }
                    
                    countdown -= 1;
                    countdownText.text = $"Game start in {countdown} secs";
                });
            });

            foreach (var playerBet in playerBets)
            {
                allBettedCardDatas.Add(playerBet);
            }

            foreach (var botBets in botsBets)
            {
                foreach (var botBet in botBets)
                {
                    allBettedCardDatas.Add(botBet);
                }
            }
            
        }


        private void RandomizePlayer(int index)
        {
            var info = table.GetPlayerInfoUI(index);
            var randomName = randomizer.GetRandomName();
            var randomPhoto = randomizer.GetRandomProfilePhoto();
            
            info.SetName(randomName);
            info.SetLevel(randomizer.GetRandomLevel());
            info.SetProfilePhoto(randomPhoto);
            info.SetActiveBetObject(true);
            info.SetActiveGainObject(false);
            info.SetBet(randomizer.GetRandomBet(masterUserCards).Select(card => card.card_data).ToArray());
            
            botProfiles[index - 1] = new Profile
            {
                name = randomName,
                photo = randomPhoto
            };
        }

        public void SetGameMode(GameMode m)
        {
            gameMode = m;
        }


        public static Profile GetBotProfile(int index)
        {
            return botProfiles == null ? new Profile() : botProfiles[index];
        }
    }

    public struct Profile
    {
        public string name;
        public Sprite photo;
    }
}