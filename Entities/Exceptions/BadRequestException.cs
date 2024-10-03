using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public  abstract class BadRequestException : Exception
    {

        // abstract class, bu sınıftan instance oluşturulamaz.
        protected BadRequestException(string message) : base(message)
        {
            
        }




    }


}
