using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        // constructor
        public BookRepository(RepositoryContext context) : base(context)
        {

        }



        public IQueryable<Book> GetAllBook(bool trackChanges)
        {
            return FindAll(trackChanges)
                .OrderBy(b => b.Id);
        }


        public Book GetOneBookById(int id, bool trackChanges) =>
            FindByCondition(b => b.Id.Equals(id), trackChanges)
                .SingleOrDefault();
        

        public void CreateOneBook(Book book) => Create(book);
       

        public void UpdateOneBook(Book book) => Update(book);
        

        public void DeleteOneBook(Book book) => Delete(book);
        
    }
}
