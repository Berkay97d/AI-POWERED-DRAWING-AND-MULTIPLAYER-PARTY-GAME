using System;
using _DatabaseAPI.Scripts;

namespace _UserOperations.Scripts
{
    public static class UserAPI
    {
        public const string NullUserID = "NULL_USER_ID";
        
        
        public static void GetUserData(string userID, Action<UserData> onCompleted, Action onFailed = default)
        {
            DatabaseAPI.GetUserByID(userID, response =>
            {
                onCompleted?.Invoke(response.user_info);
            }, onFailed);
        }

        public static void SetUserData(string userID, UserData userData, Action onSucceed = default, Action onFailed = default)
        {
            DatabaseAPI.PostUser(userID, userData, _ => onSucceed?.Invoke(), onFailed);
        }
    }
}