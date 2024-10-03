using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{

    // Base bir request classı oluşturuyoruz. Bu classı diğer request classlarımızda kullanacağız.
    // Bu nedenle abstract olarak işaretliyoruz. Yani bu classı newleyemeyeceğiz.
    public abstract class RequestParameters
    {
        const int maxPageSize = 50; // Bir sayfada en fazla 50 item olabilir.

        public int PageNumber { get; set; }
        private int _pageSize;



        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }

        public String? OrderBy { get; set; }





    }
}
