using Microsoft.Data.SqlClient;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace ConsoleApp6;

internal class Services
{
    public static string GetConnectionString() =>
        "Your database here";

    public (int, string) CheckItemExists(int id)
    {
        int productID = -1;
        string productName = string.Empty;

        using SqlConnection connection = new(GetConnectionString());
        connection.Open();

        var checkItem = $"SELECT * FROM Product WHERE ID = @ID";

        using SqlCommand command = new(checkItem, connection);
        command.Parameters.AddWithValue("@ID", id);
        using SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                productID = reader.GetInt32(0);
                productName = reader.GetString(1);
                return (productID, productName);
            }

        }
        else
        {
            return (productID, "ProductID not found. Try again.");
        }

        return (productID, "ProductID not found. Try again.");
    }

    public string AddProductQuery(string name, string description, string quantity, string isAvailable, string isOnSale, Guid guid)
    {
        var addProductQuery = $"INSERT INTO PRODUCT (Name, Description, Quantity, IsAvailable, IsOnSale, SKU)" +
            $"VALUES (@Name, @Description, @Quantity, @IsAvailable, @IsOnSale, @SKU)";

        using SqlConnection connection = new(GetConnectionString());
        connection.Open();

        using SqlCommand command = new(addProductQuery, connection);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Description", description);
        command.Parameters.AddWithValue("@Quantity", quantity);
        command.Parameters.AddWithValue("@IsAvailable", isAvailable);
        command.Parameters.AddWithValue("@IsOnSale", isOnSale);
        command.Parameters.AddWithValue("@SKU", guid);

        int rowsAffected = command.ExecuteNonQuery();

        return $"Row(s) Affected: {rowsAffected}";
    }

    public void ViewAllProducts()
    {
        var viewQuery = "SELECT * FROM PRODUCT";
        var countQuery = "SELECT COUNT(*) FROM PRODUCT";
        try
        {
            using SqlConnection connection = new(GetConnectionString());
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

            for (int i = 0; i < lineBreak.Length; i++) Console.Write("_");
            Console.WriteLine("\n" + lineBreak);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void EditProductQuery(int id, string columnToEdit, string newValue)
    {

        using SqlConnection connection = new(GetConnectionString());
        connection.Open();

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
    }
    public string DeleteProduct(int id)
    {
        var deleteQuery = "DELETE FROM PRODUCT WHERE ID = @ID";

        using SqlConnection connection = new(GetConnectionString());
        connection.Open();

        using SqlTransaction transaction = connection.BeginTransaction();
        using SqlCommand command = new(deleteQuery, connection, transaction);
        command.Parameters.AddWithValue("@ID", id);


        Console.Write("Are you sure you want to delete? (Yes or No): ");
        var user_response = Console.ReadLine();
        if (user_response.ToUpper() == "YES")
        {
            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
                return "Product deleted successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
            }
        }
        else
        {
            transaction.Rollback();
            return "Deletion Cancelled.";
        }

        return string.Empty;
    }
}
