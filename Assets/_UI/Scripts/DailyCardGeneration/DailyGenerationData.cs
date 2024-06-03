using System;
using _TimeAPI.Scripts;

namespace UI
{
    [Serializable]
    public struct DailyGenerationData
    {
        public TimeData startTime;


        public static readonly DailyGenerationData Default = new()
        {
            startTime = TimeData.Zero
        };
    }
}