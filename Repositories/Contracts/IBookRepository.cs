using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Repositories.Contracts
{


    /**
     * 
     * Thredlerde asenkron işlemler yapabilmek için Task sınıfını kullanıyoruz.
     * Task<T> şeklinde tanımladığımızda geriye dönüş değeri olan metotlar için kullanıyoruz.
     * 
     */
    public interface IBookRepository : IRepositoryBase<Book>
    {

        Task<IEnumerable<Book>> GetAllBookAsync(bool trackChanges);

        Task<Book> GetOneBookByIdAsync(int id, bool trackChanges);

        
        // 
        void CreateOneBook(Book book);

        void UpdateOneBook(Book book);

        void DeleteOneBook(Book book);



    }
}
