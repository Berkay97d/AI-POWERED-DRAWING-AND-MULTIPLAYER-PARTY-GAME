using System;
using System.Collections;
using TimeZoneConverter;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace _TimeAPI.Scripts
{
    public static class TimeAPI
    {
        private static TimeAPIRunner ms_Runner;
        
        
        public static void GetCurrentTime(Action<TimeData> onComplete, Action onFailed = default)
        {
            const string timeZone = "Africa/Banjul";
            var timeZoneUrl = $"https://timeapi.io/api/TimeZone/zone?timeZone={timeZone}";
                
            SendGetRequest(timeZoneUrl, timeZoneRequest =>
            {
                var jsonTimeZoneResponse = timeZoneRequest.downloadHandler.text;
                var timeZoneResponse = JsonUtility.FromJson<TimeZoneResponse>(jsonTimeZoneResponse);
                var timeData = new TimeData(timeZoneResponse);
                timeData.utcOffsetSeconds = GetUtcOffsetSeconds();
                
                onComplete?.Invoke(timeData);
            }, webRequest => onFailed?.Invoke());
        }


        private static int GetUtcOffsetSeconds()
        {
            return (int) TimeZoneInfo.Local.BaseUtcOffset.TotalSeconds;
        }

        private static void SendGetRequest(string url, Action<UnityWebRequest> onSucceed, Action<UnityWebRequest> onFailed = default)
        {
            StartCoroutine(Routine());
            

            IEnumerator Routine()
            {
                using var webRequest = UnityWebRequest.Get(url);

                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.InProgress:
                    {
                        Debug.Log("in progress");
                        yield return null;
                        break;
                    }

                    case UnityWebRequest.Result.Success:
                    {
                        onSucceed?.Invoke(webRequest);
                        yield break;
                    }

                    default:
                    {
                        onFailed?.Invoke(webRequest);
                        Debug.LogError($"TimeAPI: {webRequest.responseCode} : {webRequest.downloadHandler.text}");
                        break;
                    }
                }
            }
        }
        
        private static Coroutine StartCoroutine(IEnumerator routine)
        {
            return GetRunner().StartCoroutine(routine);
        }
        
        private static TimeAPIRunner GetRunner()
        {
            if (ms_Runner) return ms_Runner;
            
            ms_Runner = new GameObject($"[{nameof(TimeAPIRunner)}]")
                .AddComponent<TimeAPIRunner>();
                
            Object.DontDestroyOnLoad(ms_Runner);

            return ms_Runner;
        }
        
        private class TimeAPIRunner : MonoBehaviour {}
    }
}