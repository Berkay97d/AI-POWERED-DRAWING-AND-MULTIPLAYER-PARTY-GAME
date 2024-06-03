using System;
using UnityEngine;

namespace _DatabaseAPI.Scripts
{
    public static partial class DatabaseAPI
    {
        public static void GetUserByID(string userID, Action<UserPostData> onSucceed, Action onFailed = default)
        {
            var url = $"https://dysod0j6c5.execute-api.eu-central-1.amazonaws.com/prod?user_id={userID}";
            
            SendGetRequest(url, webRequest =>
            {
                var jsonResponse = webRequest.downloadHandler.text;
                var responseData = JsonUtility.FromJson<UserPostData>(jsonResponse);

                onSucceed?.Invoke(responseData);
            }, webRequest =>
            {
                onFailed?.Invoke();
            });
        }

        public static void PostUser(string userID, UserData userData, Action<UserPostData> onSucceed, Action onFailed = default)
        {
            const string url = "https://2c46v2vyei.execute-api.eu-central-1.amazonaws.com/dev";

            var postData = new UserPostData()
            {
                user_id = userID,
                user_info = userData
            };

            var postDataAsJson = JsonUtility.ToJson(postData);
            
            SendPostRequest(url, postDataAsJson, webRequest =>
            {
                var jsonResponse = webRequest.downloadHandler.text;
                
                onSucceed?.Invoke(postData);
            }, webRequest =>
            {
                onFailed?.Invoke();
            });
        }
        
        
        
        
        [Serializable]
        public struct UserPostData
        {
            public string user_id;
            public UserData user_info;


            public override string ToString()
            {
                return $"user_id : {user_id}\n{user_info}";
            }
        }
    }
}