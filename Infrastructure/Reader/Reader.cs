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
    }

    public class ReaderRepository : IReaderRepository
    {

        public void Add(Domain.Reader reader) { }

        public void Update(Domain.Reader reader) { }

        public void Delete(int id) { }
    }

}