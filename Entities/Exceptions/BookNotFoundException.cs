namespace Entities.Exceptions
{
    // sealed classlar inherit edilemezler. Yani bu class'tan başka bir class inherit edilemez.
    public sealed class BookNotFoundException : NotFoundException
    {
        public BookNotFoundException(int id) : base($"The book with id: {id} could not found!")
        {
            
        }
    }

}
