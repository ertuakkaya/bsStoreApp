
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EFCore;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _manager.Book.GetAllBook(false);

            if (books == null || !books.Any())
            {
                return NotFound("No books found.");
            }

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id") ] int id)
        {
            try
            {
                var book = _manager
                    .Book
                    .GetOneBookById(id, false);
               

                if (book is null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }


        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book is null");
            }

            _manager.Book.CreateOneBook(book);
            _manager.Save();
           

            return StatusCode(201, book);
        }



        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                var bookToUpdate = _manager
                    .Book
                    .GetOneBookById(id, true);
                

                if (bookToUpdate == null)
                {
                    return NotFound();
                }

                bookToUpdate.Title = book.Title;
                bookToUpdate.Price = book.Price;

                _manager.Save();

                return Ok(bookToUpdate);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var bookToDelete = _manager
                    .Book
                    .GetOneBookById(id, false);

                if (bookToDelete == null)
                {
                    return NotFound();
                }

                _manager.Book.DeleteOneBook(bookToDelete);
                _manager.Save();

                return NoContent();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                var entity = _manager
                    .Book
                    .GetOneBookById(id, true);
                    

                if (entity == null)
                    return NotFound(); // 404

                bookPatch.ApplyTo(entity);
                _manager.Book.Update(entity);

                return NoContent(); // 204
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
