using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{


    /**
     * 
     * DTO'lar read-only olmalıdır. DTO'lar sadece veri taşımak için kullanılır.
     * immutable olmalıdır. Yani DTO'lar bir kez oluşturulduktan sonra değiştirilemez.
     * LINQ sorguları için kullanılabilir.
     * 
     * 
     * 
     */
    public record BookDtoForUpdate(int Id, String Title, decimal Price);

}
