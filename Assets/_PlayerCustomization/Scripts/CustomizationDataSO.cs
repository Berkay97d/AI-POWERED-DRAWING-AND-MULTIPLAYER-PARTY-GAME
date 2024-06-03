using UnityEngine;

namespace _PlayerCustomization.Scripts
{
    [CreateAssetMenu]
    public class CustomizationDataSO : ScriptableObject
    {
        public Sprite[] headSprites;
        public Sprite[] upperBodySprites;
        public Sprite[] lowerBodySprites;
        public Sprite[] footSprites;
        public Sprite bodySprite;
        public Color[] bodyColors;
    }
}