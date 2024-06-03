using System;
using System.Linq;
using _CardPacks.Scripts;
using _DatabaseAPI.Scripts;
using _Market.Scripts;
using _MasterUser;
using _PlayerCustomization.Scripts;
using _QuestSystem.Scripts;
using _TimeAPI.Scripts;
using General;
using UI;
using UnityEngine;
using CardData = _DatabaseAPI.Scripts.CardData;

namespace _UserOperations.Scripts
{
    public static class LocalUser
    {
        private const string UserIdKey = "User_ID";


        public static event Action<UserData> OnUserDataChanged;
        public static event Action<UserData> OnUserDataLoaded;
        public static event Action<DatabaseAPI.CardPostResponseData[]> OnCardDatasLoaded;
        public static event Action<DatabaseAPI.CardPostResponseData[]> OnCardDatasChanged;


        private static UserData ms_UserData;


        public static UserData GetUserDataFromCache()
        {
            return ms_UserData;
        }
        
        public static void ClaimQuestPrize(int questIndex, Action onSucceed = default, Action onFailed = default)
        {
            var questDataContainer = QuestDataContainerSO.GetInstance();
            var questDatas = questDataContainer.GetQuests();
            
            GetUserData(userData =>
            {
                var quest = userData.quests[questIndex];
                var questData = questDatas[questIndex];

                if (quest.isClaimed)
                {
                    onFailed?.Invoke();
                    return;
                }
                
                quest.isClaimed = true;
                userData.quests[questIndex] = quest;

                if (questData.rewardCurrency == CurrencyType.Coin)
                {
                    userData.coin += questData.rewardCount;
                }

                if (questData.rewardCurrency == CurrencyType.Gem)
                {
                    userData.gem += questData.rewardCount;
                }
                
                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }
        
        public static void AddQuestProgress(QuestType questType, int count = 1, Action onSucceed = default, Action onFailed = default)
        {
            var questDataContainer = QuestDataContainerSO.GetInstance();
            var questDatas = questDataContainer.GetQuests();
            var indices = questDataContainer.GetQuestIndicesWithType(questType);
            
            GetUserData(userData =>
            {
                var quests = userData.quests;
                
                foreach (var index in indices)
                {
                    var quest = quests[index];
                    var maxCount = questDatas[index].actionCount;

                    quest.count = Mathf.Min(quest.count + count, maxCount);
                    quests[index] = quest;
                }

                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }
        
        public static void UnlockSkin(SkinData skinData, Action onSucceed = default, Action onFailed = default)
        {
            GetUserData(userData =>
            {
                if (userData.TryUnlockSkin(skinData))
                {
                    SetUserData(userData, onSucceed, onFailed);
                    return;
                }
                
                onFailed?.Invoke();

            }, onFailed);
        }
        
        public static void ResetDailyGeneration(CardRarity rarity, Action onSucceed = default,
            Action onFailed = default)
        {
            GetUserData(userData =>
            {
                TimeAPI.GetCurrentTime(time =>
                {
                    var index = (int) rarity;
                    var dailyGeneration = userData.dailyGenerations[index];

                    dailyGeneration.startTime = time;
                    userData.dailyGenerations[index] = dailyGeneration;
                    SetUserData(userData, onSucceed, onFailed);
                }, onFailed);
            }, onFailed);
        }
        
        public static void TryUnlockCardPack(int slotIndex, Action onSucceed = default, Action onFailed = default)
        {
            if (slotIndex < 0) return;
            
            GetUserData(userData =>
            {
                if (slotIndex >= userData.cardPackSlots.Length)
                {
                    onFailed?.Invoke();
                    return;
                }

                if (userData.cardPackSlots.Any(slot => slot.IsStartedUnlocking()))
                {
                    onFailed?.Invoke();
                    return;
                }

                TimeAPI.GetCurrentTime(time =>
                {
                    var slot = userData.cardPackSlots[slotIndex];
                    slot.startTime = time;
                    userData.cardPackSlots[slotIndex] = slot;
                    
                    SetUserData(userData, onSucceed, onFailed);
                }, onFailed);
            }, onFailed);
        }
        
        public static void TryAddCardPack(CardPackType cardPackType, Action onSucceed = default, Action<FailReason> onFailed = default)
        {
            if (cardPackType < 0) return;
            
            GetUserData(userData =>
            {
                if (userData.TryAddCardPack(cardPackType))
                {
                    onSucceed?.Invoke();
                    return;
                }
                
                onFailed?.Invoke(FailReason.Full);
                
                SetUserData(userData, onSucceed, () => onFailed?.Invoke(FailReason.NetworkError));
            }, () => onFailed?.Invoke(FailReason.NetworkError));
        }

        public static void TryRemoveCardPackFromSlot(int slotIndex, Action onSucceed = default, Action onFailed = default)
        {
            GetUserData(userData =>
            {
                if (userData.cardPackSlots[slotIndex].IsEmpty())
                {
                    onFailed?.Invoke();
                    return;
                }
                
                userData.cardPackSlots[slotIndex] = CardPackSlotData.Default;
                
                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }
        
        public static void AddUpgradeToken(int value, CardRarity rarity, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;

            GetUserData(userData =>
            {
                userData.AddUpgradeToken(value, rarity);
                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }

        public static void TryRemoveUpgradeToken(int value, CardRarity rarity, Action onSucceed = default,
            Action<FailReason> onFailed = default)
        {
            GetUserData(userData =>
            {
                if (userData.TryRemoveUpgradeToken(value, rarity))
                {
                    SetUserData(userData);
                    onSucceed?.Invoke();
                    return;
                }

                onFailed?.Invoke(FailReason.NotEnough);
            }, () => onFailed?.Invoke(FailReason.NetworkError));
        }

        public static void AddCard(CardData cardData, Action<DatabaseAPI.CardPostResponseData> onSucceed = default, Action onFailed = default)
        {
            var userID = GetUserID();
            DatabaseAPI.PostCard(userID, cardData, response =>
            {
                RaiseCardDatasChanged();
                onSucceed?.Invoke(response);
            }, onFailed);
        }

        public static void RemoveCard(string cardID, CardData cardData, Action onSucceed = default, Action onFailed = default)
        {
            var userID = GetUserID();
            DatabaseAPI.UpdateCard(userID, cardID, MasterUser.MasterUserId, cardData, () =>
            {
                RaiseCardDatasChanged();
                onSucceed?.Invoke();
            }, onFailed);
        }

        public static void GetCards(Action<DatabaseAPI.CardPostResponseData[]> onSucceed, Action<FailReason> onFailed = default)
        {
            var userID = GetUserID();
            DatabaseAPI.GetCardsByUserID(userID, onSucceed, onFailed);
        }

        public static void UpdateCard(string cardID, CardData cardData, Action onSucceed = default, Action onFailed = default)
        {
            var userID = GetUserID();
            DatabaseAPI.UpdateCard(userID, cardID, userID, cardData, () =>
            {
                RaiseCardDatasChanged();
                onSucceed?.Invoke();
            }, onFailed);
        }
        
        public static void LoadUserData()
        {
            GetUserData(userData =>
            {
                ms_UserData = userData;
                OnUserDataLoaded?.Invoke(userData);
            });
        }

        public static void LoadCardDatas()
        {
            GetCards(OnCardDatasLoaded, failReason =>
            {
                if (failReason == FailReason.NotFound)
                {
                    OnCardDatasChanged?.Invoke(Array.Empty<DatabaseAPI.CardPostResponseData>());
                }
            });
        }

        public static void AddGem(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                userData.gem += value;
                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }

        public static void TryRemoveGem(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                var isEnough = userData.gem >= value;

                if (isEnough)
                {
                    userData.gem -= value;
                    SetUserData(userData, onSucceed, onFailed);
                    return;
                }
                
                onFailed?.Invoke();
            }, onFailed);
        }
        
        public static void AddCoin(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                userData.coin += value;
                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }

        public static void TryRemoveCoin(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                var isEnough = userData.coin >= value;

                if (isEnough)
                {
                    userData.coin -= value;
                    SetUserData(userData, onSucceed, onFailed);
                    return;
                }
                
                onFailed?.Invoke();
            }, onFailed);
        }
        
        public static void AddEnergy(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                userData.energy += value;
                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }

        public static void TryRemoveEnergy(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                var isEnough = userData.energy >= value;

                if (isEnough)
                {
                    userData.energy -= value;
                    SetUserData(userData, onSucceed, onFailed);
                    return;
                }
                
                onFailed?.Invoke();
            }, onFailed);
        }
        
        public static void AddExperience(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                userData.experience += value;
                SetUserData(userData, onSucceed, onFailed);
            }, onFailed);
        }

        public static void TryRemoveExperience(int value, Action onSucceed = default, Action onFailed = default)
        {
            if (value <= 0) return;
            
            GetUserData(userData =>
            {
                var isEnough = userData.experience >= value;

                if (isEnough)
                {
                    userData.experience -= value;
                    SetUserData(userData, onSucceed, onFailed);
                    return;
                }
                
                onFailed?.Invoke();
            }, onFailed);
        }
        
        public static string GetUserID()
        {
            return PlayerPrefs.GetString(UserIdKey, UserAPI.NullUserID);
        }
        
        public static void SetUserID(string userID)
        {
            PlayerPrefs.SetString(UserIdKey, userID);
        }
        
        public static void GetUserData(Action<UserData> onCompleted, Action onFailed = default)
        {
            var userID = GetUserID();
            UserAPI.GetUserData(userID, onCompleted, onFailed);
        }
        
        public static void SetUserData(UserData userData, Action onSucceed = default, Action onFailed = default)
        {
            var userID = GetUserID();
            UserAPI.SetUserData(userID, userData, () =>
            {
                onSucceed?.Invoke();
                SetUserDataToCache(userData);
            }, onFailed);
        }


        private static void SetUserDataToCache(UserData userData)
        {
            ms_UserData = userData;
            OnUserDataChanged?.Invoke(userData);
        }
        
        private static void RaiseCardDatasChanged()
        {
            GetCards(cardDatas =>
            {
                OnCardDatasChanged?.Invoke(cardDatas);
            }, failReason =>
            {
                if (failReason == FailReason.NotFound)
                {
                    OnCardDatasChanged?.Invoke(Array.Empty<DatabaseAPI.CardPostResponseData>());
                    return;
                }

                LazyCoroutines.WaitForSeconds(0.1f, RaiseCardDatasChanged);
            });
        }
    }
}