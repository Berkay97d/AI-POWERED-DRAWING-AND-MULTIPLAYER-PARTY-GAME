using System;
using _Compatetion.GameOfSnows.Scripts.Snowball;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Ozer.Scripts
{
    public class ContesterMovement : MonoBehaviour
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deAcceleration;
        [SerializeField] private float gravity;
        [SerializeField] private SnowballInitilazor snowballInitilazor;
        [SerializeField] private ContesterCollision contesterCollision;
        
        private SnowballCollision m_SnowballCollision;
        private InputProvider m_InputProvider;
        private Vector3 m_Velocity;
        private bool m_CanMove = true;
        
        Vector3 direction;

        private void Awake()
        {
            m_InputProvider = GetComponent<InputProvider>();
        }

        
        private void Start()
        {
            snowballInitilazor.OnSnowballInit += OnSnowballInit;
        }

        
        private void OnSnowballInit(object sender, SnowballCollision e)
        {
            m_SnowballCollision = e;
        }
        

        private void Update()
        {
            if (!m_CanMove) return;
            Move();
            
            var position = transform.position;
            position = new Vector3(position.x , 0f, position.z);
            transform.position = position;
        }

        
        private void Move()
        {
            var horizontal = m_InputProvider.GetHorizontal();
            var vertical = m_InputProvider.GetVertical();

            direction = new Vector3(horizontal, 0f, vertical);

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-direction);
            }
            
            var targetVelocity = direction * maxSpeed;
            var isSlowing = targetVelocity.magnitude <= 0.01f;
            var t = Time.deltaTime * (isSlowing ? deAcceleration : acceleration);
            
            m_Velocity.x = Mathf.Lerp(m_Velocity.x, targetVelocity.x, t);
            m_Velocity.z = Mathf.Lerp(m_Velocity.z, targetVelocity.z, t);

            var deltaPosition = m_Velocity * Time.deltaTime;

            if (!m_SnowballCollision)
            {
                MoveDown(deltaPosition);
                transform.position += deltaPosition;

                return;
            }
            
            if (m_SnowballCollision.IsGrounded())
            {
                m_Velocity.y = 0f;
                transform.position += deltaPosition;
            }
            else
            {
                MoveDown(deltaPosition);
                transform.position += deltaPosition;

            }
        }

        private void MoveDown(Vector3 deltaPos)
        {
            m_Velocity += Vector3.down * (gravity * Time.deltaTime);
            contesterCollision.transform.position += deltaPos;
        }


        public Vector3 GetVelocity()
        {
            return m_Velocity;
        }

        public Vector3 GetDirection()
        {
            return direction;
        }

        private void OnDestroy()
        {
            snowballInitilazor.OnSnowballInit -= OnSnowballInit;
        }


        public void SetSnowballCollision(SnowballCollision snowballCollision)
        {
            m_SnowballCollision = snowballCollision;
        }


        public ContesterCollision GetContesterCollision()
        {
            return contesterCollision;
        }


        public InputProvider GetInputProvider()
        {
            return m_InputProvider;
        }


        public void AddVelocity(Vector3 velocity)
        {
            m_Velocity += velocity;
        }


        public void SetVelocity(Vector3 velocity)
        {
            m_Velocity = velocity;
        }


        public Vector3 GetPosition()
        {
            return transform.position;
        }


        public void SetCanMove(bool canMove)
        {
            m_CanMove = canMove;
        }


        public SnowballCollision GetSnowballCollision()
        {
            return m_SnowballCollision;
        }
    }
}