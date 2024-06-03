using System.IO;
using _QuestSystem.Scripts;
using _UserOperations.Scripts;
using UnityEngine;

namespace _CardPacks.Scripts
{
    public class CardPackOpener : MonoBehaviour
    {
        [SerializeField] private GameObject[] hideWhenOpening;
        [SerializeField] private Transform cardPackParent;
        [SerializeField] private CardPackTableSO cardPackTable;


        private static CardPackOpener ms_Instance;
        

        private CardPack m_CardPack;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && m_CardPack)
            {
                m_CardPack.OnClick();
            }
        }
        
        
        public static void OpenCardPack(CardPackType cardPackType)
        {
            var instance = GetInstance();

            foreach (var obj in instance.hideWhenOpening)
            {
                obj.SetActive(false);
            }
            
            instance.gameObject.SetActive(true);
            
            if (instance.m_CardPack)
            {
                Destroy(instance.m_CardPack.gameObject);
            }

            var prefab = instance.GetPrefab(cardPackType);
            instance.m_CardPack = Instantiate(prefab, instance.cardPackParent);

            instance.m_CardPack.OnCompleted += () =>
            {
                foreach (var obj in instance.hideWhenOpening)
                {
                    obj.SetActive(true);
                }
                
                ms_Instance.gameObject.SetActive(false);
                AddQuestProgress(cardPackType);
            };
        }


        private CardPack GetPrefab(CardPackType type)
        {
            return cardPackTable.GetCardPackData(type).cardPackPrefab;
        }


        private static void AddQuestProgress(CardPackType type)
        {
            var questType = type switch
            {
                CardPackType.Common => QuestType.Open_CommonCardPack,
                CardPackType.Rare => QuestType.Open_RareCardPack,
                CardPackType.Legendary => QuestType.Open_LegendaryCardPack,
                _ => throw new InvalidDataException()
            };
            
            LocalUser.AddQuestProgress(questType);
        }
        
        private static CardPackOpener GetInstance()
        {
            if (ms_Instance) return ms_Instance;

            ms_Instance = FindObjectOfType<CardPackOpener>(true);
            return ms_Instance;
        }
    }
}