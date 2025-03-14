using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly List<Reader> _readers = new List<Reader>();

        public ReaderRepository()
        {
            _readers.Add(new Reader { Id = 1, Name = "Ivan", Phone = "+79103775676" });
            _readers.Add(new Reader { Id = 2, Name = "Alice", Phone = "+79101124521" });
            _readers.Add(new Reader { Id = 3, Name = "Sergey", Phone = "+79213456789" });
            _readers.Add(new Reader { Id = 4, Name = "Olga", Phone = "+79219876543" });
            _readers.Add(new Reader { Id = 5, Name = "Dmitry", Phone = "+79307654321" });
            _readers.Add(new Reader { Id = 6, Name = "Elena", Phone = "+79301234567" });
            _readers.Add(new Reader { Id = 7, Name = "Alexey", Phone = "+79405678901" });
        }

        public void Add(Reader reader)
        {
            if (_readers.Any(r => r.Id == reader.Id))
            {
                throw new InvalidOperationException("Reader with the same ID already exists.");
            }

            _readers.Add(reader);
        }

        public void Update(Domain.Reader reader)
        {
            var existingReader = _readers.FirstOrDefault(r => r.Id == reader.Id);
            if (existingReader == null)
            {
                throw new InvalidOperationException("Reader not found.");
            }


            existingReader.Name = reader.Name;
            existingReader.Phone = reader.Phone;
        }

        public void Delete(int id)
        {
            var readerToDelete = _readers.FirstOrDefault(r => r.Id == id);
            if (readerToDelete == null)
            {
                throw new InvalidOperationException("Reader not found.");
            }

            _readers.Remove(readerToDelete);
        }
        public Domain.Reader GetById(int id)
        {
            var reader = _readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
            {
                throw new InvalidOperationException("Reader not found.");
            }

            return reader;
        }

        public IEnumerable<Domain.Reader> GetAll()
        {
            return _readers; // Возвращаем всех читателей
        }
    }

}