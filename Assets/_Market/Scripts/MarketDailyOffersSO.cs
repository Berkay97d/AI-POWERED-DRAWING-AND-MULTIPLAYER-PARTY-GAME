using System;
using System.Collections.Generic;
using System.Linq;
using _PlayerCustomization.Scripts;
using UnityEngine;

namespace _Market.Scripts
{
    [CreateAssetMenu]
    public class MarketDailyOffersSO : ScriptableObject
    {
        [SerializeField] private CustomizationOffer[] headOffers;
        [SerializeField] private CustomizationOffer[] upperBodyOffers;
        [SerializeField] private CustomizationOffer[] lowerBodyOffers;
        [SerializeField] private CustomizationOffer[] footOffers;
        [SerializeField] private CustomizationOffer[] bodyColorOffers;
        
        
        private void OnValidate()
        {
            for (var i = 0; i < headOffers.Length; i++)
            {
                headOffers[i].skinData = new SkinData {mainIndex = 0, subIndex = i + 1};
            }
            
            for (var i = 0; i < upperBodyOffers.Length; i++)
            {
                upperBodyOffers[i].skinData = new SkinData {mainIndex = 1, subIndex = i + 1};
            }
            
            for (var i = 0; i < lowerBodyOffers.Length; i++)
            {
                lowerBodyOffers[i].skinData = new SkinData {mainIndex = 2, subIndex = i + 1};
            }
            
            for (var i = 0; i < footOffers.Length; i++)
            {
                footOffers[i].skinData = new SkinData {mainIndex = 3, subIndex = i + 1};
            }
            
            for (var i = 0; i < bodyColorOffers.Length; i++)
            {
                bodyColorOffers[i].skinData = new SkinData {mainIndex = 4, subIndex = i + 1};
            }
        }
        
        
        public IReadOnlyList<CustomizationOffer> GetAllOffers(SkinData[] exclude)
        {
            var offers = new List<CustomizationOffer>();

            for (var i = 0; i < headOffers.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 0, subIndex = i + 1};
                
                if (exclude.Contains(skinData)) continue;
                
                offers.Add(headOffers[i]);
            }
            
            for (var i = 0; i < upperBodyOffers.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 1, subIndex = i + 1};
                
                if (exclude.Contains(skinData)) continue;
                
                offers.Add(upperBodyOffers[i]);
            }
            
            for (var i = 0; i < lowerBodyOffers.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 2, subIndex = i + 1};
                
                if (exclude.Contains(skinData)) continue;
                
                offers.Add(lowerBodyOffers[i]);
            }
            
            for (var i = 0; i < footOffers.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 3, subIndex = i + 1};
                
                if (exclude.Contains(skinData)) continue;
                
                offers.Add(footOffers[i]);
            }
            
            for (var i = 0; i < bodyColorOffers.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 4, subIndex = i + 1};
                
                if (exclude.Contains(skinData)) continue;
                
                offers.Add(bodyColorOffers[i]);
            }
            
            return offers;
        }
    }
    
    
    [Serializable]
    public struct CustomizationOffer
    {
        public string title;
        public int cost;
        public CurrencyType costCurrency;
        public SkinData skinData;


        public DailyOfferData ToDailyOfferData()
        {
            return new DailyOfferData
            {
                amount = 1,
                cost = cost,
                costCurrency = costCurrency,
                isClaimed = false,
                skinData = skinData,
                title = title,
                tradeType = TradeType.BuySkin
            };
        }
    }
}