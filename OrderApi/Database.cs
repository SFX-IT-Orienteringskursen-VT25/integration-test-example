using Microsoft.Data.SqlClient;

namespace OrderApi;

/// <summary>
/// For the record.
///
/// This is not good code, nor an example of how to write a data access layer.
/// This is a quick hack that serves the purpose of having a smaller amount of code and no real dependencies.
///
/// Generally things should be setup with proper repositories using dependency injection. Static should be avoided.
/// </summary>
public static class Database
{
    private const string TableName = "Orders";
    private const string DbName = "OrderDb";

    public static void Setup()
    {
        using var sqlConnection = CreateConnection();
        using var createDbCommand = sqlConnection.CreateCommand();
        createDbCommand.CommandText = $"IF DB_ID('{DbName}') IS NULL CREATE DATABASE {DbName};";
        createDbCommand.ExecuteNonQuery();

        using var createTableCommand = sqlConnection.CreateCommand();
        createTableCommand.CommandText = $@"
        USE {DbName};
        IF OBJECT_ID(N'{TableName}', N'U') IS NULL
        BEGIN
            CREATE TABLE {TableName} (
                [Id] INT PRIMARY KEY IDENTITY,
                [Item] VARCHAR(50) NOT NULL,
                [Quantity] INT NOT NULL,
            );
        END";
        createTableCommand.ExecuteNonQuery();
    }

    public static void CheckIfDatabaseIsReady()
    {
        using var sqlConnection = CreateConnection();
        using var healthCheckCommand = new SqlCommand("SELECT 1", sqlConnection);
        healthCheckCommand.ExecuteScalarAsync();
    }

    public static int InsertOrder(string item, int quantity)
    {
        using var sqlConnection = CreateConnection();
        using var insertCommand = sqlConnection.CreateCommand();
        insertCommand.CommandText = $@"
        USE {DbName};
        INSERT INTO {TableName} ([Item], [Quantity]) 
        VALUES (@item, @quantity);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        insertCommand.Parameters.AddWithValue("@item", item);
        insertCommand.Parameters.AddWithValue("@quantity", quantity);

        var insertedId = (int)insertCommand.ExecuteScalar();
        return insertedId;
    }

    public static Order? Select(int id)
    {
        using var sqlConnection = CreateConnection();
        using var selectCommand = sqlConnection.CreateCommand();
        selectCommand.CommandText = $@"
        USE {DbName};
        SELECT * FROM {TableName} WHERE Id = @id;";
        selectCommand.Parameters.AddWithValue("@id", id);

        using var reader = selectCommand.ExecuteReader();

        while (reader.Read())
        {
            return new Order(
                Convert.ToInt32(reader["Id"]),
                reader["Item"].ToString()!,
                Convert.ToInt32(reader["Quantity"]));
        }

        return null;
    }

    public static void DeleteAll()
    {
        using var sqlConnection = CreateConnection();
        using var deleteAllCommand = sqlConnection.CreateCommand();
        deleteAllCommand.CommandText = $@"
        USE {DbName};
        DELETE FROM {TableName};";
        deleteAllCommand.ExecuteNonQuery();
    }

    private static SqlConnection CreateConnection()
    {
        var sqlConnection = new SqlConnection($"Server=localhost,1433;Database=master;User Id=sa;Password={SqlCredentials.Password};TrustServerCertificate=True;");
        sqlConnection.Open();

        return sqlConnection;
    }
}