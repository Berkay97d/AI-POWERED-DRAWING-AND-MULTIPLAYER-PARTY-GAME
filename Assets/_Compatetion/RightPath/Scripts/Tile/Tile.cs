using UnityEngine;

namespace RightPath
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private Color safeColor;
        
        
        private bool isInRightPath;
        

        public void Drop()
        {
            gameObject.SetActive(false);
        }

        public void ShowTileIsSafe()
        {
            renderer.material.color = safeColor;
        }
        
        public void MarkAsRightPath()
        {
            isInRightPath = true;
        }

        public bool GetIsInRightPath()
        {
            return isInRightPath;
        }
    }
}