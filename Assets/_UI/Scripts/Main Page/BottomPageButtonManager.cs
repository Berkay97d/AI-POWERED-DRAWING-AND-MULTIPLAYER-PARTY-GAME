using System;
using _MenuSwipe.Scripts;
using UnityEngine;

namespace UI
{
    public class BottomPageButtonManager : MonoBehaviour
    {
        [SerializeField] private MenuSwipeManager menuSwipeManager;
        
        
        private BottomPageButton[] m_Buttons;


        private void Awake()
        {
            menuSwipeManager.OnActivePanelChanged += OnActivePanelChanged;
        }

        private void Start()
        {
            m_Buttons = GetComponentsInChildren<BottomPageButton>();

            for (var i = 0; i < m_Buttons.Length; i++)
            {
                var index = i;
                
                m_Buttons[i].AddOnClickListener(() =>
                {
                    menuSwipeManager.SnapToMenuPanel(index);
                    menuSwipeManager.SetDragable(true);
                });
            }
        }

        private void OnDestroy()
        {
            menuSwipeManager.OnActivePanelChanged -= OnActivePanelChanged;
        }


        private void OnActivePanelChanged(MenuSwipeManager.EventArgs args)
        {
            for (var i = 0; i < m_Buttons.Length; i++)
            {
                var isSelected = i == args.currentPanelIndex;

                if (isSelected)
                {
                    m_Buttons[i].SetActiveImage();
                    continue;
                }
                
                m_Buttons[i].SetPassiveImage();
            }
        }


        public void SetActiveButtons(bool value)
        {
            foreach (var button in m_Buttons)
            {
                button.SetActive(value);
            }
        }
    }
}