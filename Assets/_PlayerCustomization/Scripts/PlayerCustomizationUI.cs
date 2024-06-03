using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _PlayerCustomization.Scripts
{
    public class PlayerCustomizationUI : MonoBehaviour
    {
        [SerializeField] private CustomizationDataSO customizationData;
        [SerializeField] private TabMenu customizationMenu;
        [SerializeField] private Button unlockButton;


        private readonly Dictionary<SkinData, SelectionButton> m_SelectionButtons = new();


        private void Awake()
        {
            var buttons = GetComponentsInChildren<SelectionButton>(true);

            foreach (var button in buttons)
            {
                var mainIndex = button.transform.parent.parent.parent.GetSiblingIndex();
                var subIndex = button.transform.GetSiblingIndex();
                var data = new SkinData {mainIndex = mainIndex, subIndex = subIndex};
                
                button.Lock();
                button.SetSkinData(data);
                m_SelectionButtons.Add(data, button);
            }
            
            LoadSelectionButtons();
        }

        private void Start()
        {
            unlockButton.onClick.AddListener(() =>
            {
                var button = customizationMenu.GetCurrentSelectionButton();
                button.Unlock();
            });
        }

        private void LoadSelectionButtons()
        {
            var headSprites = GetHeadSprites();
            
            for (var i = 0; i < headSprites.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 0, subIndex = i + 1};
                m_SelectionButtons[skinData].SetIcon(headSprites[i]);
            }

            var upperBodySprites = GetUpperBodySprites();

            for (var i = 0; i < upperBodySprites.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 1, subIndex = i + 1};
                m_SelectionButtons[skinData].SetIcon(upperBodySprites[i]);
            }

            var lowerBodySprites = GetLowerBodySprites();
            
            for (var i = 0; i < lowerBodySprites.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 2, subIndex = i + 1};
                m_SelectionButtons[skinData].SetIcon(lowerBodySprites[i]);
            }

            var footSprites = GetFootSprites();
            
            for (var i = 0; i < footSprites.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 3, subIndex = i + 1};
                m_SelectionButtons[skinData].SetIcon(footSprites[i]);
            }

            var bodyColors = GetBodyColors();
            
            for (var i = 0; i < upperBodySprites.Length; i++)
            {
                var skinData = new SkinData {mainIndex = 4, subIndex = i};
                m_SelectionButtons[skinData].SetIconColor(bodyColors[i]);
            }
        }

        private Sprite[] GetHeadSprites()
        {
            return customizationData.headSprites;
        }

        private Sprite[] GetUpperBodySprites()
        {
            return customizationData.upperBodySprites;
        }

        private Sprite[] GetLowerBodySprites()
        {
            return customizationData.lowerBodySprites;
        }

        private Sprite[] GetFootSprites()
        {
            return customizationData.footSprites;
        }

        private Color[] GetBodyColors()
        {
            return customizationData.bodyColors;
        }
    }
}