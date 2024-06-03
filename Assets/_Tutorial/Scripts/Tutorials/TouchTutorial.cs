using System;
using UnityEngine;

namespace _Tutorial
{
    [Serializable]
    public class TouchTutorial : Tutorial
    {
        public override void OnUpdate()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            Complete();
        }
    }
}