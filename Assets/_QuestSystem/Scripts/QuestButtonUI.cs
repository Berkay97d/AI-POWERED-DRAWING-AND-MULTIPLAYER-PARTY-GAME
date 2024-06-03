using _Market.Scripts;
using _UserOperations.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _QuestSystem.Scripts
{
    public class QuestButtonUI : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private GameObject[] panels;
        [SerializeField] private QuestRewardUI[] rewards;
        [SerializeField] private TMP_Text explanationField;
        [SerializeField] private TMP_Text rateField;
        [SerializeField] private Image progressFill;
        [SerializeField] private Button claimButton;
        [SerializeField] private CanvasGroup canvasGroup;


        private QuestDatabaseData m_DatabaseData;
        private QuestData m_Data;
        private State m_State;
        private int m_Index;
        

        private void Awake()
        {
            claimButton.onClick.AddListener(OnClickClaimButton);
        }


        private void OnClickClaimButton()
        {
            LocalUser.ClaimQuestPrize(GetIndex());
        }
        

        public void SetData(QuestData data)
        {
            m_Data = data;
            
            SetReward(data.rewardCount, data.rewardCurrency);
            SetExplanation(data.explanation);
        }

        public void SetDatabaseData(QuestDatabaseData data)
        {
            m_DatabaseData = data;

            var progress = CalculateProgress();
            SetProgress(progress);
            SetRate(m_DatabaseData.count, m_Data.actionCount);

            var state = State.InProgress;

            if (m_DatabaseData.isClaimed)
            {
                state = State.Claimed;
            }

            if (progress >= 1f && !m_DatabaseData.isClaimed)
            {
                state = State.ReadyToClaim;
            }
            
            SetState(state);
        }

        public Vector2 GetSizeDelta()
        {
            return rectTransform.sizeDelta;
        }

        public int GetIndex()
        {
            return m_Index;
        }

        public void SetIndex(int index)
        {
            m_Index = index;
        }

        public void SetSiblingIndex(int siblingIndex)
        {
            transform.SetSiblingIndex(siblingIndex);
        }

        public int GetPriority()
        {
            var priority = -GetIndex();

            if (m_DatabaseData.isClaimed)
            {
                priority -= 100000;
            }

            if (CalculateProgress() >= 1f)
            {
                priority += 10000;
            }

            return priority;
        }

        public State GetState()
        {
            return m_State;
        }


        private void SetState(State state)
        {
            m_State = state;

            panels[0].SetActive(state is State.InProgress or State.Claimed);
            panels[1].SetActive(state is State.ReadyToClaim);
            panels[2].SetActive(state is State.Claimed);

            canvasGroup.alpha = state == State.Claimed ? 0.5f : 1f;
        }
        
        private void SetReward(int count, CurrencyType currencyType)
        {
            foreach (var reward in rewards)
            {
                reward.SetReward(count, currencyType);
            }
        }

        private void SetExplanation(string explanation)
        {
            explanationField.text = explanation;
        }

        private void SetRate(int x, int y)
        {
            rateField.text = $"{x}/{y}";
        }

        private float CalculateProgress()
        {
            return Mathf.InverseLerp(0, m_Data.actionCount, m_DatabaseData.count);
        }

        private void SetProgress(float value)
        {
            progressFill.fillAmount = value;
        }


        public enum State
        {
            InProgress,
            ReadyToClaim,
            Claimed
        }
    }
}