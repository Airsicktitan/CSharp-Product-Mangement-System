using System;
using System.Runtime.CompilerServices;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        static void Menu()
        {
            var title = "Please make a selection:";
            string[] options =
                [
                    "A) Add A Product",
                    "B) View Products",
                    "C) Edit A Product",
                    "D) Delete a Product",
                    "X) Exit"
                ];

            Console.WriteLine(title);

            for (int i = 0; i < title.Length; i++) Console.Write("_");

            Console.WriteLine();
            Console.WriteLine();

            foreach(string option in options) Console.WriteLine(option);

            while(true)
            {
                Console.Write("\nOption: ");
                var user_selection = Console.ReadLine();

                if (String.IsNullOrEmpty(user_selection))
                {
                    Console.WriteLine("Please select a value.");
                    continue;
                }

                switch (user_selection.ToString().ToUpper())
                {
                    case "A":
                        AddProduct();
                        return;
                    case "B":
                        ViewProducts();
                        return;
                    case "C":
                        EditProduct();
                        return;
                    case "D":
                        DeleteProduct();
                        return;
                    case "X":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid selection.  Try again.");
                        break;

                }
            }

        }

        static void AddProduct()
        {
            var title = "Adding A New Product (press r to return)";
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < title.Length; i++) Console.Write("_");
            Console.WriteLine();

            while (true)
            {
                Console.Write("\nOption: ");
                var user_selection = Console.ReadLine();

                if (String.IsNullOrEmpty(user_selection))
                {
                    Console.WriteLine("Please select a value");
                    continue;
                }

                switch (user_selection.ToString().ToUpper())
                {
                    case "R":
                        Console.Clear();
                        Menu();
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Try again.");
                        break;
                }
            }
        }

        static void ViewProducts()
        {
            var title = "Viewing All Products (press r to return)";
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < title.Length; i++) Console.Write("_");
            Console.WriteLine();

            while (true)
            {
                Console.Write("\nOption: ");
                var user_selection = Console.ReadLine();

                if (String.IsNullOrEmpty(user_selection))
                {
                    Console.WriteLine("Please select a value");
                    continue;
                }

                switch (user_selection.ToString().ToUpper())
                {
                    case "R":
                        Console.Clear();
                        Menu();
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Try again.");
                        break;
                }
            }
        }

        static void EditProduct()
        {
            var title = "Editing {Product} (press r to return)"; // To contain a product name later
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < title.Length; i++) Console.Write("_");
            Console.WriteLine();

            while (true)
            {
                Console.Write("\nOption: ");
                var user_selection = Console.ReadLine();

                if (String.IsNullOrEmpty(user_selection))
                {
                    Console.WriteLine("Please select a value");
                    continue;
                }

                switch (user_selection.ToString().ToUpper())
                {
                    case "R":
                        Console.Clear();
                        Menu();
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Try again.");
                        break;
                }
            }
        }

        static void DeleteProduct()
        {
            var title = "Deleting {Product} (press r to return)";  // To contain a product name later
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < title.Length; i++) Console.Write("_");
            Console.WriteLine();


            while (true)
            {
                Console.Write("\nOption: ");
                var user_selection = Console.ReadLine();

                if (String.IsNullOrEmpty(user_selection))
                {
                    Console.WriteLine("Please select a value");
                    continue;
                }

                switch (user_selection.ToString().ToUpper())
                {
                    case "R":
                        Console.Clear();
                        Menu();
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Try again.");
                        break;
                }
            }
        }
    }
}
