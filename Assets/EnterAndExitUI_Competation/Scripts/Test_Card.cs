using UnityEngine;
using UnityEngine.UI;

namespace EnterAndExitUI_Competation.Scripts
{
    public class Test_Card : MonoBehaviour
    {
        [SerializeField] private Sprite[] raritySprites;
        [SerializeField] private Image image;


        public void SetRarityID(int id)
        {
            image.sprite = raritySprites[id];
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetLocalPosition(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }
    }
}