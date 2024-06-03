using System;
using _Painting;
using StableDiffusionAPI;
using UnityEngine;

namespace _PaintToCard.Scripts
{
    public class Test_PaintToCard : MonoBehaviour
    {
        [SerializeField] private Texture2D texture;


        private void Awake()
        {
            var data = new PaintingData
            {
                prompt = "Volcano",
                imageBase64 = TextureUtils.ToBase64String(texture)
            };
            
            FindObjectOfType<PaintToCardConverter>().GenerateImage(data);
        }
    }
}