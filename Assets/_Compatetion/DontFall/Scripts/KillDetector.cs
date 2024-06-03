using System;
using UnityEngine;

namespace DontFall
{
    public class KillDetector : MonoBehaviour
    {
        public event EventHandler OnContesterDied;
        public event EventHandler OnRealPlayerDied;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.gameObject.TryGetComponent(out AI contester))
            {
                Debug.Log("CONTESTER BANA CARPTI");
                contester.SetIsDead(true);
                contester.RaiseOnContesterDrop();
                
                OnContesterDied?.Invoke(this, EventArgs.Empty);
            }
            
            if (other.transform.parent.gameObject.TryGetComponent(out PlayerController player))
            {
                Debug.Log("PLAYER BANA CARPTI");
                player.SetIsDead(true);
                player.RaiseOnContesterDrop();
                
                OnRealPlayerDied?.Invoke(this, EventArgs.Empty);
                OnContesterDied?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}