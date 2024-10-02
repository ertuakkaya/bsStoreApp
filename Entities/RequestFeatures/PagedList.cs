using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; } // Sayfalama meta verilerini tutan bir MetaData nesnesi

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData()
            {
                TotalCount = count, // Toplam öğe sayısı
                PageSize = pageSize, // Sayfa başına öğe sayısı
                CurrentPage = pageNumber, // Mevcut sayfa numarası
                TotalPage = (int)Math.Ceiling(count / (double)pageSize) // Toplam sayfa sayısı
            };
            AddRange(items); // Verilen öğeleri listeye ekle
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count(); // Kaynak koleksiyonunun toplam öğe sayısı
            var items = source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList(); // Belirtilen sayfa numarasına ve sayfa boyutuna göre öğeleri al

            return new PagedList<T>(items, count, pageNumber, pageSize); // Yeni bir PagedList nesnesi oluştur ve döndür
        }
    }
}
