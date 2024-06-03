using System;
using _Compatetion.GameOfSnows.Scripts;
using _Compatetion.GameOfSnows.Scripts.Snowball;
using General;
using UnityEditor;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Ozer.Scripts
{
    public class ContesterCollision : MonoBehaviour
    {
        [SerializeField] private LayerMask snowball;
        [SerializeField] private SnowballInitilazor snowballInitilazor;
        [SerializeField] private Contester contester;

        public event EventHandler<ContesterCollision> OnContesterDie;
        public event EventHandler<SnowballScaler> OnSnowballCreated;
        
        private SnowballCollision m_SnowballCollision;


        private void Update()
        {
            var localPosition = transform.localPosition;
            localPosition = new Vector3(0f, localPosition.y, 0f);
            
            transform.localPosition = localPosition;

            if (m_SnowballCollision)
            {
                Vector3 newPosition = transform.localPosition;
                newPosition.y = m_SnowballCollision.transform.localScale.y + 1;
                transform.localPosition = newPosition;
            }
            else if (transform.position.y <= 2.5 && !m_SnowballCollision)
            {
                var ball = snowballInitilazor.InitSnowball();
                SetSnowballCollusion(ball.GetSnowballCollusion());
                OnSnowballCreated?.Invoke(this, ball);
            }    
            
            // Physics.CheckCapsule(transform.position + new Vector3(0f, 2f, 0f), Vector3.forward, 0.5f, snowball);
        }
        

        public void SetSnowballCollusion(SnowballCollision collision)
        {
            m_SnowballCollision = collision;
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Area"))
            {
                OnContesterDie?.Invoke(this, this);
                
                var parent = gameObject.transform.parent;

                if (m_SnowballCollision != null) m_SnowballCollision.DestroySnowballFromWall();
                parent.gameObject.SetActive(false);
                parent.gameObject.SetActive(false);
                
                contester.SetIsDead(true);
                contester.SetActiveDeadIcon(true);
                contester.GetContesterMovement().SetCanMove(false);
                contester.AddPoint(GameManager.Instance.GetPoint());
                GameManager.Instance.AddPoint(1);

                if (GameManager.Instance.GetPoint() == GameManager.Instance.GetContesterArray().Length)
                {

                    foreach (var contester in GameManager.Instance.GetContesterArray())
                    {
                        if (!contester.IsDead())
                        {
                            contester.GetContesterMovement().SetCanMove(false);
                            contester.SetIsDead(true);
                            contester.SetActiveKingIcon(true);
                            contester.AddPoint(GameManager.Instance.GetPoint());
                            GameManager.Instance.AddPoint(1);

                            LazyCoroutines.WaitForSeconds(1f, ()=>
                            {
                                GameManager.Instance.TryOverTheGame();
                            });
                        }
                    }
                }
            }
        }
    }
}
