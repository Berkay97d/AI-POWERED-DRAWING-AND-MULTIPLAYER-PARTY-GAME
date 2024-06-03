using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace EnterAndExitUI_Competation.Scripts
{
    public class CardScrollview : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private RectTransform slotPrefab;
        [SerializeField] private RectTransform slotsParent;
        [SerializeField] private RectTransform content;


        private readonly List<RectTransform> m_Slots = new();
        private readonly List<Card> m_Cards = new();


        private void Awake()
        {
            for (var i = 0; i < gridLayoutGroup.constraintCount; i++)
            {
                AddSlot();
            }
        }


        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        public void AddCard(Card card)
        {
            if (m_Cards.Count >= m_Slots.Count)
            {
                AddSlot();
            }
            
            m_Cards.Add(card);
            card.transform.SetParent(m_Slots[m_Cards.Count - 1]);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one * 0.7f;
        }
        
        
        private void AddSlot()
        {
            var slot = Instantiate(slotPrefab, slotsParent);
            m_Slots.Add(slot);
            UpdateContentHeight();
        }

        private void UpdateContentHeight()
        {
            var sizeDelta = content.sizeDelta;
            sizeDelta.y = GetContentHeight();
            content.sizeDelta = sizeDelta;
        }

        private int GetContentHeight()
        {
            var cardPerRow = gridLayoutGroup.constraintCount;
            var isWhole = m_Slots.Count % cardPerRow == 0;
            var columnCount = (m_Slots.Count / cardPerRow);
            columnCount += isWhole ? 0 : 1;
            var spacing = (columnCount - 1) * gridLayoutGroup.spacing.y;
            var paddingTop = gridLayoutGroup.padding.top;
            var height = columnCount * gridLayoutGroup.cellSize.y;
            return Mathf.RoundToInt(spacing + paddingTop + height);
        }
    }
}