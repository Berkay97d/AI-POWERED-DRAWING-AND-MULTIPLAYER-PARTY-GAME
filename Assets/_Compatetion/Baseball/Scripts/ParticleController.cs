using UnityEngine;

namespace _Compatetion.Baseball.Scripts
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleArray;


        public void SetParticleColor(Color color)
        {
            foreach (var particle in particleArray)
            {
                var main = particle.main;

                main.startColor = color;
            }
        }
    }
}