using System;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{ 
    public class CardOptions : MonoBehaviour
    {
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button lookButton;
        [SerializeField] private Button demolishButton;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private ScrollRectEvents events;
        [SerializeField] private CraftPageCardInitilazor craftPageCardInitilazor;
        [SerializeField] private TMP_Text upgradeCoinCostField;
        [SerializeField] private TMP_Text upgradeTokenCostField;
        [SerializeField] private TMP_Text demolishCoinValueField;
        [SerializeField] private TMP_Text demolishTokenValueField;
        [SerializeField] private Image demolishTokenIcon;
        [SerializeField] private Image upgradeTokenIcon;
        [SerializeField] private Sprite[] tokenSprites;
        
        
        public event EventHandler OnUpgradeStart;
        public event EventHandler OnDemolishStart;
        public event EventHandler OnLoookStart;
        public event EventHandler<Card> OnCraftPageCardChanged;
        
        private Card card;
        
        
        private void Start()
        {
            upgradeButton.onClick.AddListener(UpgradeCard);
            demolishButton.onClick.AddListener(DemolishCard);
            lookButton.onClick.AddListener(LookCard);
            
            craftPageCardInitilazor.OnCardAddedToCraftPage += OnCardAddedToCraftPage;
            craftPageCardInitilazor.OnCardRemovedFromCraftPage += OnCardRemovedFromCraftPage;
            
            events.onBeginVerticalDrag.AddListener(() =>
            {
                CloseButtons();
            });
        }

        private void OnDestroy()
        {
            craftPageCardInitilazor.OnCardAddedToCraftPage += OnCardAddedToCraftPage;
            craftPageCardInitilazor.OnCardRemovedFromCraftPage += OnCardRemovedFromCraftPage;
        }

        private void OnCardRemovedFromCraftPage(object sender, Card e)
        {
            e.OnCardClicked -= OnCardClicked;
        }

        private void OnCardAddedToCraftPage(object sender, Card e)
        {
            e.OnCardClicked += OnCardClicked;
        }

        private void Update()
        {
            if (card)
            {
                transform.position = card.transform.position;
            }
        }

        private void OnCardClicked(object sender, Card e)
        {
            if (!MenuManager.IsCraftMenuActive())
            {
                return;
            }
            
            SetCard(e);
            
            if (card)
            {
                OpenButtons();
            }
        }

        private void CloseButtons()
        {
            const float duration = 0.2f;

            var tappedTransform = card.GetTappedTransform();
            
            buttonsParent.DOScale(Vector3.zero, duration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    buttonsParent.localScale = Vector3.zero;
                    buttonsParent.gameObject.SetActive(false);
                });

            tappedTransform.DOScale(Vector3.zero, duration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    tappedTransform.localScale = Vector3.zero;
                    tappedTransform.gameObject.SetActive(false);
                });
        }

        private void OpenButtons()
        {
            const float duration = 0.2f;
            
            var tappedTransform = card.GetTappedTransform();
            
            buttonsParent.gameObject.SetActive(true);
            tappedTransform.gameObject.SetActive(true);
            
            buttonsParent.DOScale(Vector3.one, duration)
                .From(Vector3.zero)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    buttonsParent.localScale = Vector3.one;
                });

            tappedTransform.DOScale(Vector3.one, duration)
                .From(Vector3.zero)
                .SetEase(Ease.OutBack);
        }
        
        private void DemolishCard()
        {
            CloseButtons();
            
            OnDemolishStart?.Invoke(this, EventArgs.Empty);

            LocalUser.RemoveCard(card.GetCardId(), card.GetCardData(), () =>
            {
                LocalUser.GetUserData(userData =>
                {
                    userData.AddCoin(card.GetDemolishCoinValue());
                    userData.AddUpgradeToken(card.GetDemolishTokenValue(), card.GetCardData().Rarity);
                    
                    LocalUser.SetUserData(userData);
                });
            });
        }

        private void UpgradeCard()
        {
            var data = card.GetCardData();
            var userData = LocalUser.GetUserDataFromCache();

            if (!userData.TryRemoveCoin(card.GetUpgradeCoinCost()))
            {
                ScreenLogger.Log(LogMessageContainer.NotEnoughCoin);
                return;
            }
            
            if (!userData.TryRemoveUpgradeToken(card.GetUpgradeTokenCost(), data.Rarity))
            {
                ScreenLogger.Log(LogMessageContainer.NotEnoughUpgradeToken);
                return;
            }

            LocalUser.GetUserData(newUserData =>
            {
                if (!newUserData.TryRemoveCoin(card.GetUpgradeCoinCost()))
                {
                    ScreenLogger.Log(LogMessageContainer.NotEnoughCoin);
                    return;
                }
            
                if (!newUserData.TryRemoveUpgradeToken(card.GetUpgradeTokenCost(), data.Rarity))
                {
                    ScreenLogger.Log(LogMessageContainer.NotEnoughUpgradeToken);
                    return;
                }
                
                data.Upgrade();
                
                LocalUser.SetUserData(newUserData, () =>
                {
                    OnUpgradeStart?.Invoke(this, EventArgs.Empty);
                    LocalUser.UpdateCard(card.GetCardId(), data);
                });
            });
        }
        
        private void LookCard()
        {
            OnLoookStart?.Invoke(this, EventArgs.Empty);
        }

        private void SetCard(Card c)
        {
            if (c == card)
            {
                CloseButtons();
                card = null;
                return;
            }
            
            if (card)
            {
                card.GetTappedTransform().gameObject.SetActive(false);
            }
            
            card = c;
            SetUpgradeCoinCost(card.GetUpgradeCoinCost());
            SetUpgradeTokenCost(card.GetUpgradeTokenCost(), card.GetCardData().Rarity);
            SetDemolishCoinValue(card.GetDemolishCoinValue());
            SetDemolishTokenValue(card.GetDemolishTokenValue(), card.GetCardData().Rarity);
            OnCraftPageCardChanged?.Invoke(this,c);
        }

        public Card GetCard()
        {
            return card;
        }

        private void SetUpgradeCoinCost(int value)
        {
            upgradeCoinCostField.text = value.ToString();
        }

        private void SetUpgradeTokenCost(int value, CardRarity rarity)
        {
            upgradeTokenCostField.text = value.ToString();
            upgradeTokenIcon.sprite = tokenSprites[(int) rarity];
        }

        private void SetDemolishCoinValue(int value)
        {
            demolishCoinValueField.text = value.ToString();
        }

        private void SetDemolishTokenValue(int value, CardRarity rarity)
        {
            demolishTokenValueField.text = value.ToString();
            demolishTokenIcon.sprite = tokenSprites[(int) rarity];
        }
    }
}