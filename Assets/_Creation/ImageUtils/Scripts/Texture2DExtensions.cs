using UnityEngine;

namespace ImageUtils
{
    public static class Texture2DExtensions
    {
        public static Sprite ToSprite(this Texture2D texture)
        {
            var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            var pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture, rect, pivot);
        }
    }
}