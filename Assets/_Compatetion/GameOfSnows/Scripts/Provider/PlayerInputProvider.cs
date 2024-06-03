using _Compatetion.GameOfSnows.Ozer.Scripts;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Scripts.Provider
{
    public class PlayerInputProvider : InputProvider
    {
        [SerializeField] private FloatingJoystick joystick;


        public override float GetHorizontal()
        {
            return joystick.Horizontal;
        }

        public override float GetVertical()
        {
            return joystick.Vertical;
        }
    }
}