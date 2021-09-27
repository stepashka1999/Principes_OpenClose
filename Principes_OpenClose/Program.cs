using System;
using System.Collections.Generic;

namespace Principes_OpenClose
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var products = GetProducts();
            var betterFilter = new BetterFilter();
            
            Console.WriteLine("Large items:");
            var largeSpec = new SizeSpecification(Size.Large);
            var largeItems = betterFilter.Filter(products, largeSpec);
            PrintResult(largeItems);

            Console.WriteLine("\nLarge and Blue items:");
            var largeAndGreenSpec = new AndSpecification<Product>(largeSpec, new ColorSpecification(Color.Green));
            var largeAndGreenItems = betterFilter.Filter(products, largeAndGreenSpec);
            PrintResult(largeAndGreenItems);
            
            //Or we can use another one
            Console.WriteLine("\nGreen and Small:");
            var greenSpec = new ColorSpecification(Color.Green);
            var smallSpec = new SizeSpecification(Size.Small);
            var newSpec = (greenSpec as ISpecification<Product>) & smallSpec;
            var greenAndSmallItems = betterFilter.Filter(products, newSpec);
            PrintResult(greenAndSmallItems);
        }

        private static IEnumerable<Product> GetProducts()
        {
            var apple1 = new Product("Apple", Color.Green, Size.Small);
            var apple2 = new Product("Apple", Color.Red, Size.Small);
            var car = new Product("Car", Color.Blue, Size.Medium);
            var house = new Product("House", Color.Yellow, Size.Large);
            var tree = new Product("Tree", Color.Green, Size.Large);

            return new Product[]{ apple1, apple2, car, house, tree };
        }

        private static void PrintResult(IEnumerable<Product> products)
        {
            foreach (var item in products)
            {
                Console.WriteLine("\t"+item.Name);
            }
        }
    }
}
