using System;
using System.Collections;
using _SoundSystem.Scripts;
using General;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Compatetion.Baseball.Scripts
{
    public class ContesterKicker : MonoBehaviour
    {
        [SerializeField] private Contester contester;
        [SerializeField] private Transform fruitCreateTransform;
        [SerializeField] private Transform targetCircle;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private AnimationCurve pointCurve;
        [SerializeField] private int maxPoint;
        [SerializeField] private int minPoint;
        [SerializeField] private Transform targetCirclePrefab;
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem boomParticlePrefab;
        [SerializeField] private Color orangeColor;

        public event Action<Color> OnCreatedCircleInFruit; 

        private IContesterInput m_Input;
        private Fruit m_Fruit;
        private float m_Distance;
        private int m_CurrentPoint;
        private static readonly int Kick2 = Animator.StringToHash("Kick2");
        private bool m_Flag = true;


        private void Awake()
        {
            m_Input = GetComponent<IContesterInput>();

            if (m_Input != null)
            {
                m_Input.OnKickInput += OnKickInput;
            }
        }


        private void Start()
        {
            FruitManager.Instance.OnFruitsCreated += OnFruitsCreated;
        }


        private void Update()
        {
            if (m_Flag)
            {
                if (m_Fruit != null && m_Fruit.transform.localPosition.y < -5f)
                {
                    m_Flag = false;
                    OnCreatedCircleInFruit?.Invoke(Color.red);
                    contester.AddContesterPoint(0);
                }
            }
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private void OnFruitsCreated()
        {
            Fruit fruit = fruitCreateTransform.GetChild(0).GetComponent<Fruit>();
            m_Fruit = fruit;
            m_Flag = true;
        }


        private void OnKickInput()
        {
            Kick();
        }
        

        private void Kick()
        {
            if (m_Fruit != null)
            {
                SoundManager.Instance.PlayKatanaCutSound();
                
                var distance = Mathf.Abs(targetCircle.transform.position.y - m_Fruit.transform.position.y);
                m_Distance = distance / maxDistance;
                
                animator.SetTrigger(Kick2);
                ClampDistance();
                GetPointToCurve();

                if (m_CurrentPoint == 100)
                {
                    SoundManager.Instance.PlayKatanaCorrectSliceSound();
                }
                
                
                if (m_CurrentPoint > 0)
                {
                    CreateSlicesFruit();
                }
                else
                {
                    m_Fruit = null;
                }
            }
        }

        
        private void CreateSlicesFruit()
        {
            var slicesFruit =
                FruitManager.Instance.GetSlicesFruitPrefabArray()[FruitManager.Instance.GetFruitIndex()];
            var currentSlices = Instantiate(slicesFruit, m_Fruit.transform.position, Quaternion.identity);


            foreach (Transform slice in currentSlices)
            {
                var rb = slice.GetComponent<Rigidbody>();
                
                rb.AddForce(RandomForce(), ForceMode.Impulse);
                rb.AddTorque(RandomTorque(), ForceMode.Impulse);
            }

            m_Fruit.DestroySelf(0f);
            CreateParticle();
        }

        private void CreateParticle()
        {
            var particle = Instantiate(boomParticlePrefab, m_Fruit.transform.position, Quaternion.identity);
            
            
            switch (FruitManager.Instance.GetFruitIndex())
            {
                
                case 0:
                {
                    particle.GetComponent<ParticleController>().SetParticleColor(Color.red);
                    break;
                }
                case 1:
                {
                    particle.GetComponent<ParticleController>().SetParticleColor(new Color(255, 165, 0));
                    break;
                }
                case 2:
                {
                    particle.GetComponent<ParticleController>().SetParticleColor(Color.yellow);
                    break;
                }
                case 3:
                {
                    particle.GetComponent<ParticleController>().SetParticleColor(Color.green);
                    break;
                }
                case 4:
                {
                    particle.GetComponent<ParticleController>().SetParticleColor(new Color(255, 165, 0));
                    break;
                }
            }
        }


        private void ClampDistance()
        {
            m_Distance = Mathf.Clamp(m_Distance, minDistance, maxDistance);
        }

        
        private void GetPointToCurve()
        {
            var t = pointCurve.Evaluate(m_Distance);

            CreateTargetCircleInFruit(t);

            var point = Mathf.RoundToInt(Mathf.Lerp(minPoint, maxPoint, t));
            m_CurrentPoint = point;
            
            contester.AddContesterPoint(point);
        }

        
        private void CreateTargetCircleInFruit(float t)
        {
            var position = m_Fruit.transform.position;
            
            var fruitTargetCircle = Instantiate(targetCirclePrefab, new Vector3(position.x, position.y, position.z - 0.01f),
                Quaternion.identity);
            var color = fruitTargetCircle.GetComponentInChildren<SpriteRenderer>().color =
                Color.Lerp(Color.red, Color.green, t);
            
            OnCreatedCircleInFruit?.Invoke(color);
            
            Destroy(fruitTargetCircle.gameObject, 0.75f);
        }


        public int GetCurrentPoint()
        {
            return m_CurrentPoint;
        }
        
        
        private Vector3 RandomForce()
        {
            var xForce = Random.Range(-2, 2);
            var zForce = Random.Range(60, 70);
            return new Vector3(xForce, 0, 0);
        }

        
        private Vector3 RandomTorque()
        {
            var xTorque = Random.Range(-50, 50);
            var yTorque = Random.Range(-50, 50);
            var zTorque = Random.Range(-50, 50);

            return new Vector3(xTorque, yTorque, zTorque);
        }
    }
}