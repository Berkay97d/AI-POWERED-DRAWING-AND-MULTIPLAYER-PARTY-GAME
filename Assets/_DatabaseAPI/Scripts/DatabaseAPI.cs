using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace _DatabaseAPI.Scripts
{
    public static partial class DatabaseAPI
    {
        private static DatabaseRunner ms_Runner;


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
                        Debug.LogError($"{webRequest.responseCode} : {webRequest.downloadHandler.text}");
                        break;
                    }
                }
            }
        }

        private static void SendPostRequest(string url, string postData, Action<UnityWebRequest> onSucceed, Action<UnityWebRequest> onFailed = default)
        {
            StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                var byteData = new UTF8Encoding().GetBytes(postData);
                
                using var webRequest = UnityWebRequest.Post(url, postData);
                using var uploadHandler = new UploadHandlerRaw(byteData);
                using var downloadHandler = new DownloadHandlerBuffer();
                
                webRequest.uploadHandler = uploadHandler;
                webRequest.downloadHandler = downloadHandler;
                webRequest.SetRequestHeader("Content-Type", "application/json");

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
                        Debug.LogError($"{webRequest.responseCode} : {webRequest.downloadHandler.text}");
                        break;
                    }
                }
            }
        }

        private static void SendDeleteRequest(string url, Action<UnityWebRequest> onSucceed, Action<UnityWebRequest> onFailed = default)
        {
            StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                using var webRequest = UnityWebRequest.Delete(url);
                using var downloadHandler = new DownloadHandlerBuffer();

                webRequest.downloadHandler = downloadHandler;
            
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
                        Debug.LogError($"{webRequest.responseCode} : {webRequest.downloadHandler.text}");
                        break;
                    }
                }
            }
        }

        private static void SendPutRequest(string url, string putData, Action<UnityWebRequest> onSucceed, Action<UnityWebRequest> onFailed = default)
        {
            StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                var byteData = new UTF8Encoding().GetBytes(putData);
                
                using var webRequest = UnityWebRequest.Put(url, putData);
                using var uploadHandler = new UploadHandlerRaw(byteData);
                using var downloadHandler = new DownloadHandlerBuffer();
                
                webRequest.uploadHandler = uploadHandler;
                webRequest.downloadHandler = downloadHandler;
                webRequest.SetRequestHeader("Content-Type", "application/json");
                
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
                        Debug.LogError($"{webRequest.responseCode} : {webRequest.downloadHandler.text}");
                        break;
                    }
                }
            }
        }
        
        private static Coroutine StartCoroutine(IEnumerator routine)
        {
            return GetRunner().StartCoroutine(routine);
        }
        
        private static DatabaseRunner GetRunner()
        {
            if (ms_Runner) return ms_Runner;
            
            ms_Runner = new GameObject($"[{nameof(DatabaseRunner)}]")
                .AddComponent<DatabaseRunner>();
                
            Object.DontDestroyOnLoad(ms_Runner);

            return ms_Runner;
        }
        
        
        private class DatabaseRunner : MonoBehaviour {}
    }
}