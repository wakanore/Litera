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
        private readonly List<Domain.Reader> _readers = new List<Domain.Reader>();

        public void Add(Domain.Reader reader)
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