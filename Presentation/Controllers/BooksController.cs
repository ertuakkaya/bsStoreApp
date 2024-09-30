using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Exceptions;
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

            // trackChanges = false, veritabanında bir değişiklik yapmayacağımız için false olarak belirliyoruz ve performansı arttırıyoruz.
            var books = _manager.BookService.GetAllBooks(false);

            return Ok(books);


        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {

            var book = _manager.BookService.GetOneBookById(id, false);

            return Ok(book);


        }


        [HttpPost]
        public IActionResult CreateOneBook([FromBody] BookDtoForInsertion bookDto)
        {

            if (bookDto == null)
            {
                return BadRequest("Book is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var book = _manager.BookService.CreateOneBook(bookDto);

            return StatusCode(201, book);


        }



        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {



            if (bookDto is null)
            {
                return BadRequest(); // 400
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }


            _manager.BookService.UpdateOneBook(id, bookDto, true);


            return NoContent();


        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {


            _manager.BookService.DeleteOneBook(id, false);

            return NoContent();


        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook(
            
            [FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch
            

        ){

            if (bookPatch is null)
            {
                return BadRequest();
            }

            var result = _manager.BookService.GetOneBookForPatch(id, false);


            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }


            _manager.BookService.SaveChangesForPatch(result.bookDtoForUpdate, result.book);


            return NoContent(); // 204


        }

    }
}
