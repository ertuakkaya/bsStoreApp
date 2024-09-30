using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public record BookDtoForUpdate : BookDtoForManipulation
    {
        [Required]
        public int Id { get; init; }
    }

}
