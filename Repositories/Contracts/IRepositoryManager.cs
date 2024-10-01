using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {


        IBookRepository Book { get; }


        // metot void olduğu için geriye dönüş yapmıyoruz. Sadece Task SaveAsync() şeklinde tanımlıyoruz.
        Task SaveAsync();


    }
}
