using UnityEngine;
using UnityEngine.UI;

namespace _ConnectionCheck
{
    public class ConnectionLossWarning : MonoBehaviour
    {
        [SerializeField] private Sprite[] connectionLossArray;
        [SerializeField] private Image connectionImage;
        [SerializeField] private float updateInterval;

        private int currentConnectionPhase = 0;
        private float phaseTime;
    
        private void Start()
        {
            connectionImage.sprite = connectionLossArray[0];
            
        }
        
        private void Update()
        {
            
            phaseTime += Time.deltaTime;
    
            if (phaseTime >= updateInterval)
            {
                phaseTime = 0;
                currentConnectionPhase++;
                connectionImage.sprite = connectionLossArray[currentConnectionPhase % 4];
            }
        }
        
    }
}