using System;
using UnityEngine;

namespace _Compatetion.Jumping.Scripts.Input
{
    public interface IInput
    {
        event Action OnJump;
    }
}