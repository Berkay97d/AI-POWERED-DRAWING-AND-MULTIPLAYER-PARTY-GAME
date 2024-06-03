using _CardPacks.Scripts;
using _UserOperations.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EnterAndExitUI_Competation.Scripts
{
    public class MinigameRewarder : MonoBehaviour
    {
        [SerializeField] private ExitTest_Script exit;
        [SerializeField] private GameObject coinRewardMain;
        [SerializeField] private GameObject cardPackRewardMain;
        [SerializeField] private GameObject cardPackRewardVisual;
        [SerializeField] private GameObject cardPackSlotsFullWarning;
        [SerializeField] private TMP_Text coinRewardField;
        [SerializeField] private TMP_Text cardPackNameField;
        [SerializeField] private Image cardPackIcon;
        [SerializeField] private CardPackTableSO cardPackTable;


        private CardPackType m_CardPackPrize = CardPackType.None;
        private int m_CoinPrize;
        private bool m_IsCardPackSlotsFull;
        

        private void Awake()
        {
            exit.OnMinigameResult += OnMinigameResult;
            exit.OnPreviewPrizes += OnPreviewPrizes;
            
            coinRewardMain.SetActive(false);
            cardPackRewardMain.SetActive(false);
        }

        private void OnDestroy()
        {
            exit.OnMinigameResult -= OnMinigameResult;
            exit.OnPreviewPrizes -= OnPreviewPrizes;
        }

        
        private void OnMinigameResult(MinigameResult result)
        {
            if (result.placement > 1) return;

            var coinReward = GetCoinReward();
            var cardPackReward = GetRandomCardPackReward();
            
            SetCoinReward(coinReward);
            SetCardPackReward(cardPackReward);
            
            LocalUser.GetUserData(userData =>
            {
                userData.AddCoin(coinReward);

                if (!userData.TryAddCardPack(cardPackReward))
                {
                    m_IsCardPackSlotsFull = true;
                }
                
                LocalUser.SetUserData(userData);
            });
        }

        private void OnPreviewPrizes()
        {
            if (m_CoinPrize > 0)
            {
                coinRewardMain.SetActive(true);
            }

            if (m_CardPackPrize != CardPackType.None)
            {
                cardPackRewardMain.SetActive(true);
            }
            
            cardPackRewardVisual.SetActive(!m_IsCardPackSlotsFull);
            cardPackSlotsFullWarning.SetActive(m_IsCardPackSlotsFull);
        }


        private int GetCoinReward()
        {
            return 500;
        }

        private void SetCoinReward(int value)
        {
            m_CoinPrize = value;
            coinRewardField.text = $"+{value}";
        }
        
        private CardPackType GetRandomCardPackReward()
        {
            var randomInt = Random.Range(0, 100);

            return randomInt switch
            {
                < 85 => CardPackType.Common,
                < 99 => CardPackType.Rare,
                _ => CardPackType.Legendary
            };
        }

        private void SetCardPackReward(CardPackType cardPackType)
        {
            m_CardPackPrize = cardPackType;
            
            var cardPackData = cardPackTable.GetCardPackData(cardPackType);

            cardPackIcon.sprite = cardPackData.icon;
            cardPackNameField.text = cardPackData.name.Replace(' ', '\n');
        }
    }


    public struct MinigameResult
    {
        public int placement;
        public int playerCount;


        public override string ToString()
        {
            return $"Minigame Result: {placement}/{playerCount}";
        }
    }
}