using System;
using _PlayerCustomization.Scripts;

namespace _Market.Scripts
{
    [Serializable]
    public struct DailyOfferData
    {
        public string title;
        public int amount;
        public int cost;
        public CurrencyType costCurrency;
        public TradeType tradeType;
        public SkinData skinData;
        public bool isClaimed;


        public static readonly DailyOfferData None = new() {title = "None"};
    }
}