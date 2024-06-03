using System;
using System.Collections;
using General;
using UnityEngine;

namespace _Compatetion.Jumping.Scripts.Obstacles
{
    public class ObstacleManager : MonoBehaviour
    {
        public static ObstacleManager Instance;
        
        [SerializeField] private Obstacle[] obstacleArray;
        [SerializeField] private float rotateTime;
        
        private float m_TotalRotation;


        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            LazyCoroutines.WaitForSeconds(0f, () =>
            {
                obstacleArray[0].Scale();
            });
            
            LazyCoroutines.WaitForSeconds(5f, () =>
            {
                obstacleArray[1].Scale();
            });
            
            LazyCoroutines.WaitForSeconds(10f, () =>
            {
                obstacleArray[2].Scale();
            });
            
            LazyCoroutines.WaitForSeconds(15f, () =>
            {
                obstacleArray[3].Scale();
            });

            StartCoroutine(ChangeRotateSpeed());
        }


        private void Update()
        {
            Rotate();
        }

        
        private void Rotate()
        {
            // Gradually reduce rotateTime over time
            
            float incrementalRotation = 360f * Time.deltaTime / rotateTime;

            m_TotalRotation += incrementalRotation;

            m_TotalRotation %= 360f;

            var rotation = transform.rotation;
            rotation = Quaternion.Euler(new Vector3(rotation.eulerAngles.x, m_TotalRotation, rotation.eulerAngles.z));

            transform.rotation = rotation;
        }

        private IEnumerator ChangeRotateSpeed()
        {
            while (true)
            {
                rotateTime -= (rotateTime / 10);

                yield return new WaitForSeconds(5f);
            }
        }


        public Obstacle[] GetObstacleArray()
        {
            return obstacleArray;
        }
    }
}
