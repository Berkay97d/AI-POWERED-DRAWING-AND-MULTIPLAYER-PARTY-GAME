using UnityEngine;

namespace _Compatetion.Jumping.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityHandler : MonoBehaviour
    {
        [SerializeField] private float gravityScale;

        private const float GLOBAL_GRAVITY = -9.81f;

        private Rigidbody m_Rigidbody;


        private void OnEnable ()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.useGravity = false;
        }


        private void FixedUpdate ()
        {
            Vector3 gravity = GLOBAL_GRAVITY * gravityScale * Vector3.up;
            m_Rigidbody.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}