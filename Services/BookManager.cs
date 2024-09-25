using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    // Dependency Injection
    private readonly IRepositoryManager _manager;
    
    public BookManager(IRepositoryManager manager)
    {
        _manager = manager;
    }
    
    
    // IEnumarable is a collection of items that can be enumerated
    public IEnumerable<Book> GetAllBooks(bool trackChanges)
    {
        return _manager.Book.GetAllBook(trackChanges);
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        return _manager.Book.GetOneBookById(id, trackChanges);
    }

    public Book CreateOneBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        _manager.Book.CreateOneBook(book);
        _manager.Save();
        return book;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = _manager.Book.GetOneBookById(id, trackChanges);

        if(entity is null)
            throw new Exception($"Book id:{id} not found.");
        
        // chech params
        if (book is null)
            throw new ArgumentNullException(nameof(book));
        
        
        entity.Title = book.Title;
        entity.Price = book.Price;
        
        
        _manager.Book.UpdateOneBook(entity);
        _manager.Save();
    }

   

    public void DeleteOneBook(int id, bool trackChanges)
    {
        // chech if book exists
        var entity = _manager.Book.GetOneBookById(id, trackChanges);
        
        if (entity is null)
        {
            throw new Exception($"Book id:{id} not found.");
        }
        
        _manager.Book.DeleteOneBook(entity);
        _manager.Save();
        
    }
}