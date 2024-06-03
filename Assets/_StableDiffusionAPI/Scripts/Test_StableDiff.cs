using System.Collections;
using ImageUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StableDiffusionAPI
{
    public class Test_StableDiff : MonoBehaviour
    {
        [SerializeField] private TMP_InputField promptInputField;
        [SerializeField] private Button imageToImageButton;
        [SerializeField] private Image previewImage;
        [SerializeField] private Texture2D referenceTexture;


        private bool isProcessing;


        private void Awake()
        {
            imageToImageButton.onClick.AddListener(OnClickImageToImage);
            previewImage.sprite = referenceTexture.ToSprite();
        }


        public void OnClickImageToImage()
        {
            if (isProcessing) return;

            isProcessing = true;
            Debug.Log("processing");

            var prompt = GetPrompt();
            var base64 = TextureUtils.ToBase64String(referenceTexture);
            
            StableDiffusion.ImageToImage(prompt, base64, (textures, imageBase64) =>
            {
                Debug.Log("succeed");
                isProcessing = false;
                PreviewResponse(textures);
            });
        }


        private string GetPrompt()
        {
            return promptInputField.text;
        }

        private void PreviewResponse(Texture2D[] textures)
        {
            StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                foreach (var texture in textures)
                {
                    yield return new WaitForSeconds(0.75f);
                    
                    previewImage.sprite = texture.ToSprite();
                }
            }
        }
    }
}