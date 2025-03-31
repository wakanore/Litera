using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Infrastructure
{
    public class AuthorPostgresRepository : IAuthorRepository
    {
        private readonly IDbConnection _db;

        public AuthorPostgresRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Author> Add(Author author)
        {
            const string sql = @"
                INSERT INTO Authors (Name, Description, Phone)
                VALUES (@Name, @Description, @Phone)
                RETURNING Id, Name, Description, Phone;";

            var insertedAuthor = await _db.QuerySingleAsync<Author>(sql, author);
            return insertedAuthor;
        }

        public async Task Update(Author author)
        {
            const string sql = @"
                UPDATE Authors 
                SET Name = @Name, 
                    Description = @Description,
                    Phone = @Phone
                WHERE Id = @Id;";

            var affectedRows = await _db.ExecuteAsync(sql, author);
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Author not found.");
            }
        }

        public async Task Delete(int id)
        {
            const string sql = "DELETE FROM Authors WHERE Id = @Id;";

            var affectedRows = await _db.ExecuteAsync(sql, new { Id = id });
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Author not found.");
            }
        }

        public async Task<Author> GetById(int id)
        {
            const string sql = "SELECT id, name, phone, description FROM Authors WHERE Id = @Id;";

            var author = await _db.QuerySingleOrDefaultAsync<Author>(sql, new { Id = id });
            if (author == null)
            {
                throw new InvalidOperationException("Author not found.");
            }

            return author;
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            const string sql = "SELECT id, name, phone, description FROM Authors ORDER BY Name;";
            return await _db.QueryAsync<Author>(sql);
        }

        public async Task InitializeData()
        {
            // Проверяем, есть ли уже данные в таблице
            var count = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Authors;");
            if (count > 0) return;

            var initialAuthors = new List<Author>
            {
                new Author { Name = "Tolstoi", Phone = "+79104837659" },
                new Author { Name = "Dostoevski", Phone = "+79214887395" },
                new Author { Name = "Chekhov", Phone = "+79304857612" },
                new Author { Name = "Pushkin", Phone = "+79414827364" },
                new Author { Name = "Gogol", Phone = "+79504897653" },
                new Author { Name = "Turgenev", Phone = "+79614857391" }
            };

            foreach (var author in initialAuthors)
            {
                await Add(author);
            }
        }
    }
}