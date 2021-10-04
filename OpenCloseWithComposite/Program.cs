using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCloseWithComposite
{
    internal class Program
    {
        #region Enumerators - Size and Color
       
        public enum Size
        {
            Small, 
            Medium,
            Large
        }

        public enum Color
        {
            Blue,
            Green,
            Red,
            Yellow,
            Gray,
            Black
        }

        #endregion

        public class Product
        {
            public string Name { get; set; }
            
            public Size Size { get; set; }

            public Color Color { get; set; }


            public override string ToString()
            {
                return $"- {nameof(Name)}: {Name}, {nameof(Color)}: {Color}, {nameof(Size)}: {Size}";
            }
        }

        #region Specifications - Inretafce ISpecification<T> and other realisations by Product params and AND/OR specifications
       
        public interface ISpecification<T>
        {
            bool IsSatisfied(T item);

            static ISpecification<T> operator &(ISpecification<T> first, ISpecification<T> second)
            {
                return new AndSpecification<T>(first, second);
            }

            static ISpecification<T> operator |(ISpecification<T> first, ISpecification<T> second)
            {
                return new OrSpecification<T>(first, second);
            }
        }

        public class NameSpecification : ISpecification<Product>
        {
            private readonly string _name;

            public NameSpecification(string name)
            {
                _name = name;
            }

            public bool IsSatisfied(Product item)
            {
                return item.Name == _name;
            }
        }

        public class SizeSpecification : ISpecification<Product>
        {
            private readonly Size _size;

            public SizeSpecification(Size size)
            {
                _size = size;
            }

            public bool IsSatisfied(Product item)
            {
                return item.Size == _size;
            }
        }

        public class ColorSpecification : ISpecification<Product>
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

        public abstract class CompositeSpecification<T> : ISpecification<T>
        {
            protected readonly ISpecification<T>[] specifications;

            public CompositeSpecification(params ISpecification<T>[] specifications)
            {
                this.specifications = specifications;
            }

            public abstract bool IsSatisfied(T item);
        }

        public class AndSpecification<T> : CompositeSpecification<T>
        {
            public AndSpecification(params ISpecification<T>[] items) : base(items) { }

            public override bool IsSatisfied(T item)
            {
                return specifications.All(sp => sp.IsSatisfied(item));
            }
        }

        public class OrSpecification<T> : CompositeSpecification<T>
        {
            public OrSpecification(params ISpecification<T>[] specifications) : base(specifications) {  }

            public override bool IsSatisfied(T item)
            {
                return specifications.Any(sp => sp.IsSatisfied(item));
            }
        }

        #endregion

        #region Filter - Interface IFIlter and ProductFilter

        public interface IFilter<T>
        {
            IEnumerable<T> Filter(IEnumerable<T> item, ISpecification<T> specification);
        }

        public class ProductFilter : IFilter<Product>
        {
            public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> specification)
            {
                return items.Where(i => specification.IsSatisfied(i));
            }
        }

        #endregion

        static void Main(string[] args)
        {
            var products = new Product[]
            {
               new Product { Size = Size.Small, Color = Color.Blue, Name = "Car" },
               new Product { Size = Size.Small, Color = Color.Red, Name = "Apple" },
               new Product { Size = Size.Small, Color = Color.Yellow, Name = "Lemon" },
               new Product { Size = Size.Large, Color = Color.Gray, Name = "Rock" },
               new Product { Size = Size.Medium, Color = Color.Green, Name = "Flowers" },
               new Product { Size = Size.Small, Color = Color.Blue, Name = "Pen" },
               new Product { Size = Size.Large, Color = Color.Blue, Name = "Bus" },
               new Product { Size = Size.Large, Color = Color.Green, Name = "Tree" },
               new Product { Size = Size.Medium, Color = Color.Red, Name = "Bike" }
            };

            var graySpec = new ColorSpecification(Color.Gray);
            var greenSpec = new ColorSpecification(Color.Green);
            var blueSpec = new ColorSpecification(Color.Blue);

            var smallSpec = new SizeSpecification(Size.Small);
            var mediumSpec = new SizeSpecification(Size.Medium);
            var largeSpec = new SizeSpecification(Size.Large);

            var carSpec = new NameSpecification("Car");
            var penSpec = new NameSpecification("Pen");
            var treeSpec = new NameSpecification("Tree");

            var smallAndBlueSpec = (ISpecification<Product>)smallSpec & blueSpec;
            var smallAndBlueAndPenSpec = smallAndBlueSpec & carSpec;

            var greenOrBlueSpec = (ISpecification<Product>)greenSpec | blueSpec;
            var greenOrBlueOrGraySpec = greenOrBlueSpec | graySpec;


            var filter = new ProductFilter();
            
            var grayItems = filter.Filter(products, graySpec);
            var greenItems = filter.Filter(products, greenSpec);
            var blueitems = filter.Filter(products, blueSpec);

            var smallItems = filter.Filter(products, smallSpec);
            var mediumItems = filter.Filter(products, mediumSpec);
            var largeItems = filter.Filter(products, largeSpec);

            var carItems = filter.Filter(products, carSpec);
            var penItems = filter.Filter(products, penSpec);
            var treeItems = filter.Filter(products, treeSpec);

            var smallAndBlueItems = filter.Filter(products, smallAndBlueSpec);
            var smallAndBlueAndPen = filter.Filter(products, smallAndBlueAndPenSpec);

            var greenOrBlueItems = filter.Filter(products, greenOrBlueSpec);
            var greenOrBlueOrGrayItems = filter.Filter(products, greenOrBlueOrGraySpec);


            Console.WriteLine("All products:");
            PrintCollection(products);
            Console.WriteLine();

            Console.WriteLine("---> Color Specifications <---");
            Console.WriteLine("Gray products:");
            PrintCollection(grayItems);
            Console.WriteLine();

            Console.WriteLine("Green products:");
            PrintCollection(greenItems);
            Console.WriteLine();

            Console.WriteLine("Blue products:");
            PrintCollection(blueitems);
            Console.WriteLine();

            Console.WriteLine("---> Size Specifications <---");
            Console.WriteLine("Small products:");
            PrintCollection(smallItems);
            Console.WriteLine();

            Console.WriteLine("Medium products:");
            PrintCollection(mediumItems);
            Console.WriteLine();

            Console.WriteLine("Large products:");
            PrintCollection(largeItems);
            Console.WriteLine();

            Console.WriteLine("---> Name Specifications <---");
            Console.WriteLine("Car products:");
            PrintCollection(carItems);
            Console.WriteLine();

            Console.WriteLine("Pen products:");
            PrintCollection(penItems);
            Console.WriteLine();

            Console.WriteLine("Tree products:");
            PrintCollection(treeItems);
            Console.WriteLine();

            Console.WriteLine("---> And Specifications <---");
            Console.WriteLine("Small and Blue products:");
            PrintCollection(smallAndBlueItems);
            Console.WriteLine();

            Console.WriteLine("Small and Blue and Pen products:");
            PrintCollection(smallAndBlueAndPen);
            Console.WriteLine();

            Console.WriteLine("---> And Specifications <---");
            Console.WriteLine("Green or Blue products:");
            PrintCollection(greenOrBlueItems);
            Console.WriteLine();

            Console.WriteLine("Green or Blue or Green products:");
            PrintCollection(greenOrBlueOrGrayItems);
            Console.WriteLine();

        }

        private static void PrintCollection(IEnumerable<Product> collection)
        {
            foreach (var product in collection)
            {
                Console.WriteLine(product);
            }
        }
    }
}
