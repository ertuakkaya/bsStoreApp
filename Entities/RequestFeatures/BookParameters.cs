namespace Entities.RequestFeatures
{


    /**
     * 
     * Kitap Parametreleri sınıfı RequestParameters sınıfından türetilmiştir.
     * Request Parameters sınıfı içerisinde sayfa numarası ve sayfa boyutu gibi parametreler bulunmaktadır.
     * 
     * Kıtap sınıfına özgü parametreler bu sınıf içerisinde tanımlanmıştır.
     * 
     * 
     */
    public class BookParameters : RequestParameters
    {


        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = 1000;
        public bool ValidPriceRange => MaxPrice > MinPrice;




    }
}
