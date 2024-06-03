using UnityEngine;

namespace _Compatetion.GameOfSnows.Scripts.Snowball
{
    public class SnowballForcer : MonoBehaviour
    {
        [SerializeField] private SnowballScaler snowballScaler;
        [SerializeField] private SnowballThrower snowballThrower;
        [SerializeField] private float forceSpeed;


        public void Force(Collider hitCollider)
        {
            var contesterMovement = snowballThrower.GetContesterMovement();

            if (contesterMovement != null)
            {
                Vector3 forceDirection = hitCollider.gameObject.transform.position - transform.position;
                forceDirection.y = 0f;
                
                contesterMovement.SetVelocity(Vector3.zero);
                contesterMovement.AddVelocity(forceDirection *
                                              (-1 * forceSpeed * snowballScaler.GetSnowballVisual().transform.localScale
                                                  .y));
            }
        }
    }
}
