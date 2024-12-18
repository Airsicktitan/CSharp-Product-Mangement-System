namespace ConsoleApp6;

public class Utilities
{
    private static readonly Menu menu = new();
    private static readonly Services services = new();
    public void AddProduct()
    {
        var title = "Adding A New Product (press r anytime to return)";
        Console.Clear();
        Console.WriteLine(title);
        for (int i = 0; i < title.Length; i++) Console.Write("_");
        Console.WriteLine();
        var name = string.Empty;
        var description = string.Empty;
        var quantity = string.Empty;
        var isAvailable = string.Empty;
        var isOnSale = string.Empty;

        while (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(quantity) || string.IsNullOrEmpty(isAvailable)
            || string.IsNullOrEmpty(isOnSale))
        {
            Console.Write("Name: ");
            name = Console.ReadLine();

            if (name.ToLower() == "r")
            {
                Console.Clear();
                menu.MenuSelection();
            }

            Console.Write("Description: ");
            description = Console.ReadLine();

            if (description.ToLower() == "r")
            {
                Console.Clear();
                menu.MenuSelection();
            }

            Console.Write("Quantity: ");
            quantity = Console.ReadLine();

            if (quantity.ToLower() == "r")
            {
                Console.Clear();
                menu.MenuSelection();
            }

            try
            {
                var userQuantity = Int32.Parse(quantity);
            }
            catch
            {
                Console.WriteLine("Invalid format, please type a number: ");
                quantity = string.Empty;

                Console.Write("Quantity: ");
                quantity = Console.ReadLine();
            }

            Console.Write("Available (Yes or No): ");
            isAvailable = Console.ReadLine();

            if (isAvailable.ToLower() == "r")
            {
                Console.Clear();
                menu.MenuSelection();
            }

            if (isAvailable.ToUpper() != "YES" && isAvailable.ToUpper() != "NO")
            {
                Console.WriteLine("Invalid selection.  Please Type Yes or No.");
                isAvailable = string.Empty;
                Console.Write("Available: ");
                isAvailable = Console.ReadLine();
            }

            Console.Write("Sale (Yes or No): ");
            isOnSale = Console.ReadLine();

            if (isOnSale.ToLower() == "r")
            {
                Console.Clear();
                menu.MenuSelection();
            }

            if (isOnSale.ToUpper() != "YES" && isOnSale.ToUpper() != "NO")
            {
                Console.WriteLine("Invalid selection.  Please Type Yes or No.");
                isOnSale = string.Empty;
                Console.Write("Sale: ");
                isOnSale = Console.ReadLine();
            }
        }

        if (isAvailable.ToUpper() == "YES") isAvailable = "1";
        else if (isAvailable.ToUpper() == "NO") isAvailable = "0";

        if (isOnSale.ToUpper() == "YES") isOnSale = "1";
        else if (isOnSale.ToUpper() == "NO") isOnSale = "0";

        Guid guid = Guid.NewGuid();

        string rowsAffected = services.AddProductQuery(name, description, quantity, isAvailable, isOnSale, guid);

        Console.WriteLine(rowsAffected);

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
                    menu.MenuSelection();
                    return;
                default:
                    Console.WriteLine("Invalid selection. Try again.");
                    break;
            }
        }
    }

    public void ViewProducts()
    {
        var title = "Viewing All Products (press r anytime to return)";
        Console.Clear();
        Console.WriteLine(title);
        for (int i = 0; i < title.Length; i++) Console.Write("_");
        Console.WriteLine();

        services.ViewAllProducts();
       
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
                    menu.MenuSelection();
                    return;
                default:
                    Console.WriteLine("Invalid selection. Try again.");
                    break;
            }
        }
    }

    public void EditProduct(int id, string name)
    {
        var title = $"Editing {id} - {name} (press r anytime to return)"; // To contain a product name later
        Console.Clear();
        Console.WriteLine(title);
        for (int i = 0; i < title.Length; i++) Console.Write("_");
        Console.WriteLine();

        string[] options =
            [
                    "A) Edit Product Name",
                    "B) Edit Product Description",
                    "C) Edit Product Quantity",
                    "D) Edit Product Available",
                    "E) Edit Product Sale"
            ];
        Console.WriteLine();

        foreach (string option in options) Console.WriteLine(option);

        var columnToEdit = string.Empty;
        var newValue = string.Empty;

        while (true)
        {
            Console.Write("\nOption: ");
            var user_selection = Console.ReadLine();

            switch (user_selection.ToString().ToUpper())
            {
                case "R":
                    Console.Clear();
                    menu.MenuSelection();
                    return;
                case "A":
                    Console.Write("New Name: ");
                    columnToEdit = "Name";
                    newValue = Console.ReadLine();

                    break;
                case "B":
                    Console.Write("New Description: ");
                    columnToEdit = "Description";
                    newValue = Console.ReadLine();

                    break;
                case "C":
                    Console.Write("New Quantity: ");
                    columnToEdit = "Quantity";
                    newValue = Console.ReadLine();

                    break;
                case "D":
                    Console.Write("New Availability (Yes or No): ");
                    columnToEdit = "isAvailable";
                    newValue = Console.ReadLine()?.ToUpper();
                    if (newValue != "YES" && newValue != "NO")
                    {
                        Console.WriteLine("Invalid selection.  Please select Yes or No.");
                        continue;
                    }
                    newValue = (newValue == "YES") ? "1" : "0";

                    break;
                case "E":
                    Console.Write("New Sale Status (Yes or No): ");
                    columnToEdit = "isOnSale";
                    newValue = Console.ReadLine();
                    if (newValue != "YES" && newValue != "NO")
                    {
                        Console.WriteLine("Invalid selection.  Please select Yes or No.");
                        continue;
                    }
                    newValue = (newValue == "YES") ? "1" : "0";

                    break;
                default:
                    Console.Write("Invalid selection. Try again.");
                    continue;
            }

            services.EditProductQuery(id, columnToEdit, newValue);

            Console.WriteLine("Do you want to make another edit? (Yes or No)");
            var again = Console.ReadLine()?.ToUpper();

            if (again == "NO")
            {
                Console.Clear();
                menu.MenuSelection();
                return;
            }
            else if (again != "YES")
            {
                Console.WriteLine("Invalid selection, returning to main menu.");
                Console.Clear();
                menu.MenuSelection();
                return;
            }
        }
    }

    public void DeleteProduct(int id, string name)
    {
        var title = $"Deleting {id} - {name} (press r anytime to return)";
        Console.Clear();
        Console.WriteLine(title);
        for (int i = 0; i < title.Length; i++) Console.Write("_");
        Console.WriteLine();

        var result = services.DeleteProduct(id);

        Console.WriteLine(result);

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
                    menu.MenuSelection();
                    return;
                default:
                    Console.WriteLine("Invalid selection. Try again.");
                    break;
            }
        }
    }

}
