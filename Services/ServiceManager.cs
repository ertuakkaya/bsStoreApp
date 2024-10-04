using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
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
        public ServiceManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper, IBookLinks bookLinks)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, logger, mapper , bookLinks));
        }


        public IBookService BookService => _bookService.Value;
    }
}
