using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principes_OpenClose
{
    internal class ColorSpecification : ISpecification<Product>
    {
        private readonly Color _color;
        public ColorSpecification(Color color)
        {
            _color = color;
        }

        public bool IsSatisfied(Product item)
        {
            return item.Color == _color;
        }
    }
}
