using DG.Tweening;
using UnityEngine;

namespace _Compatetion.Jumping.Scripts.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        private bool m_CanScale = true;
        
        
        public void Scale()
        {
            if (m_CanScale)
            {
                transform.DOScale(new Vector3(0.55f, 0.08f, 0.08f), 3f);
            }
        }


        public void UndoScale()
        {
            m_CanScale = false;
            
            transform.DOScale(new Vector3(0.1f, 0.08f, 0.08f), 0.5f);
        }
    }
}
