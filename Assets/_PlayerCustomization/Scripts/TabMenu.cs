using System;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using _UserOperations.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _PlayerCustomization.Scripts
{
    public class TabMenu : MonoBehaviour
    {
        [SerializeField] private Transform tabsParent;
        [SerializeField] private Transform panelsParent;


        public event Action<int, int> OnSelectionChanged; 


        private readonly List<GameObject> m_Panels = new();
        private readonly Dictionary<int, List<SelectionButton>> m_SelectionButtons = new();
        private int[] m_Selections;
        private int m_SelectedPanelIndex;
        private int m_SubIndex;


        private void Awake()
        {
            LocalUser.OnUserDataLoaded += OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged += OnLocalUserDataChanged;
        }

        private void Start()
        {
            for (var i = 0; i < tabsParent.childCount; i++)
            {
                var index = i;
                var tabButton = tabsParent
                    .GetChild(i)
                    .GetComponent<Button>();

                tabButton.onClick.AddListener(() =>
                {
                    OnClickTabButton(index);
                });
            }

            for (var i = 0; i < panelsParent.childCount; i++)
            {
                var panel = panelsParent.GetChild(i);
                var selectionButtonsParent = panel
                    .GetChild(0)
                    .GetChild(0);
                
                m_Panels.Add(panel.gameObject);
                m_SelectionButtons.Add(i, new List<SelectionButton>());

                for (var j = 0; j < selectionButtonsParent.childCount; j++)
                {
                    var mainIndex = i;
                    var subIndex = j;
                    var button = selectionButtonsParent
                        .GetChild(j)
                        .GetComponent<SelectionButton>();

                    button.OnClickLocked(() =>
                    {
                        OnClickLockedSelectionButton(mainIndex, subIndex);
                    });
                    
                    button.OnClickUnlocked(() =>
                    {
                        OnClickUnlockedSelectionButton(mainIndex, subIndex);
                    });
                    
                    m_SelectionButtons[i].Add(button);
                }
            }
            
            SetSelectedPanelIndex(0);
            SetSelections(PlayerSkinner.GetSelections());
        }

        private void OnDestroy()
        {
            LocalUser.OnUserDataLoaded -= OnLocalUserDataLoaded;
            LocalUser.OnUserDataChanged -= OnLocalUserDataChanged;
        }


        public SelectionButton GetCurrentSelectionButton()
        {
            return m_SelectionButtons[m_SelectedPanelIndex][m_SubIndex];
        }


        private void OnLocalUserDataLoaded(UserData userData)
        {
            SetData(userData.skins);
        }

        private void OnLocalUserDataChanged(UserData userData)
        {
            SetData(userData.skins);
        }
        
        private void OnClickTabButton(int index)
        {
            SetSelectedPanelIndex(index);
        }

        private void OnClickLockedSelectionButton(int mainIndex, int subIndex)
        {
            m_SubIndex = subIndex;
            
            var buttons = m_SelectionButtons[mainIndex];

            for (var i = 0; i < buttons.Count; i++)
            {
                var isSelected = i == subIndex;

                if (isSelected)
                {
                    buttons[i].Select();
                    continue;
                }
                
                buttons[i].Deselect();
            }
            
            OnSelectionChanged?.Invoke(mainIndex, subIndex);
        }
        
        private void OnClickUnlockedSelectionButton(int mainIndex, int subIndex)
        {
            m_Selections[mainIndex] = subIndex;
            PlayerSkinner.SetSelections(m_Selections);
            UpdateSelections(m_Selections);
            
            OnSelectionChanged?.Invoke(mainIndex, subIndex);
        }

        private void SetSelectedPanelIndex(int index)
        {
            m_Panels[m_SelectedPanelIndex].SetActive(false);
            m_SelectedPanelIndex = index;
            m_Panels[index].SetActive(true);
        }

        private void SetData(SkinData[] data)
        {
            foreach (var (mainIndex, buttons) in m_SelectionButtons)
            {
                foreach (var button in buttons)
                {
                    button.Lock();
                }
            }
            
            foreach (var skinData in data)
            {
                var mainIndex = skinData.mainIndex;
                var subIndex = skinData.subIndex;
                var buttons = m_SelectionButtons[mainIndex];
                var button = buttons[subIndex];
                
                button.Unlock();
            }
        }

        private void SetSelections(int[] selections)
        {
            m_Selections = selections;
            UpdateSelections(selections);
        }

        private void UpdateSelections(int[] selections)
        {
            for (var i = 0; i < selections.Length; i++)
            {
                var subIndex = selections[i];
                var buttons = m_SelectionButtons[i];

                for (var j = 0; j < buttons.Count; j++)
                {
                    var isSelected = j == subIndex;

                    if (isSelected)
                    {
                        buttons[j].Select();
                        continue;
                    }
                
                    buttons[j].Deselect();
                }
            }
        }
    }
}