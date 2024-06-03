using System;
using UnityEngine;

namespace StableDiffusionAPI
{
    public static class TextureUtils
    {
        public static Texture2D FromBase64String(string base64, int width, int height)
        {
            var imageBytes = Convert.FromBase64String(base64);
            var texture = new Texture2D(width, height);
            
            texture.LoadImage(imageBytes);

            return texture;
        }

        public static string ToBase64String(Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();
            return Convert.ToBase64String(bytes);
        }
    }
}