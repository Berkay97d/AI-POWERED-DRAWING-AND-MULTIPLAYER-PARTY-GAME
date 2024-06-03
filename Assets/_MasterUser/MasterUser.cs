using System;
using _DatabaseAPI.Scripts;

namespace _MasterUser
{
    public static class MasterUser
    {
        public const string MasterUserId = "volvoxwashere";
        
        
        public static void GetCards(Action<DatabaseAPI.CardPostResponseData[]> onSucceed, Action<FailReason> onFailed = default)
        {
            DatabaseAPI.GetCardsByUserID(MasterUserId, onSucceed, onFailed);
        }
    }
}