using _Compatetion.GameOfSnows.Ozer.Scripts;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Scripts.Snowball
{
    public class SnowBallRotater : MonoBehaviour
    {
        private ContesterMovement m_ContesterMovement;
        float rotationSpeed;
        float rotater;

        private void Awake()
        {
            m_ContesterMovement = GetComponentInParent<ContesterMovement>();
        }

        // Update is called once per frame
        void Update() {
            //rotation on x axis will decrase or increase relative to player velocity
            RotationHandler();
        }

        void RotationHandler()
        {
            rotater += rotationSpeed * Time.deltaTime;

            if (m_ContesterMovement.GetDirection()!= Vector3.zero)
            {
                rotationSpeed += 20f;
                rotationSpeed = Mathf.Clamp(rotationSpeed, 0, 300f);
                transform.GetChild(1).rotation = Quaternion.Euler(-rotater, m_ContesterMovement.transform.eulerAngles.y, 0f);
            }
            else rotationSpeed = 0f;
        }
    }
}
