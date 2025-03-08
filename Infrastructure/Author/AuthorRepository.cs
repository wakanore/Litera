using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly List<Domain.Author> _authors = new List<Domain.Author>();
        public void Add(Domain.Author author)
        {
            if (_authors.Any(a => a.Id == author.Id))
            {
                throw new InvalidOperationException("Author with the same ID already exists.");
            }

            _authors.Add(author);
        }

        public void Update(Domain.Author author)
        {
            var existingAuthor = _authors.FirstOrDefault(a => a.Id == author.Id);
            if (existingAuthor == null)
            {
                throw new InvalidOperationException("Author not found.");
            }

            // Обновляем данные автора
            existingAuthor.Name = author.Name;
            existingAuthor.Description = author.Description;
        }

        public void Delete(int id)
        {
            var authorToDelete = _authors.FirstOrDefault(a => a.Id == id);
            if (authorToDelete == null)
            {
                throw new InvalidOperationException("Author not found.");
            }

            _authors.Remove(authorToDelete);
        }
        public Domain.Author GetById(int id)
        {
            var author = _authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                throw new InvalidOperationException("Author not found.");
            }

            return author;
        }
        public IEnumerable<Domain.Author> GetAll()
        {
            return _authors; // Возвращаем всех авторов
        }
    }
}