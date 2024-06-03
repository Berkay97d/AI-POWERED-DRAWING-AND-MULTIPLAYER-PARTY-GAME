using UnityEngine;

namespace ImageUtils
{
    public static class ImageSplitter
    {
        public static Sprite[] Split(Sprite sprite, int row, int column)
        {
            return Split(sprite.texture, row, column);
        }
        
        public static Sprite[] Split(Texture2D texture, int row, int column)
        {
            /*if (texture.width % column != 0)
            {
                throw Exceptions.CannotDivideByColumnCount(texture.width, column);
            }

            if (texture.height % row != 0)
            {
                throw Exceptions.CannotDivideByRowCount(texture.height, row);
            }*/

            var sprites = new Sprite[row * column];
            var subTextureWidth = texture.width / column;
            var subTextureHeight = texture.height / row;

            var counter = 0;
            for (var r = 0; r < row; r++)
            {
                for (var c = 0; c < column; c++)
                {
                    var subTexture = new Texture2D(subTextureWidth, subTextureHeight, TextureFormat.ARGB32, false);
                    var subPixels = texture.GetPixels(c * subTextureWidth, r * subTextureHeight, subTextureWidth, subTextureHeight);
                    subTexture.SetPixels(subPixels);
                    subTexture.Apply();
                    sprites[counter] = subTexture.ToSprite();
                    counter += 1;
                }
            }

            return sprites;
        }
    }
}
