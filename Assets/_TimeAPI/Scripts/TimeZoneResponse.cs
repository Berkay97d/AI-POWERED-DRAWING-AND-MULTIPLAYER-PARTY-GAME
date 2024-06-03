using System;
using UnityEngine;

namespace _TimeAPI.Scripts
{
    [Serializable]
    public struct TimeZoneResponse
    {
        public string timeZone;
        public string currentLocalTime;
        public UtcOffset currentUtcOffset;
        public UtcOffset standardUtcOffset;
        public bool hasDayLightSaving;
        public bool isDayLightSavingActive;
        public string dstInterval;
        
        
        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }

    [Serializable]
    public struct UtcOffset
    {
        public int seconds;
        public int milliSeconds;
        public int ticks;
        public int nanoSeconds;
    }
}