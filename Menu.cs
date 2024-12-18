namespace ConsoleApp6;

public class Menu
{
    private static readonly Services services = new();
    private static readonly Utilities utilities = new();
    public void MenuSelection()
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

        foreach (string option in options) Console.WriteLine(option);

        while (true)
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
                    utilities.AddProduct();
                    return;
                case "B":
                    utilities.ViewProducts();
                    return;
                case "C":
                invalidOptionEdit:
                    Console.Write("Enter Product ID to edit: ");
                    var user_selection_id_edit = Console.ReadLine();
                    int id_edit;
                    try
                    {
                        id_edit = int.Parse(user_selection_id_edit);

                        var(productId, productName) = services.CheckItemExists(id_edit);
                        if (productId == -1)
                        {
                            Console.WriteLine("Item Not found.");
                            goto invalidOptionEdit;
                        }
                        utilities.EditProduct(id_edit, productName);
                    }
                    catch
                    {
                        Console.WriteLine("Please enter a valid ID");
                        goto invalidOptionEdit;
                    }
                    return;
                case "D":
                invalidOptionDelete:
                    Console.Write("Enter Product ID to delete: ");
                    var user_selection_id_delete = Console.ReadLine();
                    int id_delete;
                    try
                    {
                        id_delete = int.Parse(user_selection_id_delete);

                        var (productId, productName) = services.CheckItemExists(id_delete);
                        if (productId == -1)
                        {
                            Console.WriteLine("Item Not found.");
                            goto invalidOptionDelete;
                        }
                        utilities.DeleteProduct(id_delete, productName);
                    }
                    catch
                    {
                        Console.WriteLine("Please enter a valid ID");
                        goto invalidOptionDelete;
                    }
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
}
