using NaughtyAttributes;
using UnityEngine;

namespace _TimeAPI.Scripts
{
    public class Test_Time_API : MonoBehaviour
    {
        private TimeData m_AwakeTimeData;


        private void Awake()
        {
            TimeAPI.GetCurrentTime(timeData =>
            {
                m_AwakeTimeData = timeData;
            });
        }


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void GetCurrentTime()
        {
            TimeAPI.GetCurrentTime(timeData =>
            {
                Debug.Log(timeData);
            });
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void GetElapsedRTime()
        {
            TimeAPI.GetCurrentTime(timeData =>
            {
                Debug.Log(timeData - m_AwakeTimeData);
            });
        }
    }
}