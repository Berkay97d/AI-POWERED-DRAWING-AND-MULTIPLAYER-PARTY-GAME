using _Compatetion.GameOfSnows.Scripts;
using General;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Ozer.Scripts
{
    public class AiInputProvider : InputProvider
    {
        private float m_HorizontalValue = 0.3f;
        private float m_VerticalValue = 0.47f;
        private float m_Timer = 0f;
        private float m_ChangeInterval = .5f;

        public override float GetHorizontal()
        {
            return m_HorizontalValue;
        }

        public override float GetVertical()
        {
            return m_VerticalValue;
        }

        private void Update()
        {
            GetNewDirection(RandomPos(Random.Range(0, 2)));
        }

        private void GetNewDirection(Vector3 targetPos)
        {
            m_Timer += Time.deltaTime;

            if (m_Timer >= m_ChangeInterval)
            {
                m_Timer = 0f;
                var position = transform.position;
                var direction = (targetPos - position);
                direction.Normalize();
                
                m_HorizontalValue = direction.x;
                m_VerticalValue = direction.z;
            }
        }

        
        private Vector3 RandomAreaPos()
        {
            return new Vector3(Random.Range(-12.5f, 12.5f), 0, Random.Range(-12.5f, 12.5f));
        }


        private Vector3 RandomAliveContesterPos()
        {
            if (GameManager.Instance.GetAliveContesterList().Count > 0)
            {
                var aliveContester = GameManager.Instance.GetAliveContesterList()[
                    Random.Range(0, GameManager.Instance.GetAliveContesterList().Count)];

                return aliveContester.transform.position - new Vector3(3f, transform.position.y ,3f);
            }

            return RandomAreaPos();
        }


        private Vector3 RandomPos(int random)
        {
            switch (random)
            {
                case 0:
                    return RandomAliveContesterPos();
                
                case 1:
                    return RandomAreaPos();
                
                default:
                    return RandomAreaPos();
            }
        }
    }
}