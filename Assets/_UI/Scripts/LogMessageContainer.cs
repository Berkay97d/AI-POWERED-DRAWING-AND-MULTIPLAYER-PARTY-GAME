using UnityEngine;

namespace UI
{
    [CreateAssetMenu]
    public class LogMessageContainer : ScriptableObject
    {
        private const string Path = nameof(LogMessageContainer);


        [SerializeField, Multiline] private string
            notAvailable,
            offerAlreadyClaimed,
            notEnoughUpgradeToken,
            notEnoughGem,
            notEnoughCoin;


        public static string NotAvailable => GetInstance().notAvailable;
        public static string OfferAlreadyClaimed => GetInstance().offerAlreadyClaimed;
        public static string NotEnoughUpgradeToken => GetInstance().notEnoughUpgradeToken;
        public static string NotEnoughGem => GetInstance().notEnoughGem;
        public static string NotEnoughCoin => GetInstance().notEnoughCoin;
        

        private static LogMessageContainer ms_Instance;


        private static LogMessageContainer GetInstance()
        {
            if (ms_Instance) return ms_Instance;

            ms_Instance = Resources.Load<LogMessageContainer>(Path);

            return ms_Instance;
        }
    }
}