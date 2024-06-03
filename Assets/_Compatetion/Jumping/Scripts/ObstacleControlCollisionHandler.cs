using _Compatetion.Jumping.Scripts.Contesters;
using UnityEngine;

namespace _Compatetion.Jumping.Scripts
{
    public class ObstacleControlCollisionHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.TryGetComponent(out ContesterCollisionHandler contesterCollisionHandler))
            {

                var aiInput = contesterCollisionHandler.GetAiInput();

                if (aiInput)
                {
                    aiInput.TryJump();
                }
            }
        }
    }
}