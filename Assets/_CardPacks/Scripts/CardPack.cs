using System;
using System.Collections.Generic;
using _UserOperations.Scripts;
using General;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _CardPacks.Scripts
{
    public class CardPack : MonoBehaviour
    {
        [SerializeField] private CardPackTableSO cardPackTable;
        [SerializeField] private CardPackPrize coinPrizePrefab;
        [SerializeField] private CardPackPrize gemPrizePrefab;
        [SerializeField] private CardPackPrize commonUpgradeTokenPrefab;
        [SerializeField] private CardPackPrize rareUpgradeTokenPrefab;
        [SerializeField] private CardPackPrize epicUpgradeTokenPrefab;
        [SerializeField] private CardPackPrize legendaryUpgradeTokenPrefab;
        [SerializeField] private CardPackProfitSummaryUI profitSummary;
        [SerializeField] private Transform prizeCardsParent;
        [SerializeField] private CardPackAnimator animator;
        [SerializeField] private ParticleSystem outlineParticle;
        [SerializeField] private GameObject clickCountMain;
        [SerializeField] private TMP_Text clickCountField;
        [SerializeField] private CardPackType type;


        public event Action OnCompleted;
        

        private readonly List<Action> m_PullPrizeOrder = new();
        private CardPackPrize m_LastPulledPrizeCard;
        private CardPackProfit m_Profit;
        private Action m_OnClick;
        private int m_PrizeIndex;


        private void Awake()
        {
            CreatePullPrizeOrder();

            profitSummary.SetProfit(m_Profit);
            ApplyProfit();
            
            m_OnClick = Open;
            
            animator.OnOpenCompleted += Animator_OnOpenCompleted;
        }

        
        private void Animator_OnOpenCompleted()
        {
            m_OnClick = PullNextPrize;
        }


        public void OnClick()
        {
            m_OnClick?.Invoke();
        }

        private void Open()
        {
            m_OnClick = null;
            
            outlineParticle.Stop();

            LazyCoroutines.WaitForSeconds(0.2f, () =>
            {
                animator.PlayOpen();
            });
        }

        private void CreatePullPrizeOrder()
        {
            var content = GetContent();
            var coinCount = Random.Range(content.minCoin, content.maxCoin + 1);

            if (coinCount > 0)
            {
                m_Profit.coin += coinCount;
                m_PullPrizeOrder.Add(() => PullCoinPrize(coinCount));
            }

            var gemCount = Random.Range(content.minGem, content.maxGem + 1);

            if (gemCount > 0)
            {
                m_Profit.gem += gemCount;
                m_PullPrizeOrder.Add(() => PullGemPrize(gemCount));
            }

            var commonUpgradeTokenCount = Random.Range(content.minCommonUpgradeToken, content.maxCommonUpgradeToken + 1);

            if (commonUpgradeTokenCount > 0)
            {
                m_Profit.commonUpgradeToken += commonUpgradeTokenCount;
                m_PullPrizeOrder.Add(() => PullCommonUpgradeTokenPrize(commonUpgradeTokenCount));
            }
            
            var rareUpgradeTokenCount = Random.Range(content.minRareUpgradeToken, content.maxRareUpgradeToken + 1);

            if (rareUpgradeTokenCount > 0)
            {
                m_Profit.rareUpgradeToken += rareUpgradeTokenCount;
                m_PullPrizeOrder.Add(() => PullRareUpgradeTokenPrize(rareUpgradeTokenCount));
            }
            
            var epicUpgradeTokenCount = Random.Range(content.minEpicUpgradeToken, content.maxEpicUpgradeToken + 1);

            if (epicUpgradeTokenCount > 0)
            {
                m_Profit.epicUpgradeToken += epicUpgradeTokenCount;
                m_PullPrizeOrder.Add(() => PullEpicUpgradeTokenPrize(epicUpgradeTokenCount));
            }
            
            var legendaryUpgradeTokenCount = Random.Range(content.minLegendaryUpgradeToken, content.maxLegendaryUpgradeToken + 1);

            if (legendaryUpgradeTokenCount > 0)
            {
                m_Profit.legendaryUpgradeToken += legendaryUpgradeTokenCount;
                m_PullPrizeOrder.Add(() => PullLegendaryUpgradeTokenPrize(legendaryUpgradeTokenCount));
            }
            
            SetClickCount(m_PullPrizeOrder.Count);
        }

        private void ApplyProfit()
        {
            gameObject.SetActive(false);
            
            LocalUser.GetUserData(data =>
            {
                data.coin += m_Profit.coin;
                data.gem += m_Profit.gem;
                data.upgradeTokens[0] += m_Profit.commonUpgradeToken;
                data.upgradeTokens[1] += m_Profit.rareUpgradeToken;
                data.upgradeTokens[2] += m_Profit.epicUpgradeToken;
                data.upgradeTokens[3] += m_Profit.legendaryUpgradeToken;
                
                LocalUser.SetUserData(data, () =>
                {
                    gameObject.SetActive(true);
                });
            });
        }

        private void ShowProfitSummary()
        {
            m_LastPulledPrizeCard.Disappear();
            clickCountMain.SetActive(false);
            profitSummary.Show();

            m_OnClick = Complete;
        }

        private void Complete()
        {
            OnCompleted?.Invoke();
            m_OnClick = null;
        }

        private void PullNextPrize()
        {
            if (m_LastPulledPrizeCard)
            {
                m_LastPulledPrizeCard.Disappear();
            }
            
            var action = m_PullPrizeOrder[m_PrizeIndex];
            action?.Invoke();
            m_PrizeIndex += 1;
            animator.PlayPull();
            
            SetClickCount(m_PullPrizeOrder.Count - m_PrizeIndex);
            
            if (m_PrizeIndex < m_PullPrizeOrder.Count) return;

            m_OnClick = ShowProfitSummary;
        }

        private void PullCoinPrize(int count)
        {
            PullPrize(coinPrizePrefab, count);
        }

        private void PullGemPrize(int count)
        {
            PullPrize(gemPrizePrefab, count);
        }

        private void PullCommonUpgradeTokenPrize(int count)
        {
            PullPrize(commonUpgradeTokenPrefab, count);
        }

        private void PullRareUpgradeTokenPrize(int count)
        {
            PullPrize(rareUpgradeTokenPrefab, count);
        }
        
        private void PullEpicUpgradeTokenPrize(int count)
        {
            PullPrize(epicUpgradeTokenPrefab, count);
        }
        
        private void PullLegendaryUpgradeTokenPrize(int count)
        {
            PullPrize(legendaryUpgradeTokenPrefab, count);
        }
        
        private void PullPrize(CardPackPrize prefab, int count)
        {
            var prize = SpawnCardPackPrize(prefab);
            
            prize.SetValue(count);
            prize.SetOrder(m_PrizeIndex);
            prize.Preview();
            
            m_LastPulledPrizeCard = prize;
        }

        private CardPackPrize SpawnCardPackPrize(CardPackPrize prefab)
        {
            return Instantiate(prefab, prizeCardsParent);
        }

        private void SetClickCount(int value)
        {
            clickCountField.text = value.ToString();
        }

        private CardPackData GetData()
        {
            return cardPackTable.GetCardPackData(type);
        }

        private CardPackContent GetContent()
        {
            return GetData().content;
        }
    }
}