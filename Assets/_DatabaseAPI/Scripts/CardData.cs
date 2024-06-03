using System;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _DatabaseAPI.Scripts
{
    [Serializable]
    public struct CardData
    {
        public string imageID;
        public int score;
        public string subTitle;
        public int inCollection;


        public CardRarity Rarity
        {
            get
            {
                return score switch
                {
                    < 25 => CardRarity.Common,
                    < 50 => CardRarity.Rare,
                    < 75 => CardRarity.Epic,
                    _ => CardRarity.Legendary
                };
            }
        }


        public void Upgrade()
        {
            score += 25;
        }
        
        
        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }


        public static int GetRandomScoreByRarity(CardRarity rarity)
        {
            return rarity switch
            {
                CardRarity.Common => Random.Range(10, 25),
                CardRarity.Rare => Random.Range(25, 50),
                CardRarity.Epic => Random.Range(50, 75),
                CardRarity.Legendary => Random.Range(75, 101),
                _ => 10
            };
        }
    }
}