using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {



        /**
         * Üst katman olan Service katmanına erişmek için IServiceManager interface'ini kullanıyoruz.
         */
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                // trackChanges = false, veritabanında bir değişiklik yapmayacağımız için false olarak belirliyoruz ve performansı arttırıyoruz.
                var books = _manager.BookService.GetAllBooks(false);

                return Ok(books);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _manager.BookService.GetOneBookById(id, false);


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
            try
            {
                if (book == null)
                {
                    return BadRequest("Book is null");
                }

                _manager.BookService.CreateOneBook(book);

                return StatusCode(201, book);
            }
            catch (Exception e)
            {
                // Log the inner exception message
                var innerExceptionMessage = e.InnerException?.Message ?? e.Message;
                return StatusCode(500, $"An error occurred: {innerExceptionMessage}");
            }
        }



        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {

                // 
                if (book is null)
                {
                    return BadRequest(); // 400
                }

                _manager.BookService.UpdateOneBook(id, book, true);


                return NoContent();
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

                _manager.BookService.DeleteOneBook(id, false);

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
                var entity = _manager.BookService.GetOneBookById(id, true);



                if (entity == null)
                    return NotFound(); // 404

                bookPatch.ApplyTo(entity);
                _manager.BookService.UpdateOneBook(id, entity, true);

                return NoContent(); // 204
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
