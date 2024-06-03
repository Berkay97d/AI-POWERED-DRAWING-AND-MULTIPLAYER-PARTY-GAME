using System.Collections;
using _Compatetion.Jumping.Scripts.Input;
using _SoundSystem.Scripts;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace _Compatetion.Jumping.Scripts.Contesters
{
    public class Contester : MonoBehaviour
    {
        [SerializeField] private ContesterCollisionHandler contesterCollisionHandler;
        [SerializeField] private GameObject inputGameObject;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpCooldown = 0.2f;
        [SerializeField] private Image deadIcon;
        [SerializeField] private Image kingIcon;
        [SerializeField] private TMP_Text pointText;
        [SerializeField] private Animator animator;
        [SerializeField] private ContesterType type;
        [SerializeField] private ParticleSystem deadParticle;

        private Rigidbody m_Rigidbody;
        private IInput m_Input;
        private bool m_IsDead;
        private bool m_CanJump = true;
        private int m_Point;
        private int m_Index;


        protected virtual void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Input = inputGameObject.GetComponent<IInput>();
        }


        private void Start()
        {
            m_Input.OnJump += OnJump;

            pointText.text = 0.ToString();
        }


        public int GetIndex()
        {
            return m_Index;
        }

        public void SetIndex(int index)
        {
            m_Index = index;
        }


#if UNITY_EDITOR
      
        private void OnValidate()
        {
            if (inputGameObject && !inputGameObject.TryGetComponent(out IInput _))
            {
                Undo.RecordObject(this, $"{name} values changed!");
                
                inputGameObject = null;
            }
        }
        
#endif

        private void Update()
        {
            if (CanDie())
            {
                Die();
            }
        }


        private void OnJump()
        {
            if (m_CanJump && !m_IsDead)
            {
                Jump();
                StartCoroutine(EnableJumpAfterCooldown());
            }
        }


        private void OnDestroy()
        {
            m_Input.OnJump -= OnJump;
        }


        private IEnumerator EnableJumpAfterCooldown()
        {
            m_CanJump = false;
            yield return new WaitForSeconds(jumpCooldown);
            m_CanJump = true;
        }
        

        private void Jump()
        {
            if (!contesterCollisionHandler.GetIsTouchGround()) return;
            m_Rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2.5f * Physics.gravity.y),
                ForceMode.VelocityChange);
            
            animator.SetTrigger("Jump");

            if (!m_IsDead)
            {
                SoundManager.Instance.PlayLaserJump();
            }
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            gameObject.SetActive(false);

            m_IsDead = true;
            deadIcon.gameObject.SetActive(true);
            
            GameManager.Instance.AddPoint(1);
            m_Point = GameManager.Instance.GetPoint();

            pointText.text = GameManager.Instance.GetPoint().ToString();
            
            GameManager.Instance.TryGameOver();

            SoundManager.Instance.PlayLaserLaserDead();
            var position = transform.position;
            Instantiate(deadParticle, new Vector3(position.x, position.y + 1.75f, position.z), Quaternion.identity);

            if (type == ContesterType.Player)
            {
                foreach (var contester in GameManager.Instance.GetContesterArray())
                {
                    if (contester.type == ContesterType.Ai)
                    {
                        contester.GetComponent<AiInput>().SetAiLevel(AiLevelType.Easy);
                    }
                }
            }
        }


        private bool CanDie()
        {
            return contesterCollisionHandler.GetIsTouchObstacle() && !m_IsDead;
        }
        

        public ContesterCollisionHandler GetContesterCollisionHandler()
        {
            return contesterCollisionHandler;
        }


        public bool GetIsDead()
        {
            return m_IsDead;
        }


        public void SetText(string text)
        {
            pointText.text = text;
        }


        public void SetActiveKingIcon(bool isActive)
        {
            kingIcon.gameObject.SetActive(isActive);
        }


        public int GetPoint()
        {
            return m_Point;
        }


        public ContesterType GetContesterType()
        {
            return type;
        }


        public void SetPoint(int point)
        {
            m_Point = point;
        }
    }
}
