using System;
using System.Collections.Generic;
using System.Linq;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _QuestSystem.Scripts
{
    public class QuestPanelUI : MonoBehaviour
    {
        [SerializeField] private QuestDataContainerSO questDataContainer;
        [SerializeField] private QuestButtonUI questButtonPrefab;
        [SerializeField] private RectTransform questButtonsParent;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject main;
        [SerializeField] private Transform panel;
        [SerializeField] private Image background;
        [SerializeField] private float toggleDuration = 0.25f;


        public event Action<int> OnNotificationCountChanged;


        private List<QuestButtonUI> m_SortedQuestButtons;
        private QuestButtonUI[] m_QuestButtons;
        private QuestButtonUIComparer m_ButtonComparer;


        private void Awake()
        {
            m_ButtonComparer = new QuestButtonUIComparer();
            
            closeButton.onClick.AddListener(OnClickCloseButton);
            
            LocalUser.OnUserDataLoaded += OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged += OnLocalUserDataChanged;

            LoadFromDataContainer(questDataContainer);
        }

        private void OnDestroy()
        {
            LocalUser.OnUserDataLoaded -= OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged -= OnLocalUserDataChanged;
        }


        private void OnLocalUserDataLoaded(UserData userData)
        {
            LoadFromDatabase(userData.quests);
        }

        private void OnLocalUserDataChanged(UserData userData)
        {
            LoadFromDatabase(userData.quests);
        }

        private void OnClickCloseButton()
        {
            Close();
        }


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Open()
        {
            main.SetActive(true);
            
            background.DOColor(new Color(0f, 0f, 0f, 0.8f), toggleDuration)
                .SetEase(Ease.OutSine);

            panel.DOScale(Vector3.one, toggleDuration)
                .SetEase(Ease.OutBack);
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Close()
        {
            background.DOColor(new Color(0f, 0f, 0f, 0f), toggleDuration)
                .SetEase(Ease.InSine);

            panel.DOScale(Vector3.zero, toggleDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    main.SetActive(false);
                });
        }
        

        private void LoadFromDatabase(QuestDatabaseData[] databaseData)
        {
            for (var i = 0; i < databaseData.Length; i++)
            {
                m_QuestButtons[i].SetDatabaseData(databaseData[i]);
            }
            
            SortButtons();
            
            OnNotificationCountChanged?.Invoke(GetNotificationCount());
        }
        
        private void LoadFromDataContainer(QuestDataContainerSO dataContainer)
        {
            var quests = dataContainer.GetQuests();

            m_QuestButtons = new QuestButtonUI[quests.Count];

            for (var i = 0; i < m_QuestButtons.Length; i++)
            {
                var questButton = CreateQuestButton();
                
                questButton.SetIndex(i);
                questButton.SetData(quests[i]);
                m_QuestButtons[i] = questButton;
            }

            m_SortedQuestButtons = m_QuestButtons.ToList();

            var sizeDelta = questButtonsParent.sizeDelta;
            sizeDelta.y = GetContentHeight();
            questButtonsParent.sizeDelta = sizeDelta;
        }

        private void SortButtons()
        {
            m_SortedQuestButtons.Sort(m_ButtonComparer);

            for (var i = 0; i < m_SortedQuestButtons.Count; i++)
            {
                m_SortedQuestButtons[i].SetSiblingIndex(i);
            }
        }

        private QuestButtonUI CreateQuestButton()
        {
            var questButton = Instantiate(questButtonPrefab, questButtonsParent);
            
            return questButton;
        }

        private float GetContentHeight()
        {
            return m_QuestButtons.Length * questButtonPrefab.GetSizeDelta().y;
        }

        private int GetNotificationCount()
        {
            var count = 0;
            
            foreach (var questButton in m_QuestButtons)
            {
                if (questButton.GetState() is QuestButtonUI.State.ReadyToClaim)
                {
                    count += 1;
                }
            }

            return count;
        }
    }
}