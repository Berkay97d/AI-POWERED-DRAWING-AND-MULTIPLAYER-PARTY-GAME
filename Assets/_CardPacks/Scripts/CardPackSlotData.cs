using System;
using _TimeAPI.Scripts;

namespace _CardPacks.Scripts
{
    [Serializable]
    public struct CardPackSlotData
    {
        public CardPackType cardPackType;
        public TimeData startTime;


        public bool IsEmpty()
        {
            return cardPackType <= CardPackType.None;
        }
        
        public bool IsStartedUnlocking()
        {
            return !startTime.Equals(TimeData.Zero);
        }
        
        
        public static readonly CardPackSlotData Default = new()
        {
            cardPackType = CardPackType.None,
            startTime = TimeData.Zero
        };
    }
}