using System;
using System.Linq;
using _UserOperations.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _PlayerCustomization.Scripts
{
    public class SelectionButton : MonoBehaviour
    {
        [SerializeField] private GameObject selectedVisual;
        [SerializeField] private GameObject lockedVisual;
        [SerializeField] private Image icon;


        private Action m_OnClickLocked;
        private Action m_OnClickUnlocked;
        private Button m_Button;
        private SkinData m_Data;
        private bool m_IsLocked;


        private void Awake()
        {
            m_Button = GetComponent<Button>();
            
            m_Button.onClick.AddListener(OnClick);
        }


        private void OnClick()
        {
            if (m_IsLocked)
            {
                m_OnClickLocked?.Invoke();
                return;
            }
                
            m_OnClickUnlocked?.Invoke();
        }
        

        public void OnClickLocked(Action action)
        {
            m_OnClickLocked = action;
        }

        public void OnClickUnlocked(Action action)
        {
            m_OnClickUnlocked = action;
        }

        public void Select()
        {
            selectedVisual.SetActive(true);
        }

        public void Deselect()
        {
            selectedVisual.SetActive(false);
        }

        public void Lock()
        {
            m_IsLocked = true;
            lockedVisual.SetActive(true);
        }

        public void Unlock()
        {
            m_IsLocked = false;
            lockedVisual.SetActive(false);
        }

        public void SetSkinData(SkinData data)
        {
            m_Data = data;
        }

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetIconColor(Color color)
        {
            icon.color = color;
        }
    }
}