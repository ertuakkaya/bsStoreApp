using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public  class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;



        /**
         *  Lazy<T> sınıfı, bir değeri yalnızca ihtiyaç duyulduğunda oluşturmak için kullanılır. 
         */
        public ServiceManager(IRepositoryManager repositoryManager, ILoggerService logger , IMapper mapper)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager , logger , mapper));
        }


        public IBookService BookService => _bookService.Value;
    }
}
