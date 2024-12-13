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
                        AddProduct(GetConnectionString());
                        return;
                    case "B":
                        ViewProducts(GetConnectionString());
                        return;
                    case "C":
                        EditProduct(GetConnectionString());
                        return;
                    case "D":
                        DeleteProduct(GetConnectionString());
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
            var title = "Adding A New Product (press r to return)";
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

                Console.Write("Description: ");
                description = Console.ReadLine();

                Console.Write("Quantity: ");
                quantity = Console.ReadLine();

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

                if(isAvailable.ToUpper() != "YES" && isAvailable.ToUpper() != "NO")
                {
                    Console.WriteLine("Invalid selection.  Please Type Yes or No.");
                    isAvailable = string.Empty;
                    Console.Write("Available: ");
                    isAvailable = Console.ReadLine();
                } 

                Console.Write("Sale (Yes or No): ");
                isOnSale = Console.ReadLine();

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

        private static string GetConnectionString()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Management;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            return connectionString;
        }

        static void ViewProducts(string connectionString)
        {
            var title = "Viewing All Products (press r to return)";
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

                Console.WriteLine($"Count of results in Product Table: {count}");
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

        static void EditProduct(string connectionString)
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

        static void DeleteProduct(string connectionString)
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
