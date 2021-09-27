using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principes_OpenClose
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Yellow
    };

    public enum Size
    {
        Small,
        Medium,
        Large
    };

    internal class Product
    {
        public Product(string name, Color color, Size size )
        {
            Name = name;
            Color = color;
            Size = size;
        }

        public string Name { get; private set; }
        public Color Color { get; private set; }
        public Size Size { get; private set; }
    }
}
