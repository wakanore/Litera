using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IFavouriteRepository
    {
        void Add(Domain.Favourite book);
        void Delete(int id);
        IEnumerable<Domain.Favourite> GetAll();
    }
}