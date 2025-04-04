using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace Infrastructure
{
    public class ReaderPostgresRepository : IReaderRepository
    {
        private readonly IDbConnection _db;

        public ReaderPostgresRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<Reader> Add(Reader reader)
        {
            const string sql = @"
                INSERT INTO Readers (Name, Phone, Description)
                VALUES (@Name, @Phone, @Description)
                RETURNING Id, Name, Phone, Description;";

            var insertedReader = await _db.QuerySingleAsync<Reader>(sql, reader);
            return insertedReader;
        }

        public async Task<bool> Update(Reader reader)
        {
            const string sql = @"
        UPDATE Readers 
        SET Name = @Name, 
            Phone = @Phone,
            Description = @Description
        WHERE Id = @Id;";

            var affectedRows = await _db.ExecuteAsync(sql, reader);
            return affectedRows > 0;
        }

        public async Task Delete(int id)
        {
            const string sql = "DELETE FROM Readers WHERE Id = @Id;";

            var affectedRows = await _db.ExecuteAsync(sql, new { Id = id });
            if (affectedRows == 0)
            {
                throw new InvalidOperationException("Reader not found.");
            }
        }

        public async Task<Reader> GetById(int id)
        {
            const string sql = @"
                SELECT *
                FROM Readers
                WHERE Id = @Id;";

            var reader = await _db.QueryFirstOrDefaultAsync<Reader>(sql, new { Id = id });

            return reader ?? throw new InvalidOperationException("Reader not found.");
        }

        public async Task<IEnumerable<Reader>> GetAll()
        {
            const string sql = "SELECT name, phone FROM Readers ORDER BY Name";
            return await _db.QueryAsync<Reader>(sql);
        }

        public async Task InitializeData()
        {
            var count = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Readers;");
            if (count > 0) return;

            var initialReaders = new List<Reader>
            {
                new Reader { Name = "Иван Иванов", Phone = "+79111234567", Description = "Активный читатель" },
                new Reader { Name = "Петр Петров", Phone = "+79217654321", Description = "Любит классику" },
                new Reader { Name = "Мария Сидорова", Phone = "+79316543278", Description = "Предпочитает современную литературу" }
            };

            foreach (var reader in initialReaders)
            {
                await Add(reader);
            }
        }
    }
}