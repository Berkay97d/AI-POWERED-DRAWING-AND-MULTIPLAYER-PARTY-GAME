using System;
using _Compatetion.GameOfSnows.Ozer.Scripts;
using _SoundSystem.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Compatetion.GameOfSnows.Scripts.Snowball
{
    public class SnowballInitilazor : MonoBehaviour
    {
        [SerializeField] private SnowballScaler snowballPrefab;
        [SerializeField] private Transform snowballParent;
        [FormerlySerializedAs("m_ContesterCollision")] [SerializeField] private ContesterCollision contesterCollision;

        public event EventHandler<SnowballCollision> OnSnowballInit; 


        // ReSharper disable Unity.PerformanceAnalysis
        public SnowballScaler InitSnowball()
        {
            var a = Instantiate(snowballPrefab, snowballParent);
            SoundManager.Instance.PlaySnowbuttSnowParticleSound();
            contesterCollision.SetSnowballCollusion(a.GetSnowballCollusion());
            
            OnSnowballInit?.Invoke(this, a.GetSnowballCollusion());
            return a;
        }
    
    
    }
}
