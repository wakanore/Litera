//using Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Infrastructure
//{
//    public class ReaderInMemoryRepository : IReaderRepository
//    {
//        private readonly List<Reader> _readers = new List<Reader>();

//        public ReaderInMemoryRepository()
//        {
//            _readers.Add(new Reader { Id = 1, Name = "Ivan", Phone = "+79103775676" });
//            _readers.Add(new Reader { Id = 2, Name = "Alice", Phone = "+79101124521" });
//            _readers.Add(new Reader { Id = 3, Name = "Sergey", Phone = "+79213456789" });
//            _readers.Add(new Reader { Id = 4, Name = "Olga", Phone = "+79219876543" });
//            _readers.Add(new Reader { Id = 5, Name = "Dmitry", Phone = "+79307654321" });
//            _readers.Add(new Reader { Id = 6, Name = "Elena", Phone = "+79301234567" });
//            _readers.Add(new Reader { Id = 7, Name = "Alexey", Phone = "+79405678901" });
//        }

//        public Reader Add(Reader reader)
//        {
//            reader.Id = _readers.Any() ? _readers.Max(a => a.Id) + 1 : 1;
//            _readers.Add(reader);
//            return reader;
//        }

//        public void Update(Reader reader)
//        {
//            var existingReader = _readers.FirstOrDefault(r => r.Id == reader.Id);
//            if (existingReader == null)
//            {
//                throw new InvalidOperationException("Reader not found.");
//            }


//            existingReader.Name = reader.Name;
//            existingReader.Phone = reader.Phone;
//        }

//        public void Delete(int id)
//        {
//            var readerToDelete = _readers.FirstOrDefault(r => r.Id == id);
//            if (readerToDelete == null)
//            {
//                throw new InvalidOperationException("Reader not found.");
//            }

//            _readers.Remove(readerToDelete);
//        }
//        public Reader GetById(int id)
//        {
//            var reader = _readers.FirstOrDefault(r => r.Id == id);
//            if (reader == null)
//            {
//                throw new InvalidOperationException("Reader not found.");
//            }

//            return reader;
//        }

//        public IEnumerable<Reader> GetAll()
//        {
//            return _readers; // Возвращаем всех читателей
//        }
//    }

//}