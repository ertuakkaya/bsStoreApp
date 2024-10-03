using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDataShaper<T> 
    {
        // kaynaktaki hangi alanları seçiyorsak onu liste olarak döndür
        IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString);


        // Tek bir nesne döndürmek için
        ExpandoObject ShapeData(T entity, string fieldsString);



    }
}
