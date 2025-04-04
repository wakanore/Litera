using System.Data;
using Npgsql;
using Domain;

public class AppDbContext : IDisposable
{
    private readonly NpgsqlConnection _connection;

    public AppDbContext(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
    }

    public void ExecuteNonQuery(string sql, params NpgsqlParameter[] parameters)
    {
        using var command = new NpgsqlCommand(sql, _connection);
        command.Parameters.AddRange(parameters);
        command.ExecuteNonQuery();
    }

    public IEnumerable<Author> ExecuteQuery(string sql, params NpgsqlParameter[] parameters)
    {
        using var command = new NpgsqlCommand(sql, _connection);
        command.Parameters.AddRange(parameters);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new Author
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }
    }

    public IEnumerable<Author> GetAuthors()
    {
        return ExecuteQuery("SELECT * FROM Authors");
    }

    public void AddAuthor(Author author)
    {
        ExecuteNonQuery(
            "INSERT INTO Authors (Name, ...) VALUES (@name, ...)",
            new NpgsqlParameter("@name", author.Name)
        );
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}