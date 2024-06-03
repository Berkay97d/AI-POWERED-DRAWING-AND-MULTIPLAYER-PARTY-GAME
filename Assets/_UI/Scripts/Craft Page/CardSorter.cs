using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CardSorter : MonoBehaviour
    {
        private static readonly (SortBy, SortDirection)[] SortTypes = 
        {
            (SortBy.Rarity, SortDirection.Ascending),
            (SortBy.Score, SortDirection.Ascending),
            //(SortBy.Title, SortDirection.Ascending),
            (SortBy.Rarity, SortDirection.Descending),
            (SortBy.Score, SortDirection.Descending),
            //(SortBy.Title, SortDirection.Descending),
        };
        
        
        [SerializeField] private Button sortButton;
        [SerializeField] private TMP_Text sortText;


        private ICardList m_CardList;
        private int m_SortTypeIndex;
        

        private void Awake()
        {
            m_CardList = GetComponent<ICardList>();
            
            sortButton.onClick.AddListener(OnClickSortButton);
            
            UpdateSortText();
        }


        private void OnClickSortButton()
        {
            m_SortTypeIndex = (m_SortTypeIndex + 1) % SortTypes.Length;
            UpdateSortText();
            SortCards();
        }
        
        
        public void SortCards()
        {
            var cards = m_CardList.GetCards();
            
            cards.Sort(CompareCard);
            
            for (var i = 0; i < cards.Count; i++)
            {
                cards[i].transform.SetSiblingIndex(i);
            }
        }
        
        
        private int CompareCard(Card a, Card b)
        {
            var (sortBy, sortDirection) = SortTypes[m_SortTypeIndex];

            var isEqual = sortBy switch
            {
                SortBy.Rarity => a.GetCardData().Rarity == b.GetCardData().Rarity,
                SortBy.Score => a.GetCardData().score == b.GetCardData().score,
                SortBy.Title => string.CompareOrdinal(a.GetCardData().subTitle, b.GetCardData().subTitle) == 0,
                _ => true
            };

            if (isEqual) return 0;
            
            var isGreater = sortBy switch
            {
                SortBy.Rarity => a.GetCardData().Rarity > b.GetCardData().Rarity,
                SortBy.Score => a.GetCardData().score > b.GetCardData().score,
                SortBy.Title => string.CompareOrdinal(a.GetCardData().subTitle, b.GetCardData().subTitle) > 0,
                _ => true
            };

            if (isGreater)
            {
                return sortDirection == SortDirection.Ascending ? 1 : -1;
            }

            return sortDirection == SortDirection.Ascending ? -1 : 1;
        }

        private void UpdateSortText()
        {
            var (sortBy, sortDirection) = SortTypes[m_SortTypeIndex];
            sortText.text = GetSortText(sortBy, sortDirection);
        }
        
        
        private string GetSortText(SortBy sortBy, SortDirection sortDirection)
        {
            var text = $"By {sortBy}";

            return sortDirection == SortDirection.Ascending
                ? text
                : text + " descending";
        }
        
        
        
        private enum SortBy
        {
            Rarity,
            Score,
            Title
        }
        
        private enum SortDirection
        {
            Ascending,
            Descending
        }
    }
}