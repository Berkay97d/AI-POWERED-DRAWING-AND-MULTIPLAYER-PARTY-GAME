using UnityEngine;

namespace _Compatetion.GameOfSnows.Ozer.Scripts
{
    public abstract class InputProvider : MonoBehaviour
    {
        public abstract float GetHorizontal();
        public abstract float GetVertical();
    }
}