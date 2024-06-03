using System;
using _DatabaseAPI.Scripts;
using _TimeAPI.Scripts;
using _UserOperations.Scripts;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _CardPacks.Scripts
{
    public class CardPackSlotUI : MonoBehaviour
    {
        [SerializeField] private CardPackInteractUI interactUI;
        [SerializeField] private CardPackTableSO cardPackTable;
        [SerializeField] private RectTransform visualParent;
        [SerializeField] private GameObject lockedMain;
        [SerializeField] private GameObject countdownMain;
        [SerializeField] private GameObject unlockNowMain;
        [SerializeField] private GameObject unlockedMain;
        [SerializeField] private TMP_Text countdownField;
        [SerializeField] private TMP_Text unlockNowCostField;
        [SerializeField] private TMP_Text unlockDurationField;
        [SerializeField] private Image background;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite unlockedSprite;
        [SerializeField] private Sprite unlockingSprite;
        [SerializeField] private Sprite lockedSprite;


        private CardPackSlotData m_Data;
        private Coroutine m_Routine;
        private Button m_Button;
        private Canvas m_Canvas;
        private Action m_OnClick;
        private TimeSpan m_Countdown;
        private int m_UnlockNowCost;
        private bool m_IsUnlocking;


        private void Awake()
        {
            m_Canvas = GetComponentInParent<Canvas>();
            m_Button = GetComponent<Button>();
            
            SetEmptyVisual();

            LocalUser.OnUserDataLoaded += OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged += OnLocalUserDataChanged;
            
            m_Button.onClick.AddListener(() => m_OnClick?.Invoke());
            
            unlockedMain.SetActive(false);
        }

        private void OnDestroy()
        {
            LocalUser.OnUserDataLoaded -= OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged -= OnLocalUserDataChanged;
            
            LazyCoroutines.StopCoroutine(m_Routine);
        }


        private void OnLocalUserDataLoaded(UserData userData)
        {
            ApplyData(userData.cardPackSlots[GetIndex()]);
                
            if (m_Data.IsEmpty()) return;
                
            SetUnlockDuration(GetUnlockDuration(m_Data.cardPackType));
                
            Tick();
            m_Routine = LazyCoroutines.EverySeconds(() => 1f, Tick);
        }
        
        private void OnLocalUserDataChanged(UserData userData)
        {
            m_Data = userData.cardPackSlots[GetIndex()];
        }

        private void Tick()
        {
            m_IsUnlocking = m_Data.IsStartedUnlocking();

            if (m_IsUnlocking)
            {
                SetUnlockingVisual();
            }

            else
            {
                SetLockedVisual();
            }

            m_OnClick = m_IsUnlocking ? OnClickUnlocking : OnClickLocked;
                
            if (m_IsUnlocking)
            {
                TimeAPI.GetCurrentTime(timeData =>
                {
                    var elapsedTime = timeData - m_Data.startTime;
                    var unlockDuration = GetUnlockDuration(m_Data.cardPackType);
                    var timeLeft = unlockDuration - elapsedTime;
                    var unlockNowCost = CalculateUnlockNowCost(timeLeft);
                        
                    SetCountdown(timeLeft);
                    SetUnlockNowCost(unlockNowCost);

                    if (timeLeft.TotalMilliseconds < 0d)
                    {
                        LazyCoroutines.StopCoroutine(m_Routine);
                                
                        SetUnlockedVisual();

                        m_OnClick = OnClickUnlocked;
                        m_IsUnlocking = false;
                    }
                });
            }
        }


        public bool IsUnlocking()
        {
            return m_IsUnlocking;
        }

        public void OnUnlockedNow()
        {
            var cost = m_UnlockNowCost;

            LocalUser.TryRemoveGem(cost, () =>
            { 
                OnClickUnlocked();
            });
        }
        

        private void OnClickLocked()
        {
            interactUI.SetSlotIndex(GetIndex());
            interactUI.SetData(GetData(m_Data.cardPackType));
            interactUI.SetLockedState();
            interactUI.Show();
        }

        private void OnClickUnlocking()
        {
            interactUI.SetSlotIndex(GetIndex());
            interactUI.SetData(GetData(m_Data.cardPackType));
            interactUI.SetUnlockingState();
            interactUI.Show();
        }

        private void OnClickUnlocked()
        {
            LocalUser.TryRemoveCardPackFromSlot(GetIndex(), () =>
            {
                CardPackOpener.OpenCardPack(m_Data.cardPackType);
                CleanUp();
            });
        }


        private void CleanUp()
        {
            LazyCoroutines.StopCoroutine(m_Routine);
            
            m_Data = CardPackSlotData.Default;
            m_Countdown = TimeSpan.Zero;
            m_UnlockNowCost = 0;
            m_IsUnlocking = false;
            m_OnClick = null;
            
            Destroy(visualParent.GetChild(0).gameObject);
            SetEmptyVisual();
        }
        
        private void ApplyData(CardPackSlotData data)
        {
            m_Data = data;

            if (data.IsEmpty())
            {
                SetEmptyVisual();
                return;
            }

            m_Button.enabled = true;
            CreateCardPackVisual(data.cardPackType);

            if (!data.IsStartedUnlocking())
            {
                var unlockDuration = GetUnlockDuration(m_Data.cardPackType);
                var unlockNowCost = CalculateUnlockNowCost(unlockDuration);
                SetUnlockNowCost(unlockNowCost);
            }
        }

        private void SetEmptyVisual()
        {
            m_Button.enabled = false;
            lockedMain.SetActive(false);
            countdownMain.SetActive(false);
            unlockNowMain.SetActive(false);
            unlockedMain.SetActive(false);
            background.sprite = emptySprite;
        }

        private void SetLockedVisual()
        {
            lockedMain.SetActive(true);
            countdownMain.SetActive(false);
            unlockNowMain.SetActive(false);
            unlockedMain.SetActive(false);
            background.sprite = lockedSprite;
        }

        private void SetUnlockingVisual()
        {
            lockedMain.SetActive(false);
            countdownMain.SetActive(true);
            unlockNowMain.SetActive(true);
            unlockedMain.SetActive(false);
            background.sprite = unlockingSprite;
        }

        private void SetUnlockedVisual()
        {
            lockedMain.SetActive(false);
            countdownMain.SetActive(false);
            unlockNowMain.SetActive(false);
            unlockedMain.SetActive(true);
            background.sprite = unlockedSprite;
        }

        private void SetUnlockDuration(TimeSpan duration)
        {
            unlockDurationField.text = duration.ToCountdownString();
        }

        public TimeSpan GetCountdown()
        {
            return m_Countdown;
        }
        
        private void SetCountdown(TimeSpan countdown)
        {
            m_Countdown = countdown;
            countdownField.text = countdown.ToCountdownString();
        }

        public int GetUnlockNowCost()
        {
            return m_UnlockNowCost;
        }
        
        private void SetUnlockNowCost(int cost)
        {
            m_UnlockNowCost = cost;
            unlockNowCostField.text = cost.ToString();
        }

        private int GetIndex()
        {
            return transform.GetSiblingIndex();
        }

        private void CreateCardPackVisual(CardPackType type)
        {
            var prefab = GetVisualPrefab(type);
            var visual = Instantiate(prefab, visualParent);
            
            visual.transform.localScale = GetVisualLocalScale();
        }

        private Vector3 GetVisualLocalScale()
        {
            var canvasScale = m_Canvas.transform.lossyScale;
            return new Vector3(1f / canvasScale.x, 1f / canvasScale.y, 1f / canvasScale.z);
        }

        private CardPackData GetData(CardPackType type)
        {
            return cardPackTable.GetCardPackData(type);
        }

        private GameObject GetVisualPrefab(CardPackType type)
        {
            return GetData(type).visualPrefab;
        }

        private TimeSpan GetUnlockDuration(CardPackType type)
        {
            return GetData(type).unlockDuration.ToTimeSpan();
        }
        
        
        private static int CalculateUnlockNowCost(TimeSpan countdown)
        {
            return Mathf.Max(0, Mathf.CeilToInt((float) (countdown.TotalMinutes / 10f)));
        }
    }
}