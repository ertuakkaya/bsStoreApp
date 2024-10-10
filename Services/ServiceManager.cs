using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public  class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<IAuthenticationService> _authenticationService;




        /**
         *  Lazy<T> sınıfı, bir değeri yalnızca ihtiyaç duyulduğunda oluşturmak için kullanılır. 
         */
        public ServiceManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper, IBookLinks bookLinks,IConfiguration configuration,UserManager<User> userManager)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, logger, mapper , bookLinks));


            _authenticationService = new Lazy<IAuthenticationService>(() =>
            
                new AuthenticatonManager(logger, mapper, configuration, userManager));
        }


        public IBookService BookService => _bookService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
