using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    // Dependency Injection
    private readonly IRepositoryManager _manager;

    private readonly ILoggerService _logger;

    public BookManager(IRepositoryManager manager, ILoggerService logger)
    {
        _manager = manager;
        _logger = logger;
    }



    public Book CreateOneBook(Book book)
    {
        
        _manager.Book.CreateOneBook(book);
        _manager.Save();
        return book;
    }



    // IEnumarable is a collection of items that can be enumerated
    public IEnumerable<Book> GetAllBooks(bool trackChanges)
    {
        return _manager.Book.GetAllBook(trackChanges);
    }

    /**
     * public IEnumerable<Book> GetAllBooks() {
     * @return _manager.Book.GetAllBook(false);
     * @Param trackChanges = false
     */
    public Book GetOneBookById(int id, bool trackChanges)
    {
        var book =  _manager.Book.GetOneBookById(id, trackChanges);
        if (book is null)
        {
            throw new BookNotFoundException(id);
        }
        return book;
    }

   

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = _manager.Book.GetOneBookById(id, trackChanges);

        if(entity is null)
        {
            throw new BookNotFoundException(id);
        }
            
        
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
            throw new BookNotFoundException(id);
        }
        
        _manager.Book.DeleteOneBook(entity);
        _manager.Save();
        
    }
}