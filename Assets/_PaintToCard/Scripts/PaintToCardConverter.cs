using System;
using System.Collections;
using _DatabaseAPI.Scripts;
using _Painting;
using ImageUtils;
using StableDiffusionAPI;
using UnityEngine;
using UnityEngine.UI;

namespace _PaintToCard.Scripts
{
    public class PaintToCardConverter : MonoBehaviour
    {
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Image previewImage;
        
        
        public static event Action<Texture2D> OnImageGenerated;


        private static Texture2D ms_GeneratedImage;
        private static string ms_GeneratedImageBase64;
        private static string ms_GeneratedImageID;
        private static string ms_Prompt;
        
        
        private void Awake()
        {
            PaintManager.OnPaintingComplete += PaintManager_OnPaintingComplete;
        }

        private void OnDestroy()
        {
            PaintManager.OnPaintingComplete -= PaintManager_OnPaintingComplete;
        }


        private void PaintManager_OnPaintingComplete(PaintingData data)
        {
            GenerateImage(data);
        }

        
        public void GenerateImage(PaintingData data)
        {
            ShowLoadingPanel();

            StableDiffusion.ImageToImage(data.prompt, data.imageBase64, (textures, imageBase64) =>
            {
                var generatedTexture = textures[^1];

                ms_Prompt = data.prompt;
                ms_GeneratedImage = generatedTexture;
                ms_GeneratedImageBase64 = imageBase64;
                
                DatabaseAPI.PostImage(imageBase64, response =>
                {
                    ms_GeneratedImageID = response.body;
                    
                    PreviewImage(textures, () =>
                    {
                        OnImageGenerated?.Invoke(generatedTexture);
                    });
                });
            });
        }
        

        private void ShowLoadingPanel()
        {
            loadingPanel.SetActive(true);
        }

        private void HideLoadingPanel()
        {
            loadingPanel.SetActive(false);
        }

        private void PreviewImage(Texture2D[] images, Action onComplete)
        {
            StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                foreach (var texture in images)
                {
                    yield return new WaitForSeconds(0.75f);
                    
                    previewImage.sprite = texture.ToSprite();
                }
                
                yield return new WaitForSeconds(0.75f);

                onComplete?.Invoke();
            }
        }


        public static Texture2D GetGeneratedImage()
        {
            return ms_GeneratedImage;
        }

        public static string GetGeneratedImageBase64()
        {
            return ms_GeneratedImageBase64;
        }

        public static string GetGeneratedImageID()
        {
            return ms_GeneratedImageID;
        }

        public static string GetPrompt()
        {
            return ms_Prompt;
        }
    }
}