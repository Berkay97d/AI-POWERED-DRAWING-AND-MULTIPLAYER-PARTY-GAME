using UnityEngine;
using UnityEngine.UI;

namespace _CardPacks.Scripts
{
    public class CardPackProfitSummaryUI : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup profitsGrid;
        [SerializeField] private CardPackPrizeUI coinPrefab;
        [SerializeField] private CardPackPrizeUI gemPrefab;
        [SerializeField] private CardPackPrizeUI commonUpgradeTokenPrefab;
        [SerializeField] private CardPackPrizeUI rareUpgradeTokenPrefab;
        [SerializeField] private CardPackPrizeUI epicUpgradeTokenPrefab;
        [SerializeField] private CardPackPrizeUI legendaryUpgradeTokenPrefab;


        private void Awake()
        {
            gameObject.SetActive(false);
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void SetProfit(CardPackProfit profit)
        {
            if (profit.coin > 0)
            {
                var coin = CreatePrize(coinPrefab);
                coin.SetValue(profit.coin);
            }

            if (profit.gem > 0)
            {
                var gem = CreatePrize(gemPrefab);
                gem.SetValue(profit.gem);
            }

            if (profit.commonUpgradeToken > 0)
            {
                var commonUpgradeToken = CreatePrize(commonUpgradeTokenPrefab);
                commonUpgradeToken.SetValue(profit.commonUpgradeToken);
            }
            
            if (profit.rareUpgradeToken > 0)
            {
                var rareUpgradeToken = CreatePrize(rareUpgradeTokenPrefab);
                rareUpgradeToken.SetValue(profit.rareUpgradeToken);
            }
            
            if (profit.epicUpgradeToken > 0)
            {
                var epicUpgradeToken = CreatePrize(epicUpgradeTokenPrefab);
                epicUpgradeToken.SetValue(profit.epicUpgradeToken);
            }
            
            if (profit.legendaryUpgradeToken > 0)
            {
                var legendaryUpgradeToken = CreatePrize(legendaryUpgradeTokenPrefab);
                legendaryUpgradeToken.SetValue(profit.legendaryUpgradeToken);
            }
        }


        private CardPackPrizeUI CreatePrize(CardPackPrizeUI prefab)
        {
            var prize = Instantiate(prefab, profitsGrid.transform);
            return prize;
        }
    }
}