using BookShop.BookManager;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookShopController : ControllerBase
    {
        private readonly BookFileHandler BookManager;
        public BookShopController(BookFileHandler BookManager)
        {
            this.BookManager = BookManager;
        }

        [HttpGet("GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            return Ok(BookManager.GetBooks());
        }

        [HttpGet("GetBookDataByBookId/{bookId}")]
        public IActionResult GetBookData(ulong bookId)
        {
            return Ok(BookManager.GetBookDataByBookId(bookId));
        }

        [HttpPost("AddBook")]
        public IActionResult Post([FromBody] Book newBook)
        {
            OperationResult result =  BookManager.AddNewBook(newBook);
            return Ok(result);
        }

        [HttpPut("EditBook/{bookId}")]
        public IActionResult Put(ulong bookId, [FromBody] Book book)
        {
            OperationResult result = BookManager.EditBook(bookId, book);
            return Ok(result);
        }

        [HttpDelete("DeleteBook/{bookId}")]
        public IActionResult Delete(ulong bookId)
        {
            OperationResult result = BookManager.DeleteBookById(bookId);
            return Ok(result);
        }
    }
}
