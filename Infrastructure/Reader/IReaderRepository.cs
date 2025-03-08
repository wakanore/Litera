using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IReaderRepository
    {
        void Add(Domain.Reader reader);
        void Update(Domain.Reader reader);
        void Delete(int id);
        Domain.Reader GetById(int id);
        IEnumerable<Domain.Reader> GetAll();
    }
}