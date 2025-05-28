using System.Data;
using Npgsql;
using Domain;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext, IDisposable
{
    private NpgsqlConnection _connection;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public void OpenConnection()
    {
        _connection = Database.GetDbConnection() as NpgsqlConnection;
        _connection?.Open();
    }

    public void ExecuteNonQuery(string sql, params NpgsqlParameter[] parameters)
    {
        if (_connection == null)
            OpenConnection();

        using var command = new NpgsqlCommand(sql, _connection);
        command.Parameters.AddRange(parameters);
        command.ExecuteNonQuery();
    }

    public IEnumerable<Author> ExecuteQuery(string sql, params NpgsqlParameter[] parameters)
    {
        if (_connection == null)
            OpenConnection();

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

    // DbSet для работы через EF Core
    public DbSet<Author> Authors { get; set; }

    // Методы для EF Core
    public IEnumerable<Author> GetAuthors() => Authors.ToList();

    public void AddAuthor(Author author)
    {
        Authors.Add(author);
        SaveChanges();
    }

    public void Dispose()
    {
        _connection?.Dispose();
        base.Dispose();
    }
}