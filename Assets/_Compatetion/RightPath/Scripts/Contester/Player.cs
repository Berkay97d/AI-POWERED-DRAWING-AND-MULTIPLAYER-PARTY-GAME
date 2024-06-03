using General;
using UnityEngine;

namespace RightPath
{
    public class Player : Contester
    {
        [SerializeField] private FloatingJoystick joystick;
        [SerializeField] private float speed;
        [SerializeField] private Transform feetTransform;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float dropSpeed;
        [SerializeField] private CapsuleCollider playerCollider;
        [SerializeField] private Transform respawnPos;
        
        
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private  void Update()
        {
            Debug.Log(IsGrounded());
            
            if (!IsGrounded())
            {
                rb.velocity = new Vector3(0, -1 * dropSpeed, 0);
                return;
            }
            
            if (-1 * GetMoveDirection() == Vector3.zero)
            {
                rb.velocity = Vector3.zero;
                return;
            }
            
            transform.forward = GetMoveDirection();
            
            Move();
        }

        private void Move()
        {
            var moveVector = GetMoveDirection() * speed;
            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
        }

        private Vector3 GetMoveDirection()
        {
            var dir = new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized;
            return dir;
        }

        private bool IsGrounded()
        {
            var isGrounded = Physics.CheckSphere(feetTransform.position, 0.1f, groundLayer);

            if (!isGrounded && playerCollider.enabled && transform.position.y < 0)
            {
                playerCollider.enabled = false;
                LazyCoroutines.WaitForSeconds(1f, RespawnPlayer);
            }
            
            return isGrounded;
        }

        private void RespawnPlayer()
        {
            playerCollider.enabled = true;
            transform.position = respawnPos.position;

        }

        
     

        
    }
}