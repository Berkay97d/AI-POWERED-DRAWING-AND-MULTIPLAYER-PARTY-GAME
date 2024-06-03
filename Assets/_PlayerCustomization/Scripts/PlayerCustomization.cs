using System.Collections.Generic;
using UnityEngine;

namespace _PlayerCustomization.Scripts
{
    public class PlayerCustomization : MonoBehaviour
    {
        [SerializeField] private CustomizationDataSO customizationData;
        [SerializeField] private TabMenu customizationMenu;
        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Transform partsRoot;


        public static readonly SkinData[] DefaultUnlockedSkins = 
        {
            new() {mainIndex = 0, subIndex = 0},
            new() {mainIndex = 1, subIndex = 0},
            new() {mainIndex = 2, subIndex = 0},
            new() {mainIndex = 3, subIndex = 0},
            new() {mainIndex = 4, subIndex = 0}
        };
        

        private readonly Dictionary<int, List<GameObject>> m_Parts = new();


        private void Awake()
        {
            CacheParts();
            LoadFromSelections(PlayerSkinner.GetSelections());
            
            customizationMenu.OnSelectionChanged += OnSelectionChanged;
        }

        private void OnDestroy()
        {
            customizationMenu.OnSelectionChanged -= OnSelectionChanged;
        }


        private void CacheParts()
        {
            for (var i = 0; i < partsRoot.childCount; i++)
            {
                var part = partsRoot.GetChild(i);
                var subParts = new List<GameObject>();

                for (var j = 0; j < part.childCount; j++)
                {
                    var subPart = part.GetChild(j).gameObject;
                    subPart.SetActive(false);
                    subParts.Add(subPart);
                }
                
                m_Parts.Add(i, subParts);
            }
        }

        private void LoadFromSelections(int[] selections)
        {
            for (var i = 0; i < selections.Length; i++)
            {
                OnSelectionChanged(i, selections[i]);
            }
        }
        

        private void OnSelectionChanged(int tabIndex, int selectionIndex)
        {
            switch (tabIndex)
            {
                case 4:
                {
                    OnColorChanged(selectionIndex);
                    break;
                }

                default:
                {
                    OnItemChanged(tabIndex, selectionIndex);
                    break;
                }
            }
        }

        private void OnItemChanged(int tabIndex, int index)
        {
            var subParts = m_Parts[tabIndex];
            
            for (var i = 0; i < subParts.Count; i++)
            {
                var isActive = i == index;
                subParts[i].SetActive(isActive);
            }
        }

        private void OnColorChanged(int index)
        {
            var bodyColors = GetBodyColors();
            bodyRenderer.material.color = bodyColors[index];
        }

        private Color[] GetBodyColors()
        {
            return customizationData.bodyColors;
        }
    }
}