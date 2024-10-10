using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class RefreshTokenBadRequestExcepiton : BadRequestException
    {

        public RefreshTokenBadRequestExcepiton() : base($"Invalid client request. The tokenDto has some invalid values")
        {
            
        }
    }
}
