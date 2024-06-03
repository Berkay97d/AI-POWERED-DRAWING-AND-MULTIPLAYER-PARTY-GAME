using System;

namespace _CardPacks.Scripts
{
    [Serializable]
    public struct CardPackContent
    {
        public int minCoin;
        public int maxCoin;
        public int minGem;
        public int maxGem;
        public int minCommonUpgradeToken;
        public int maxCommonUpgradeToken;
        public int minRareUpgradeToken;
        public int maxRareUpgradeToken;
        public int minEpicUpgradeToken;
        public int maxEpicUpgradeToken;
        public int minLegendaryUpgradeToken;
        public int maxLegendaryUpgradeToken;


        public int GetMinTokenCount()
        {
            var sum = minCommonUpgradeToken;
            sum += minRareUpgradeToken;
            sum += minEpicUpgradeToken;
            sum += minLegendaryUpgradeToken;
            return sum;
        }

        public int GetMaxTokenCount()
        {
            var sum = maxCommonUpgradeToken;
            sum += maxRareUpgradeToken;
            sum += maxEpicUpgradeToken;
            sum += maxLegendaryUpgradeToken;
            return sum;
        }
    }
}