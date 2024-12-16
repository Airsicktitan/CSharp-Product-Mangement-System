using Microsoft.Data.SqlClient;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }
        private static string GetConnectionString() =>
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Management;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        static void Menu()
        {
            var connectionString = GetConnectionString();
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
                        AddProduct(connectionString);
                        return;
                    case "B":
                        ViewProducts(connectionString);
                        return;
                    case "C":
                        invalidOptionEdit:
                        Console.Write("Enter Product ID to edit: ");
                        var user_selection_id_edit = Console.ReadLine();
                        int id_edit;
                        try
                        {
                            id_edit = int.Parse(user_selection_id_edit);

                            using SqlConnection connection = new(connectionString);
                            connection.Open();

                            var checkItem = $"SELECT * FROM Product WHERE ID = @ID";

                            using SqlCommand command = new(checkItem, connection);
                            command.Parameters.AddWithValue("@ID", id_edit);

                            using SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int productID = reader.GetInt32(0);
                                    string productName = reader.GetString(1);
                                    EditProduct(connectionString, productID, productName);
                                }

                            }
                            else
                            {
                                Console.WriteLine("ProductID not found. Try again.");
                                goto invalidOptionEdit;
                            }
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

                            using SqlConnection connection = new(connectionString);
                            connection.Open();

                            var checkItem = $"SELECT * FROM Product WHERE ID = @ID";

                            using SqlCommand command = new(checkItem, connection);
                            command.Parameters.AddWithValue("@ID", id_delete);

                            using SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int productID = reader.GetInt32(0);
                                    string productName = reader.GetString(1);
                                    DeleteProduct(connectionString, productID, productName);
                                }
                            }
                            else
                            {
                                Console.WriteLine("ProductID not found. Try again.");
                                goto invalidOptionDelete;
                            }
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

        static void AddProduct(string connectionString)
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
                    Menu();
                }

                Console.Write("Description: ");
                description = Console.ReadLine();

                if (description.ToLower() == "r")
                {
                    Console.Clear();
                    Menu();
                }

                Console.Write("Quantity: ");
                quantity = Console.ReadLine();

                if (quantity.ToLower() == "r")
                {
                    Console.Clear();
                    Menu();
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
                    Menu();
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
                    Menu();
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

            var addProductQuery = $"INSERT INTO PRODUCT (Name, Description, Quantity, IsAvailable, IsOnSale, SKU)" +
                $"VALUES (@Name, @Description, @Quantity, @IsAvailable, @IsOnSale, @SKU)";

            using SqlConnection connection = new(connectionString);
            connection.Open();

            using SqlCommand command = new(addProductQuery, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@IsAvailable", isAvailable);
            command.Parameters.AddWithValue("@IsOnSale", isOnSale);
            command.Parameters.AddWithValue("@SKU", guid);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"{rowsAffected} row(s) inserted.");

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

        static void ViewProducts(string connectionString)
        {
            var title = "Viewing All Products (press r anytime to return)";
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < title.Length; i++) Console.Write("_");
            Console.WriteLine();

            
            var viewQuery = "SELECT * FROM PRODUCT";
            var countQuery = "SELECT COUNT(*) FROM PRODUCT";

            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();

                using SqlCommand commandCount = new(countQuery, connection);
                object result = commandCount.ExecuteScalar();
                int count = (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);

                if (count == 0) Console.WriteLine("There are no products to view at this time");

                using SqlCommand command = new(viewQuery, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader["id"].ToString();
                    string name = reader["Name"].ToString();
                    string description = reader["Description"].ToString();
                    string quantity = reader["Quantity"].ToString();
                    string isAvailable = reader["IsAvailable"].ToString();
                    string isOnSale = reader["IsOnSale"].ToString();
                    string sku = reader["SKU"].ToString();

                    Console.WriteLine($"ID: {id} Name: {name} Description: {description} Quantity: {quantity} Available: {isAvailable} Sale: {isOnSale} Material Number: {sku}");
                    
                }

                var lineBreak = $"Count of results in Product Table: {count}";

                for(int i = 0; i < lineBreak.Length; i++) Console.Write("_");
                Console.WriteLine("\n" + lineBreak);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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

        static void EditProduct(string connectionString, int id, string name)
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

            foreach(string option in options) Console.WriteLine(option);

            using SqlConnection connection = new(connectionString);
            connection.Open();

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
                        Menu();
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
                        if(newValue != "YES" &&  newValue != "NO")
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

                if (!string.IsNullOrEmpty(columnToEdit) && !string.IsNullOrEmpty(newValue))
                {
                    var updateSQLQuery = $"UPDATE PRODUCT SET {columnToEdit} = @NewValue WHERE ID = @ID";
                    using SqlCommand command = new(updateSQLQuery, connection);
                    command.Parameters.AddWithValue("@NewValue", newValue);
                    command.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{columnToEdit} updated successfully!");
                        Console.WriteLine($"Row(s) Affected: {rowsAffected}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                Console.WriteLine("Do you want to make another edit? (Yes or No)");
                var again = Console.ReadLine()?.ToUpper();

                if (again == "NO")
                {
                    Console.Clear();
                    Menu();
                    return;
                }
                else if (again != "YES")
                {
                    Console.WriteLine("Invalid selection, returning to main menu.");
                    Console.Clear();
                    Menu();
                    return;
                }
            }
        }

        static void DeleteProduct(string connectionString, int id, string name)
        {
            var title = $"Deleting {id} - {name} (press r anytime to return)";  // To contain a product name later
            Console.Clear();
            Console.WriteLine(title);
            for (int i = 0; i < title.Length; i++) Console.Write("_");
            Console.WriteLine();


            var deleteQuery = "DELETE FROM PRODUCT WHERE ID = @ID";

            using SqlConnection connection = new(connectionString);
            connection.Open();

            using SqlTransaction transaction = connection.BeginTransaction();
            using SqlCommand command = new(deleteQuery, connection, transaction);
            command.Parameters.AddWithValue("@ID", id);


            Console.Write("Are you sure you want to delete? (Yes or No): ");
            var user_response = Console.ReadLine();
            if(user_response.ToUpper() == "YES")
            {
                try
                {
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Product deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                }
            }
            else
            {
                Console.WriteLine("Deletion cancelled: ");
                transaction.Rollback();
            }

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
