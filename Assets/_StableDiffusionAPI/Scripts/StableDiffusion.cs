using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace StableDiffusionAPI
{
    public static class StableDiffusion
    {
        private static StableDiffusionRunner ms_Runner;
        
        
        public static void ImageToImage(string prompt, string base64, Action<Texture2D[], string> onCompleted)
        {
            const string url = "http://18.196.98.244/sdapi/v1/img2img";
            const int width = 320;
            const int height = 320;
            
            
            var args = ImageToImageArgs.Default;

            args.init_images[0] = "data:image/png;base64," + base64;
            args.prompt = prompt;
            args.width = width;
            args.height = height;
            
            var data = JsonUtility.ToJson(args);
            
            StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                var byteData = new UTF8Encoding().GetBytes(data);
                
                using var webRequest = UnityWebRequest.Post(url, data);
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
                        var jsonResponse = webRequest.downloadHandler.text;
                        var response = JsonUtility.FromJson<ImageToImageResponse>(jsonResponse);
                        var imagePhase1AsBase64 = response.images[0];
                        var texture1 = TextureUtils.FromBase64String(imagePhase1AsBase64, width, height);

                        onCompleted?.Invoke(new []{texture1}, imagePhase1AsBase64);
                        
                        yield break;
                    }

                    default:
                    {
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

        private static StableDiffusionRunner GetRunner()
        {
            if (ms_Runner) return ms_Runner;

            ms_Runner = new GameObject($"[{nameof(StableDiffusionRunner)}]")
                .AddComponent<StableDiffusionRunner>();

            Object.DontDestroyOnLoad(ms_Runner);
            
            return ms_Runner;
        }


        private class StableDiffusionRunner : MonoBehaviour {}
    }
}