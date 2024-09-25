using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {

        
        // Dependency Injection
        private readonly RepositoryContext _context;
        
        // Lazy Loading
        private readonly Lazy<IBookRepository> _bookRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(_context));
        }
        // Dependency Injection



        public IBookRepository Book => _bookRepository.Value;
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
