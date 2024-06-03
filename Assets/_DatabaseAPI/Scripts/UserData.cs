using System;
using System.Linq;
using _CardPacks.Scripts;
using _Market.Scripts;
using _PlayerCustomization.Scripts;
using _QuestSystem.Scripts;
using _TimeAPI.Scripts;
using UI;
using UnityEngine;

namespace _DatabaseAPI.Scripts
{
    [Serializable]
    public struct UserData
    {
        public string name;
        public int gem;
        public int coin;
        public int energy;
        public int experience;
        public SkinData[] skins;
        public int[] upgradeTokens;
        public CardPackSlotData[] cardPackSlots;
        public DailyGenerationData[] dailyGenerations;
        public DailyOfferData[] dailyOffers;
        public TimeData lastDailyOfferTime;
        public QuestDatabaseData[] quests;


        public static UserData Default = new()
        {
            name = "Player",
            gem = 250,
            coin = 3000,
            energy = 0,
            experience = 0,
            skins = PlayerCustomization.DefaultUnlockedSkins,
            upgradeTokens = new [] {0, 1, 0, 0},
            cardPackSlots = new []
            {
                CardPackSlotData.Default,
                CardPackSlotData.Default,
                CardPackSlotData.Default,
                CardPackSlotData.Default
            },
            dailyGenerations = new []
            {
                DailyGenerationData.Default,
                DailyGenerationData.Default,
                DailyGenerationData.Default,
                DailyGenerationData.Default
            },
            dailyOffers = Array.Empty<DailyOfferData>(),
            lastDailyOfferTime = TimeData.Zero,
            quests = new QuestDatabaseData[7]
        };


        public int GetFirstEmptyCardPackSlotIndex()
        {
            for (var i = 0; i < cardPackSlots.Length; i++)
            {
                if (cardPackSlots[i].IsEmpty()) return i;
            }

            return -1;
        }

        public void AddCoin(int value)
        {
            coin += value;
        }

        public bool TryRemoveCoin(int value)
        {
            if (coin < value) return false;

            coin -= value;
            return true;
        }
        
        public bool TryUnlockSkin(SkinData skinData)
        {
            if (skins.Contains(skinData))
            {
                return false;
            }

            var newSkins = new SkinData[skins.Length + 1];

            for (var i = 0; i < skins.Length; i++)
            {
                newSkins[i] = skins[i];
            }

            newSkins[^1] = skinData;
            skins = newSkins;

            return true;
        }

        public bool TryAddCardPack(CardPackType cardPackType)
        {
            var slotIndex = GetFirstEmptyCardPackSlotIndex();

            if (slotIndex < 0) return false;

            var slot = cardPackSlots[slotIndex];
            slot.cardPackType = cardPackType;
            cardPackSlots[slotIndex] = slot;

            return true;
        }

        public void AddUpgradeToken(int value, CardRarity rarity)
        {
            var index = (int) rarity;
            upgradeTokens[index] += value;
        }
        
        public bool TryRemoveUpgradeToken(int value, CardRarity rarity)
        {
            if (value <= 0) return false;

            var index = (int) rarity;

            var isEnough = upgradeTokens[index] >= value;

            if (!isEnough) return false;

            upgradeTokens[index] -= value;
            return true;
        }
        
        
        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}