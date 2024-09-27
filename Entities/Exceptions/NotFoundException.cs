
namespace Entities.Exceptions
{


    // abstract classlar newlenemezler. Yani bu class'tan bir nesne oluşturulamaz.
    public abstract class NotFoundException : Exception
    {

        protected NotFoundException(string message ) : base(message)
        {
            
        }




    }

}
