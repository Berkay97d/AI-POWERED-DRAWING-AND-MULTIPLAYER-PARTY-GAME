using System;
using UnityEngine;

namespace _CardPacks.Scripts
{
    [CreateAssetMenu]
    public class CardPackTableSO : ScriptableObject
    {
        [SerializeField] private CardPackData[] cardPackDatas;


        public CardPackData GetCardPackData(CardPackType type)
        {
            return cardPackDatas[type.ToIndex()];
        }
    }


    [Serializable]
    public struct CardPackData
    {
        public string name;
        public CardPack cardPackPrefab;
        public GameObject visualPrefab;
        public CardPackContent content;
        public Duration unlockDuration;
        public Sprite icon;
    }

    [Serializable]
    public struct Duration
    {
        public int days;
        public int hours;
        public int minutes;
        public int seconds;


        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(days, hours, minutes, seconds);
        }
    }
}