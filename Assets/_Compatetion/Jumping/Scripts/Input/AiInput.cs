using System;
using _Compatetion.Jumping.Scripts.Contesters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Compatetion.Jumping.Scripts.Input
{
    public class AiInput : MonoBehaviour, IInput
    {
        [SerializeField] private Contester contester;
        [SerializeField] private AiLevelType aiLevelType;
        
        public event Action OnJump;
        

        public void TryJump()
        {
            // if (!contester.GetContesterCollisionHandler().GetIsTouchObstacleControl()) return;
            
            var ratio = Random.Range(0f, 100f);
            switch (aiLevelType)
            {
                case AiLevelType.Easy:
                {
                    if (ratio < 75f)
                    {
                        OnJump?.Invoke();
                    }
                }
                    break;
                case AiLevelType.Medium:
                {
                    if (ratio < 85f)
                    {
                        OnJump?.Invoke();
                    }
                }
                    break;
                case AiLevelType.Hard:
                {
                    if (ratio < 95f)
                    {
                        OnJump?.Invoke();
                    }
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void SetAiLevel(AiLevelType type)
        {
            aiLevelType = type;
        }
    }
}