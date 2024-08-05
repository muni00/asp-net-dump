using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // method parameter tarafındaki [From...(Name = "..")] ifadeleri değerin , requestin neresinden geleceğini belirtir
        //200 = Success
        //201 = created
        //404 = not found
        //400 = bad request
        //409 = conflict
        //204 = No content
        //415 = unsupported media types
        
        #region GET
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBooks([FromRoute(Name = "id")]int id)
        {
            var book = ApplicationContext
                .Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();
            //bu sorgunun adı LINQ sorgusudur (dil entegre sorgu ifadesi diye geçer) , single or default (tek değer ya da  default değer döndürür)

            if (book is null)
            {
                return NotFound(); //404 hatası
            }
            return Ok(book); //200
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult GetOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); //400 Bad Request
                }

                ApplicationContext.Books.Add(book);
                return StatusCode(201, book); //Created 

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //oluşan hata bu şekilde kullanıcıya iletilir
            }
        }

        #endregion

        #region PUT
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")]int id , [FromBody] Book book)
        {
            //kitap varlık kontrolü
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id)); //id si verilen id ye eşit olan bir kitap mevcut mu sorgu ifadesi    
            //tek ifade olduğundan ve nullable olduğundan sinle or default a gerek yok

            if(entity is null)
                return NotFound(); //404
            if (id != book.Id)
                return BadRequest(); //400

            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);

            return Ok(book); //200
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBooks([FromRoute(Name = "id")]int id)
        {
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
            {
                return NotFound(
                    new
                    {
                        StatusCode = 404,
                        Message = $"id değeri ({id}) olan bir kitap nesnesi bulunamadı."
                    }); //şeklinde de hata oluşturulabilir
            }
            ApplicationContext.Books.Remove(entity);
            return NoContent(); //204
        }
        #endregion


        #region PACTH
        [HttpPatch("{id:int")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch) 
        {
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
                return NotFound();
            bookPatch.ApplyTo(entity);
            return NoContent();
            
        }
        #endregion
    }
}
