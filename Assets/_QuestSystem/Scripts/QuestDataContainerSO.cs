using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using _Market.Scripts;
using UnityEngine;

namespace _QuestSystem.Scripts
{
    [CreateAssetMenu]
    public class QuestDataContainerSO : ScriptableObject
    {
        private const string QuestDataContainerPath = "QuestDataContainer";
        
        
        [SerializeField] private QuestData[] quests;


        private static QuestDataContainerSO ms_Instance;
        

        public IReadOnlyList<QuestData> GetQuests()
        {
            return quests;
        }

        public int GetQuestCount()
        {
            return quests.Length;
        }

        public IReadOnlyList<int> GetQuestIndicesWithType(QuestType type)
        {
            var indices = new List<int>();
            
            for (var i = 0; i < quests.Length; i++)
            {
                if (quests[i].type != type) continue;
                
                indices.Add(i);
            }

            return indices;
        }


        public static QuestDataContainerSO GetInstance()
        {
            if (ms_Instance) return ms_Instance;
            
            ms_Instance = Resources.Load<QuestDataContainerSO>(QuestDataContainerPath);
            return ms_Instance;
        }
    }

    
    [Serializable]
    public struct QuestData
    {
        public string explanation;
        public QuestType type;
        public int actionCount;
        public int rewardCount;
        public CurrencyType rewardCurrency;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum QuestType
    {
        Play_Competition,
        Play_SnowButt,
        Play_DontFall,
        Play_Stack,
        Play_FastEye,
        Play_Generation,

        Win_Competition,
        Win_Generation,
        Win_SnowButt,
        Win_DontFall,
        Win_Stack,
        Win_FastEye,

        Win_SnowButt_WithoutFall,
        Win_DontFall_WithoutFall,
        Win_FastEye_WithoutMiss,
        Win_Stack_WithSingleRow,
        Win_MemoryMatch_WithMaxScore,
        Win_EightPiece_UnderThirtySeconds,
        
        Open_CommonCardPack,
        Open_RareCardPack,
        Open_LegendaryCardPack,
        
        Buy_Skin,
        
        Place_CardToCollection,
        
        Play_MemoryMatch,
        Play_SwapMatch,
        Win_MemoryMatch,
        Win_SwapMatch,
        
        Play_Katana
    }
}