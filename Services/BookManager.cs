using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System.Dynamic;

namespace Services;

public class BookManager : IBookService
{
    // Dependency Injection
    private readonly IRepositoryManager _manager;

    private readonly ILoggerService _logger;

    private readonly IMapper _mapper;

    private readonly IDataShaper<BookDto> _shaper;

    public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IDataShaper<BookDto> shaper)
    {
        _manager = manager;
        _logger = logger;
        _mapper = mapper;
        _shaper = shaper;
    }



    public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
    {
        var entitiy = _mapper.Map<Book>(bookDto);
        _manager.Book.CreateOneBook(entitiy);
        await _manager.SaveAsync();
        return _mapper.Map<BookDto>(entitiy);
    }



    // IEnumarable is a collection of items that can be enumerated
    public async Task<(IEnumerable<ExpandoObject> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges)
    {

        if (!bookParameters.ValidPriceRange)
        {
            throw new PriceOutOfRangeBadRequestExceiton(); 
        }


        var booksWithMetaData =  await _manager.Book.GetAllBookAsync(bookParameters,trackChanges);
        var booksDto =  _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

        var shapedData = _shaper.ShapeData(booksDto, bookParameters.Fields);


        return (books: shapedData, metaData: booksWithMetaData.MetaData);
    }

    /**
     * public IEnumerable<Book> GetAllBooks() {
     * @return _manager.Book.GetAllBook(false);
     * @Param trackChanges = false
     */
    public async  Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
    {
        var book =  await GetOneBookByIdAndCheckExist(id, trackChanges);
      
        return _mapper.Map<BookDto>(book);
    }

   

    public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var entity = await GetOneBookByIdAndCheckExist(id, trackChanges);
        entity = _mapper.Map<Book>(bookDto);

        _manager.Book.UpdateOneBook(entity);
        await _manager.SaveAsync();
    }

   

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
       
        var entity = await GetOneBookByIdAndCheckExist(id,trackChanges);
        _manager.Book.DeleteOneBook(entity);
        await _manager.SaveAsync();
        
    }

    public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
    {
        var book = await GetOneBookByIdAndCheckExist(id, trackChanges);
        
        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

        return (bookDtoForUpdate, book);
    }

    public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        await _manager.SaveAsync();
    }





    public async Task<Book> GetOneBookByIdAndCheckExist(int id, bool trackChanges)
    {
        // chech if book exists
        var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

        if (entity is null)
        {
            throw new BookNotFoundException(id);
        }

        return entity;
    }


}