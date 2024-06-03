using System;
using UnityEngine;

namespace _TimeAPI.Scripts
{
    [Serializable]
    public struct TimeData
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int seconds;
        public int milliSeconds;
        public int utcOffsetSeconds;


        public static readonly TimeData Zero = new();
        

        public TimeData(TimeZoneResponse timeZone)
        {
            var localTime = timeZone.currentLocalTime.Split('T');
            var date = localTime[0];
            var time = localTime[1];
            var dateSplit = date.Split('-');
            var timeSplit = time.Split(':');
            var secondsSplit = timeSplit[2].Split('.');

            year = Convert.ToInt32(dateSplit[0]);
            month = Convert.ToInt32(dateSplit[1]);
            day = Convert.ToInt32(dateSplit[2]);
            
            hour = Convert.ToInt32(timeSplit[0]);
            minute = Convert.ToInt32(timeSplit[1]);
            seconds = Convert.ToInt32(secondsSplit[0]);
            milliSeconds = Convert.ToInt32(secondsSplit[1][..3]);

            utcOffsetSeconds = timeZone.standardUtcOffset.seconds;
        }
        
        public DateTime ToDateTime()
        {
            return new DateTime(year, month, day, hour, minute, seconds, milliSeconds)
                .AddSeconds(utcOffsetSeconds);
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
        
        public static TimeSpan operator -(TimeData lhs, TimeData rhs)
        {
            var secondsOffset = lhs.utcOffsetSeconds - rhs.utcOffsetSeconds;
            var timeSpan = lhs.ToDateTime() - rhs.ToDateTime();

            return timeSpan.Add(new TimeSpan(0, 0, secondsOffset));
        }
    }
}