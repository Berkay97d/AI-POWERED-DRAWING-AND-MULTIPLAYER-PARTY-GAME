using System;

namespace ImageUtils
{
    public static class Exceptions
    {
        public static Exception CannotDivideByRowCount(int height, int row)
        {
            return new Exception($"Texture height must be dividable by the row count. {height} / {row} is not a whole number!");
        }

        public static Exception CannotDivideByColumnCount(int width, int column)
        {
            return new Exception($"Texture width must be dividable by the column count. {width} / {column} is not a whole number!");
        }
    }
}