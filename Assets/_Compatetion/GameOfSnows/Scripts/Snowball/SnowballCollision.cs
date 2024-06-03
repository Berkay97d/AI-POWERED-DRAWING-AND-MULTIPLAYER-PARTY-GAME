using System;
using _Compatetion.GameOfSnows.Ozer.Scripts;
using _SoundSystem.Scripts;
using General;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Scripts.Snowball
{
    public class SnowballCollision : MonoBehaviour
    {
        [SerializeField] private LayerMask ground;
        [SerializeField] private GameObject boooomParticle;
        [SerializeField] private Transform particleTransfromParent;
        [SerializeField] private float smallSpeed;

        public event EventHandler OnSnowballDestroyed;
        
        private Vector3 m_LocalScale;
        private ContesterMovement m_ContesterMovement;
        private SnowballForcer m_SnowballForcer;
        private bool canPlaySound = true;


        private void Awake()
        {
            m_SnowballForcer = GetComponentInParent<SnowballForcer>();
            m_ContesterMovement = GetComponentInParent<ContesterMovement>();
        }


        private void Update()
        {
            SetLocalScale();
            // transform.localPosition = new Vector3(0, transform.localPosition.y, 0);

            Collider[] snowballArray = Physics.OverlapSphere(transform.position + new Vector3(0, m_LocalScale.y * 0.5f),
                m_LocalScale.y * 0.5f);

            foreach (var snowball in snowballArray)
            {
                if (snowball.CompareTag("Snow") && snowball.gameObject != gameObject)
                {
                    // TODO KAR ÇARPINCA BÜYÜK OLAN KAR KÜCÜLSÜN
                    
                    var snowball1 = snowball.transform.localScale.y;
                    var snowball2 = transform.localScale.y;
                    
                    var difference = snowball1 - snowball2;
                    
                    if (difference > 0.5f)
                    {
                        DestroySnowball();

                        var localScale = snowball.transform.localScale;
                        localScale = localScale - new Vector3(localScale.x - Mathf.Abs(difference),
                            localScale.y - Mathf.Abs(difference), localScale.z - Mathf.Abs(difference));
                        var lerpScale = Vector3.Lerp(snowball.transform.localScale, localScale, Time.deltaTime * smallSpeed);
                        snowball.transform.localScale = lerpScale;
                        
                        GetSnowballForcer().Force(snowball);
                    }
                    else if (difference < 0.5f)
                    {
                        snowball.GetComponent<SnowballCollision>().DestroySnowball();
                        
                        var localScale = transform.localScale;
                        localScale = localScale - new Vector3(localScale.x - Mathf.Abs(difference),
                            localScale.y - Mathf.Abs(difference), localScale.z - Mathf.Abs(difference));
                        var lerpScale = Vector3.Lerp(transform.localScale, localScale, Time.deltaTime * smallSpeed);
                        snowball.transform.localScale = lerpScale;
                        
                        snowball.GetComponent<SnowballCollision>().GetSnowballForcer().Force(GetComponent<Collider>());
                    }
                    else
                    {
                        Destroy(gameObject.transform.parent.gameObject);
                        Destroy(snowball.transform.parent.gameObject);
                    }
                }
            }
        }

        private void SetLocalScale()
        {
            m_LocalScale = transform.localScale;
        }

        public bool IsGrounded()
        {
            return Physics.CheckSphere
                (transform.position + new Vector3(0, m_LocalScale.y * 0.5f), m_LocalScale.y * 0.5f, ground);
        }


        private SnowballForcer GetSnowballForcer()
        {
            return m_SnowballForcer;
        }


        private SnowballCollision GetSnowballCollision()
        {
            return this;
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Area"))
            {
                DestroySnowballFromWall();
            }
        }

        public void DestroySnowballFromWall()
        {
            if (transform.localScale.y < 1.5f) return;
            
            Destroy(Instantiate(boooomParticle, transform.position, Quaternion.identity), 1.51f);
            OnSnowballDestroyed?.Invoke(this, EventArgs.Empty);
            SoundManager.Instance.PlaySnowbuttBooomParticleSound();
            // Destroy(transform.parent.gameObject);
            gameObject.SetActive(false);
        }

        private void DestroySnowball()
        {
            if (transform.localScale.y < 1.5f) return;

            
            // TODO SESLERİ İFLERDE ÇAL
            var boomParticle = Instantiate(boooomParticle, transform.position, Quaternion.identity);

            if (canPlaySound)
            {
                OnSnowballDestroyed?.Invoke(this, EventArgs.Empty);
                SoundManager.Instance.PlaySnowbuttBooomParticleSound();
                canPlaySound = false;
            }
            else
            {
                // boomParticle.GetComponent<AudioSource>().volume = 0f;
                LazyCoroutines.WaitForSeconds(2f, () =>
                {
                    canPlaySound = true;
                });
            }
            
            Destroy(boomParticle, 1.51f);
            Destroy(transform.parent.gameObject);
        }
    }
}
