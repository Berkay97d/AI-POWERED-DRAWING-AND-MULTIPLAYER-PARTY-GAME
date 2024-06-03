namespace _CardPacks.Scripts
{
    public enum CardPackType
    {
        None,
        Common,
        Rare,
        Legendary
    }

    public static class CardPackTypeExtensions
    {
        public static int ToIndex(this CardPackType cardPackType)
        {
            return (int) cardPackType -1;
        }
    }
}