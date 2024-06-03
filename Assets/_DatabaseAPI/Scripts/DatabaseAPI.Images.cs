using System;
using System.Collections.Generic;
using UnityEngine;

namespace _DatabaseAPI.Scripts
{
    public static partial class DatabaseAPI
    {
        private static readonly Dictionary<string, ImageGetResponseData> m_Images = new();


        public static void GetImageByID(string imageID, Action<ImageGetResponseData> onSucceed, Action onFailed = default)
        {
            if (m_Images.TryGetValue(imageID, out var value))
            {
                onSucceed?.Invoke(value);
                return;
            }
            
            var url = $"https://9bdp7uvrn7.execute-api.eu-central-1.amazonaws.com/prod?image_id={imageID}";
            
            SendGetRequest(url, webRequest =>
            {
                var jsonResponse = webRequest.downloadHandler.text;
                var responseData = JsonUtility.FromJson<ImageGetResponseData>(jsonResponse);
                responseData.base64_image = responseData.base64_image.Substring(1, responseData.base64_image.Length - 2);

                m_Images[imageID] = responseData;
                onSucceed?.Invoke(responseData);
            }, webRequest =>
            {
                onFailed?.Invoke();
            });
        }

        public static void PostImage(string imageAsBase64, Action<ImagePostResponseData> onSucceed, Action onFailed = default)
        {
            const string url = "https://ah32pb2rhd.execute-api.eu-central-1.amazonaws.com/dev/images";

            SendPostRequest(url, imageAsBase64, webRequest =>
            {
                var jsonResponse = webRequest.downloadHandler.text;
                var responseData = JsonUtility.FromJson<ImagePostResponseData>(jsonResponse);
                
                onSucceed?.Invoke(responseData);
            }, webRequest =>
            {
                onFailed?.Invoke();
            });
        }
        
        
        
        
        [Serializable]
        public struct ImageGetResponseData
        {
            public string base64_image;
            public string image_id;


            public override string ToString()
            {
                return $"base64_image : {base64_image}\nimage_id : {image_id}";
            }
        }

        [Serializable]
        public struct ImagePostResponseData
        {
            public int statusCode;
            public string body;


            public override string ToString()
            {
                return $"statusCode : {statusCode}\nbody : {body}";
            }
        }
    }
}