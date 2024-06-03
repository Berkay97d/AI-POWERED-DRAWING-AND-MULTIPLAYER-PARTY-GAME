using System;
using System.Linq;
using _UserOperations.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _CardPacks.Scripts
{
    public class CardPackInteractUI : MonoBehaviour
    {
        [SerializeField] private RectTransform slotsParent;
        [SerializeField] private Sprite[] backgroundSprites;
        [SerializeField] private TMP_Text titleField;
        [SerializeField] private TMP_Text coinField;
        [SerializeField] private TMP_Text tokenField;
        [SerializeField] private TMP_Text durationField;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private Button unlockButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject anotherPackUnlocking;
        [SerializeField] private GameObject lockedMain;
        [SerializeField] private GameObject unlockingMain;
        [SerializeField] private TMP_Text countdownField;
        [SerializeField] private TMP_Text[] unlockNowCostFields;
        [SerializeField] private Button[] unlockNowButtons;


        public UnityEvent onShow;
        public UnityEvent onHide;
        

        private CardPackSlotUI[] m_Slots;
        private int m_SlotIndex;


        private void Awake()
        {
            m_Slots = slotsParent.GetComponentsInChildren<CardPackSlotUI>(true);
            
            unlockButton.onClick.AddListener(() =>
            {
                Hide();
                LocalUser.TryUnlockCardPack(m_SlotIndex);
            });
            
            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });

            foreach (var unlockNowButton in unlockNowButtons)
            {
                unlockNowButton.onClick.AddListener(() =>
                {
                    Hide();
                    m_Slots[m_SlotIndex].OnUnlockedNow();
                });
            }
        }

        private void OnEnable()
        {
            var isUnlockingAnyPack = IsUnlockingAnyPack();
            unlockButton.gameObject.SetActive(!isUnlockingAnyPack);
            anotherPackUnlocking.SetActive(isUnlockingAnyPack);
        }

        private void Update()
        {
            var slot = m_Slots[m_SlotIndex];
            SetCountdown(slot.GetCountdown());
            SetUnlockNowCost(slot.GetUnlockNowCost());
        }


        public void SetSlotIndex(int index)
        {
            m_SlotIndex = index;
            background.sprite = backgroundSprites[index];
        }
        
        public void SetData(CardPackData data)
        {
            SetTitle(data.name);
            SetCoinRange(data.content.minCoin, data.content.maxCoin);
            SetTokenRange(data.content.GetMinTokenCount(), data.content.GetMaxTokenCount());
            SetDuration(data.unlockDuration.ToTimeSpan());
            SetIcon(data.icon);
        }

        public void SetLockedState()
        {
            lockedMain.SetActive(true);
            unlockingMain.SetActive(false);
        }

        public void SetUnlockingState()
        {
            lockedMain.SetActive(false);
            unlockingMain.SetActive(true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            onShow?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            onHide?.Invoke();
        }


        private void SetCountdown(TimeSpan countdown)
        {
            countdownField.text = countdown.ToCountdownString();
        }

        private void SetUnlockNowCost(int cost)
        {
            foreach (var unlockNowCostField in unlockNowCostFields)
            {
                unlockNowCostField.text = cost.ToString();
            }
        }
        
        private void SetTitle(string title)
        {
            titleField.text = title;
        }

        private void SetCoinRange(int min, int max)
        {
            coinField.text = $"{min}-{max}";
        }

        private void SetTokenRange(int min, int max)
        {
            tokenField.text = $"x{min}-{max}";
        }

        private void SetDuration(TimeSpan duration)
        {
            durationField.text = duration.ToCountdownString();
        }

        private void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        private bool IsUnlockingAnyPack()
        {
            return m_Slots.Any(slot => slot.IsUnlocking());
        }
    }
}