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



    public BookDto CreateOneBook(BookDtoForInsertion bookDto)
    {
        var entitiy = _mapper.Map<Book>(bookDto);
        _manager.Book.CreateOneBook(entitiy);
        _manager.Save();
        return _mapper.Map<BookDto>(entitiy);
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
    public BookDto GetOneBookById(int id, bool trackChanges)
    {
        var book =  _manager.Book.GetOneBookById(id, trackChanges);
        if (book is null)
        {
            throw new BookNotFoundException(id);
        }
        return _mapper.Map<BookDto>(book);
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

    public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
    {
        var book = _manager.Book.GetOneBookById(id, trackChanges);
        if (book is null)
        {
            throw new BookNotFoundException(id);
        }

        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

        return (bookDtoForUpdate, book);
    }

    public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        _manager.Save();
    }
}