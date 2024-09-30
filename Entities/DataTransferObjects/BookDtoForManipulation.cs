using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract record BookDtoForManipulation
    {

        [Required(ErrorMessage = "Title is a reqired field!")]
        [MinLength(2, ErrorMessage = "Title must consist of at least 2 characters!")] // başlık en az 2 karakter olmalı
        [MaxLength(50, ErrorMessage = "Title must consist of at meximum 50 characters!")] // başlık en fazla 50 karakter olmalı
        public String Title { get; init; }



        [Required(ErrorMessage = "Price is a reqired field!")]

        [Range(10, 1000)] // fiyat 10 ile 1000 arasında olmalı
        public decimal Price { get; init; }




    }
}
