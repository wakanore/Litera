//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Domain;

//namespace Infrastructure
//{
//    public class AuthorInMemoryRepository : IAuthorRepository
//    {
//        private readonly List<Author> _authors = new List<Author>();

//        public AuthorInMemoryRepository()
//        {
//            _authors.Add(new Author { Id = 1, Name = "Tolstoi", Phone = "+79104837659" });
//            _authors.Add(new Author { Id = 2, Name = "Dostoevski", Phone = "+79214887395" });
//            _authors.Add(new Author { Id = 3, Name = "Chekhov", Phone = "+79304857612" });
//            _authors.Add(new Author { Id = 4, Name = "Pushkin", Phone = "+79414827364" });
//            _authors.Add(new Author { Id = 5, Name = "Gogol", Phone = "+79504897653" });
//            _authors.Add(new Author { Id = 6, Name = "Turgenev", Phone = "+79614857391" });

//        }

//        public Author Add(Author author)
//        {
//            author.Id = _authors.Any() ? _authors.Max(a => a.Id) + 1 : 1;
//            _authors.Add(author);
//            return author;
//        }

//        public void Update(Author author)
//        {
//            var existingAuthor = _authors.FirstOrDefault(a => a.Id == author.Id);
//            if (existingAuthor == null)
//            {
//                throw new InvalidOperationException("Author not found.");
//            }

//            // Обновляем данные автора
//            existingAuthor.Name = author.Name;
//            existingAuthor.Description = author.Description;
//        }

//        public void Delete(int id)
//        {
//            var authorToDelete = _authors.FirstOrDefault(a => a.Id == id);
//            if (authorToDelete == null)
//            {
//                throw new InvalidOperationException("Author not found.");
//            }
//            _authors.Remove(authorToDelete);
//        }

//        public Author GetById(int id)
//        {
//            var author = _authors.FirstOrDefault(a => a.Id == id);
//            if (author == null)
//            {
//                throw new InvalidOperationException("Author not found.");
//            }

//            return author;
//        }

//        public IEnumerable<Author> GetAll()
//        {
//            return _authors; // Возвращаем всех авторов
//        }
//    }
//}