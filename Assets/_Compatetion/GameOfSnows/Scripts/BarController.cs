using _Compatetion.GameOfSnows.Ozer.Scripts;
using _Compatetion.GameOfSnows.Scripts.Snowball;
using UnityEngine;
using UnityEngine.UI;

namespace _Compatetion.GameOfSnows.Scripts
{
    public class BarController : MonoBehaviour
    {
        [SerializeField] private ContesterCollision contesterCollision;
        [SerializeField] private Image barImage;
        
        private SnowballScaler m_SnowballScaler;


        private void Start()
        {
            contesterCollision.OnSnowballCreated += OnSnowballCreated;
        }


        private void Update()
        {
            if (barImage == null || m_SnowballScaler == null) return;
            barImage.fillAmount = m_SnowballScaler.GetSnowballVisual().transform.localScale.y / 8;
            barImage.color = Color.Lerp(Color.green, Color.red,
                m_SnowballScaler.GetSnowballVisual().transform.localScale.y / 8);
        }


        private void OnSnowballCreated(object sender, SnowballScaler e)
        {
            m_SnowballScaler = e;
        }


        private void OnDestroy()
        {
            contesterCollision.OnSnowballCreated -= OnSnowballCreated;
        }
    }
}
