using AutoMapper;
using Entities.DataTransferObjects;
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

    private readonly IMapper _mapper;

    public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
    {
        _manager = manager;
        _logger = logger;
        _mapper = mapper;
    }



    public Book CreateOneBook(Book book)
    {
        
        _manager.Book.CreateOneBook(book);
        _manager.Save();
        return book;
    }



    // IEnumarable is a collection of items that can be enumerated
    public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
    {
        var books =  _manager.Book.GetAllBook(trackChanges);
        return _mapper.Map<IEnumerable<BookDto>>(books);
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

   

    public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var entity = _manager.Book.GetOneBookById(id, trackChanges);

        if(entity is null)
        {
            throw new BookNotFoundException(id);
        }
            
        
        //// chech params
        //if (book is null)
        //    throw new ArgumentNullException(nameof(book));
        
        entity = _mapper.Map<Book>(bookDto);


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