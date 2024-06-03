using System;
using UnityEngine;

namespace _DatabaseAPI.Scripts
{
    public static partial class DatabaseAPI
    {
        public static void GetCardsByUserID(string userID, Action<CardPostResponseData[]> onSucceed, Action<FailReason> onFailed = default)
        {
            var url = $"https://yyui3iyhkl.execute-api.eu-central-1.amazonaws.com/prod?user_id={userID}";
            
            SendGetRequest(url, webRequest =>
            {
                var jsonResponse = webRequest.downloadHandler.text;
                var responseData = JsonUtils.ToArrayOf<CardPostResponseData>(jsonResponse);
                
                onSucceed?.Invoke(responseData);
            }, webRequest =>
            {
                onFailed?.Invoke(webRequest.responseCode == 404 ? FailReason.NotFound : FailReason.NetworkError);
            });
        }
        
        public static void PostCard(string userID, CardData cardData, Action<CardPostResponseData> onSucceed, Action onFailed = default)
        {
            const string url = "https://hogy2nbhy3.execute-api.eu-central-1.amazonaws.com/prod";

            
            var postData = new CardPostData()
            {
                user_id = userID,
                card_data = cardData
            };

            var postDataAsJson = JsonUtility.ToJson(postData);
            
            
            SendPostRequest(url, postDataAsJson, webRequest =>
            {
                var jsonResponse = webRequest.downloadHandler.text;
                var response = JsonUtility.FromJson<CardPostResponseData>(jsonResponse);

                onSucceed?.Invoke(response);
            }, webRequest =>
            {
                onFailed?.Invoke();
            });
        }

        public static void DeleteCard(string userID, string cardID, Action onSucceed, Action onFailed = default)
        {
            var url = $"https://f6admuwsma.execute-api.eu-central-1.amazonaws.com/prod/cards?user_id={userID}&card_id={cardID}";
            
            SendDeleteRequest(url, webRequest => onSucceed?.Invoke(), webRequest => onFailed?.Invoke());
        }

        public static void UpdateCard(string userID, string cardID, string newUserID, CardData cardData, Action onSucceed, Action onFailed = default)
        {
            var url = $"https://f6admuwsma.execute-api.eu-central-1.amazonaws.com/prod/cards?user_id={userID}&card_id={cardID}";

            var userChanged = userID != newUserID;

            if (userChanged)
            {
                cardData.inCollection = 0;
            }
            
            var data = new CardPostData()
            {
                user_id = newUserID,
                card_data = cardData
            };

            var dataAsJson = JsonUtility.ToJson(data);
            
            SendPutRequest(url, dataAsJson, webRequest => onSucceed?.Invoke(), webRequest => onFailed?.Invoke());
        }

        



        
        [Serializable]
        private struct CardPostData
        {
            public string user_id;
            public CardData card_data;
        }
        
        [Serializable]
        public struct CardPostResponseData
        {
            public string user_id;
            public string card_id;
            public CardData card_data;


            public override string ToString()
            {
                return $"user_id : {user_id}\ncard_id : {card_id}\n{card_data}";
            }
        }
    }
}