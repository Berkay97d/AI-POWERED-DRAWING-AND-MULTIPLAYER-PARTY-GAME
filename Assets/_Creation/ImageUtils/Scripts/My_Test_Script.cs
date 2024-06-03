using UnityEngine;

namespace ImageUtils
{
    public class My_Test_Script : MonoBehaviour
    {
        [SerializeField] private Texture2D texture;
        [SerializeField] private int row;
        [SerializeField] private int column;
        

        private void Start()
        {
            const float pixelPerUnit = 100f;
            
            var sprites = ImageSplitter.Split(texture, row, column);

            var counter = 0;
            for (var y = 0; y < row; y++)
            {
                for (var x = 0; x < column; x++)
                {
                    var obj = new GameObject($"({x}, {y})");
                    var sizeX = x * (texture.width / (float) column) / pixelPerUnit;
                    var sizeY = y * (texture.height / (float) row) / pixelPerUnit;
                    
                    obj.transform.position = new Vector3(sizeX, sizeY);
                    obj.AddComponent<SpriteRenderer>().sprite = sprites[counter];
                    counter += 1;
                }
            }
        }
    }
}