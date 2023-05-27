using System;

namespace kursworkModel
{
    internal class Variables
    {
        public ushort A { get; set; } = 0;
        public ushort B { get; set; } = 0;
        public uint AM { get; set; } = 0;
        public uint C { get; set; } = 0;
        public byte Count { get; set; } // Счётчик
        public ushort D { get; set; } = 0; 


        public char[] ToCharArray(uint value)
        {
            char[] chars = Convert.ToString(value, toBase: 2).ToCharArray();
            Array.Reverse(chars, 0, chars.Length);
            return chars;
        }
    }
}
