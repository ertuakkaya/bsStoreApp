using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers
{

    /**
     * 
     * [ActionFilter] -> Bir action'ın çalışmasını engelleyebilir, bir action'ın çalışmasından önce veya sonra bir işlem yapabiliriz.
     * 
     * 
     */

    //[ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))] // tüm metotları loglamak için
    [ApiController]
    [Route("api/books")]
    //[ResponseCache(CacheProfileName = "5mins")] // Controllerdaki tüm metotlar için 5 dakika boyunca cache'de tutar.
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 80)]
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



        // ...
        [Authorize]
        [HttpHead]
        [HttpGet(Name = "GetAllBooksAsync")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        //[ResponseCache(Duration = 60)] // 60 sanıye boyunca cache'de tutar.
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
        {

            var linkedParameters = new LinkParameters
            {
               BookParameters = bookParameters,
               HttpContext = HttpContext
            };



            var result = await _manager.BookService.GetAllBooksAsync(linkedParameters, false);

            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);


        }




        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {

            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

            return Ok(book);


        }


        [Authorize(Roles = "Editor, Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))] // Uygulama çalışmadan önce ValidationFilterAttribute sınıfını çalıştırır.
        [HttpPost(Name = "CreateOneBookAsync")]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {

      
            var book = await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, book);


        }


        [Authorize(Roles = "Editor, Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {


            await _manager.BookService.UpdateOneBookAsync(id, bookDto, true);

            return NoContent();

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {


            await _manager.BookService.DeleteOneBookAsync(id, false);

            return NoContent();


        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBook(
            
            [FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch
            

        )
        {

            if (bookPatch is null)
            {
                return BadRequest();
            }

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);


            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }


            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);


            return NoContent(); // 204


        }



        [Authorize]
        [HttpOptions]
        public IActionResult GetBooksOptions()
        {

            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, PATCH, HEAD, OPTIONS");

            return Ok();

        }

    }
}
