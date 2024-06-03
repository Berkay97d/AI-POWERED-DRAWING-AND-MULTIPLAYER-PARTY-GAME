using System;
using _CardPacks.Scripts;
using _DatabaseAPI.Scripts;
using _PlayerCustomization.Scripts;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Market.Scripts
{
    public class MarketBuyButton : MonoBehaviour
    {
        [SerializeField] private CustomizationDataSO customizationData;
        [SerializeField] private Button button;
        [SerializeField] private Sprite[] costSprites;
        [SerializeField] private TMP_Text titleField;
        [SerializeField] private TMP_Text amountField;
        [SerializeField] private TMP_Text costField;
        [SerializeField] private GameObject freeText;
        [SerializeField] private Image costIcon;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject costInfoPanel;
        [SerializeField] private GameObject notAvailablePanel;
        [SerializeField] private GameObject claimedPanel;
        [SerializeField] private GameObject[] hideWhenHasNoSpecialOffer;
        
        [Header("Settings")]
        [SerializeField, Multiline] private string title;
        [SerializeField, Min(0)] private int amount;
        [SerializeField, Min(0)] private int cost;
        [SerializeField] private CurrencyType costCurrency;
        [SerializeField] private TradeType tradeType;
        [SerializeField] private CardPackType cardPackType;
        [SerializeField] private bool isAvailable = true;
        [SerializeField] private bool applyDataOnAwake = true;


        private MarketManager m_MarketManager;
        private Action m_OnConfirmed;
        private SkinData m_SkinData;
        private DailyOfferData m_DailyOfferData;
        private bool m_Interactable;


        private void Awake()
        {
            button.onClick.AddListener(OnClicked);

            if (applyDataOnAwake)
            {
                m_Interactable = true;
                SetVisuals();
                SetTradeType(tradeType);
            }
        }


        private void OnClicked()
        {
            if (!m_Interactable) return;

            if (!isAvailable)
            {
                ScreenLogger.Log(LogMessageContainer.NotAvailable);
                return;
            }

            if (m_DailyOfferData.isClaimed)
            {
                ScreenLogger.Log(LogMessageContainer.OfferAlreadyClaimed);
                return;
            }
            
            m_MarketManager.OpenConfirmPanelFromBuyButton(this);
        }


        public void SetMarketManager(MarketManager marketManager)
        {
            m_MarketManager = marketManager;
        }

        public void ApplyDailyOffer(DailyOfferData data)
        {
            m_DailyOfferData = data;
            m_SkinData = data.skinData;
            
            if (data.Equals(DailyOfferData.None))
            {
                m_Interactable = false;
                
                foreach (var obj in hideWhenHasNoSpecialOffer)
                {
                    obj.SetActive(false);
                }
                
                notAvailablePanel.SetActive(true);
                
                return;
            }

            m_Interactable = true;
                
            foreach (var obj in hideWhenHasNoSpecialOffer)
            {
                obj.SetActive(true);
            }
                
            notAvailablePanel.SetActive(false);
            
            SetTitle(data.title);
            SetAmount(data.amount);
            SetCost(data.cost);
            SetCostCurrency(data.costCurrency);
            SetIsAvailable(isAvailable);
            SetTradeType(data.tradeType);

            if (data.isClaimed)
            {
                costInfoPanel.SetActive(false);
                claimedPanel.SetActive(true);
            }
        }

        public string GetTitle()
        {
            return applyDataOnAwake ? title : m_DailyOfferData.title;
        }

        public Sprite GetIcon()
        {
            return iconImage.sprite;
        }

        public Color GetIconColor()
        {
            return iconImage.color;
        }

        public int GetAmount()
        {
            return applyDataOnAwake ? amount : m_DailyOfferData.amount;
        }

        public int GetCost()
        {
            return applyDataOnAwake ? cost : m_DailyOfferData.cost;
        }

        public CurrencyType GetCostCurrency()
        {
            return applyDataOnAwake ? costCurrency : m_DailyOfferData.costCurrency;
        }
        
        public bool IsFree()
        {
            return GetCost() <= 0;
        }

        public Action GetOnConfirmedCallback()
        {
            return m_OnConfirmed;
        }
        

        private void SetTitle(string value)
        {
            titleField.text = value;
        }

        private void SetAmount(int value)
        {
            amountField.text = $"x{value}";
        }

        private void SetCost(int value)
        {
            freeText.SetActive(IsFree());
            costField.text = value.ToString();
            costField.gameObject.SetActive(!IsFree());
        }
        
        private void SetCostCurrency(CurrencyType currencyType)
        {
            costIcon.sprite = costSprites[(int) currencyType];
            costIcon.gameObject.SetActive(!IsFree());
        }

        private void SetIsAvailable(bool value)
        {
            costInfoPanel.SetActive(value);
            notAvailablePanel.SetActive(!value);
        }

        private void SetVisuals()
        {
            SetTitle(title);
            SetAmount(amount);
            SetCost(cost);
            SetCostCurrency(costCurrency);
            SetIsAvailable(isAvailable);
        }

        private void SetTradeType(TradeType trade)
        {
            if (trade == TradeType.BuySkin)
            {
                SetSkinPreview();
            }
            
            if (!isAvailable)
            {
                m_OnConfirmed = NotAvailableTrade;
                return;
            }
            
            var tradeMethod = trade switch
            {
                TradeType.BuyCoin => BuyCoin,
                TradeType.BuyGem => BuyGem,
                TradeType.BuySkin => BuySkin,
                TradeType.BuyCardPack => BuyCardPack,
                _ => m_OnConfirmed
            };

            m_OnConfirmed = () =>
            {
                var cacheUserData = LocalUser.GetUserDataFromCache();

                if (!TryWithdrawCost(ref cacheUserData))
                {
                    var msg = costCurrency switch
                    {
                        CurrencyType.Coin => LogMessageContainer.NotEnoughCoin,
                        CurrencyType.Gem => LogMessageContainer.NotEnoughGem,
                        _ => "Not enough resources!"
                    };
                    ScreenLogger.Log(msg);
                    return;
                }

                tradeMethod?.Invoke();
            };
        }

        private void SetSkinPreview()
        {
            var icon = m_SkinData.mainIndex switch
            {
                0 => customizationData.headSprites[m_SkinData.subIndex - 1],
                1 => customizationData.upperBodySprites[m_SkinData.subIndex - 1],
                2 => customizationData.lowerBodySprites[m_SkinData.subIndex - 1],
                3 => customizationData.footSprites[m_SkinData.subIndex - 1],
                4 => customizationData.bodySprite,
                _ => null
            };

            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(true);
            iconImage.color = m_SkinData.mainIndex == 4 
                ? customizationData.bodyColors[m_SkinData.subIndex] 
                : Color.white;
        }

        private void BuyCoin()
        {
            LocalUser.GetUserData(userData =>
            {
                if (TryWithdrawCost(ref userData))
                {
                    userData.coin += amount;
                    LocalUser.SetUserData(userData);
                }
            });
        }

        private void BuyGem()
        {
            NotAvailableTrade();
        }

        private void BuySkin()
        {
            LocalUser.GetUserData(userData =>
            {
                if (TryWithdrawCost(ref userData))
                {
                    if (userData.TryUnlockSkin(m_SkinData))
                    {
                        var index = GetIndex();
                        userData.dailyOffers[index].isClaimed = true;
                        LocalUser.SetUserData(userData, () =>
                        {
                            LocalUser.AddQuestProgress(QuestType.Buy_Skin);
                        });
                    }
                }
            });
        }

        private void BuyCardPack()
        {
            LocalUser.GetUserData(userData =>
            {
                if (TryWithdrawCost(ref userData))
                {
                    LocalUser.SetUserData(userData, () =>
                    {
                        CardPackOpener.OpenCardPack(cardPackType);
                    });
                }
            });
        }

        private int GetIndex()
        {
            return transform.GetSiblingIndex();
        }

        private void NotAvailableTrade()
        {
            ScreenLogger.Log(LogMessageContainer.NotAvailable);
        }

        private bool TryWithdrawCost(ref UserData userData)
        {
            if (costCurrency == CurrencyType.Coin)
            {
                if (userData.coin < cost)
                {
                    m_MarketManager.OnNotEnoughCoins();
                    return false;
                }

                userData.coin -= cost;
                return true;
            }

            if (costCurrency == CurrencyType.Gem)
            {
                if (userData.gem < cost)
                {
                    m_MarketManager.OnNotEnoughGems();
                    return false;
                }

                userData.gem -= cost;
                return true;
            }

            return false;
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            SetVisuals();
        }

#endif
    }
}