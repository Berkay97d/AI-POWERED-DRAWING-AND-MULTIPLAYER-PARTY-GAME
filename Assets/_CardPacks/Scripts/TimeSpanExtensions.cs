using System;

namespace _CardPacks.Scripts
{
    public static class TimeSpanExtensions
    {
        public static string ToCountdownString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalMilliseconds < 0) return "0s";
            
            if (timeSpan.Days > 0)
            {
                return $"{timeSpan.Days}d {timeSpan.Hours}h";
            }

            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
            }

            if (timeSpan.Minutes > 0)
            {
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            }

            return $"{timeSpan.Seconds}s";
        }
    }
}