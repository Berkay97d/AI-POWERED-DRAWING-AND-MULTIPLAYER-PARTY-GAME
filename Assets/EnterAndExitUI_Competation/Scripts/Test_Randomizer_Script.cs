using System;
using System.Collections.Generic;
using System.Linq;
using _DatabaseAPI.Scripts;
using _MasterUser;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnterAndExitUI_Competation.Scripts
{
    public class Test_Randomizer_Script : MonoBehaviour
    {
        [SerializeField] private Sprite[] profilePhotos;
        [SerializeField] private string[] names;
        
        
        public string GetRandomName()
        {
            return names[Random.Range(0, names.Length)];
        }

        public int GetRandomLevel()
        {
            return Random.Range(1, 50);
        }

        public Sprite GetRandomProfilePhoto()
        {
            return profilePhotos[Random.Range(0, profilePhotos.Length)];
        }

        public DatabaseAPI.CardPostResponseData[] GetRandomBet(IReadOnlyList<DatabaseAPI.CardPostResponseData> cards)
        {
            var randomCard = Random.Range(0, cards.Count);
            return new[]{cards[randomCard]};
        }
        
        public int[] GetRandomPrizePool(int playerCount)
        {
            var count = 0;
            var pool = new List<int>();

            for (var i = 0; i < playerCount; i++)
            {
                count += Random.Range(1, 6);
            }

            for (var i = 0; i < count; i++)
            {
                var rarityID = Random.Range(0, 4);
                pool.Add(rarityID);
            }
            
            pool.Sort();
            pool.Reverse();

            return pool.ToArray();
        }
    }
}