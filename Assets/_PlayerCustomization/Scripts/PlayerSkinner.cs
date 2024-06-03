using System;
using System.Collections.Generic;
using _DatabaseAPI.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _PlayerCustomization.Scripts
{
    public class PlayerSkinner : MonoBehaviour
    {
        private const string PlayerSelectionKey = "Player_Skin_Selection";


        [SerializeField] private CustomizationDataSO customizationData;
        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Transform partsRoot;
        [SerializeField] private bool randomizeOnAwake;

        public static Action<int[]> OnSelectionsChanged;


        private readonly Dictionary<int, List<GameObject>> m_Parts = new();
        

        private void Awake()
        {
            OnSelectionsChanged += OnSelectionsChanged_Internal;
            
            CacheParts();

            var selections = randomizeOnAwake
                ? GetRandomSelections()
                : GetSelections();
            
            LoadFromSelections(selections);
        }

        private void OnDestroy()
        {
            OnSelectionsChanged -= OnSelectionsChanged_Internal;
        }


        private void OnSelectionsChanged_Internal(int[] selections)
        {
            LoadFromSelections(selections);
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
            for (var i = 0; i < selections.Length - 1; i++)
            {
                var subParts = m_Parts[i];
            
                for (var j = 0; j < subParts.Count; j++)
                {
                    var isActive = j == selections[i];
                    subParts[j].SetActive(isActive);
                }
            }

            var bodyColors = GetBodyColors();
            
            bodyRenderer.material.color = bodyColors[selections[^1]];
        }

        private int[] GetRandomSelections()
        {
            var selections = new int[5];
            selections[0] = Random.Range(0, customizationData.headSprites.Length + 1);
            selections[1] = Random.Range(0, customizationData.upperBodySprites.Length + 1);
            selections[2] = Random.Range(0, customizationData.lowerBodySprites.Length + 1);
            selections[3] = Random.Range(0, customizationData.footSprites.Length + 1);
            selections[4] = Random.Range(0, customizationData.bodyColors.Length);
            return selections;
        }

        private Color[] GetBodyColors()
        {
            return customizationData.bodyColors;
        }


        public static int[] GetSelections()
        {
            const string none = "NONE";

            var selections = PlayerPrefs.GetString(PlayerSelectionKey, none);

            return selections == none
                ? new int[5]
                : JsonUtils.ToArrayOf<int>(selections, false);
        }
        
        public static void SetSelections(int[] selections)
        {
            PlayerPrefs.SetString(PlayerSelectionKey, JsonUtils.ToJson(selections));
            OnSelectionsChanged?.Invoke(selections);
        }
    }
}