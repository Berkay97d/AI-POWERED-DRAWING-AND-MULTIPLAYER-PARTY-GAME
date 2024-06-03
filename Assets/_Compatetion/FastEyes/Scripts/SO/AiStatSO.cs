using UnityEngine;
using NaughtyAttributes;


namespace FastEyes
{
    [CreateAssetMenu(fileName = "STAT", menuName = "ScriptableObjects/FastEyesAI")]
    public class AiStatSO : ScriptableObject
    {
        [SerializeField] [Foldout("AI Level Stats") ]private float lowAIMin;
        [SerializeField] [Foldout("AI Level Stats") ]private float lowAIMax;
        [SerializeField] [Foldout("AI Level Stats") ]private float lowMidAIMin;
        [SerializeField] [Foldout("AI Level Stats") ]private float lowMidAIMax;
        [SerializeField] [Foldout("AI Level Stats") ]private float midAIMin;
        [SerializeField] [Foldout("AI Level Stats") ]private float midAIMax;
        [SerializeField] [Foldout("AI Level Stats") ]private float midHighAiMin;
        [SerializeField] [Foldout("AI Level Stats") ]private float midHighAiMax;
        [SerializeField] [Foldout("AI Level Stats") ]private float highAiMin;
        [SerializeField] [Foldout("AI Level Stats") ]private float highAiMax;

        [SerializeField] [Foldout("AI Level Stats")] private int lowSuccessRate;
        [SerializeField] [Foldout("AI Level Stats")] private int lowMidSuccessRate;
        [SerializeField] [Foldout("AI Level Stats")] private int midSuccessRate;
        [SerializeField] [Foldout("AI Level Stats")] private int midHighSuccessRate;
        [SerializeField] [Foldout("AI Level Stats")] private int highSuccessRate;
        
        public float GetRandomReactionTime(AILevel aıLevel)
        {
            switch (aıLevel)
            {
                case AILevel.Low:
                    return Random.Range(lowAIMin, lowAIMax);
                case AILevel.LowMid:
                    return Random.Range(lowMidAIMin, lowMidAIMax);
                case AILevel.Mid:
                    return Random.Range(midAIMin, midAIMax);
                case AILevel.MidHigh:
                    return Random.Range(midHighAiMin, midHighAiMax);
                case AILevel.High:
                    return Random.Range(highAiMin, highAiMax);
                    
            }

            return 0;
        }

        public bool GetSuccess(AILevel aıLevel)
        {
            int randomNum = Random.Range(0, 100);
            
            switch (aıLevel)
            {
                case AILevel.Low:
                    return randomNum < lowSuccessRate;
                case AILevel.LowMid:
                    return randomNum < lowMidSuccessRate;
                case AILevel.Mid:
                    return randomNum < midSuccessRate;
                case AILevel.MidHigh:
                    return randomNum < midHighSuccessRate;
                case AILevel.High:
                    return randomNum < highSuccessRate;
                    
            }

            return false;
        }
    }
}