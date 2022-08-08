using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.DAL;
using MyAPI.DTOs.Book;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private AppDbContext _context { get; }

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        //private List<Book> _book;

        //public BookController()
        //{
        //    _book = new List<Book>()
        //    {
        //        new Book()
        //        {
        //            Id = 1,
        //            Name = "Birinci",
        //            Price = 11.99
        //        },
        //        new Book()
        //        {
        //            Id = 2,
        //            Name = "Ikinci",
        //            Price = 12.99
        //        },
        //        new Book()
        //        {
        //            Id = 3,
        //            Name = "Ucuncu",
        //            Price = 13.99
        //        },
        //    };
        //}

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var books = _context.Books.ToList();
            if (books.Count == 0)
            {
                //return StatusCode(StatusCodes.Status404NotFound, new {errorCode="666", message="Kitab yoxdu ...("});
                return StatusCode(404, new {errorCode = 666, message = "Kitab yoxdur :("});
            }
            return Ok(books);
        }


        //[Route("{id}")]
        [HttpGet("{id}")]

        public IActionResult Get(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book is null)
            {
                //return StatusCode(404);
                //return NotFound();
                return StatusCode(404, new {errorCode = 666, message = "Bu kitab tapilmadi :("});
            }

            return Ok(book);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDTO bookDTO)
        {
            if (bookDTO is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new {errorCode = 666, message = "Kitab tapilmadi :("});
            }

            var book = new Book()
            {
                Name = bookDTO.Name,
                Price = bookDTO.Price
            };


            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateBookDTO bookDTO)
        {
            var dbBook = await _context.Books.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (dbBook is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            dbBook.Name = bookDTO.Name ?? dbBook.Name;
            dbBook.Price = bookDTO.Price == 0 ? dbBook.Price : bookDTO.Price;

            _context.Books.Update(dbBook);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var dbBook = await _context.Books.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (dbBook is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            _context.Books.Remove(dbBook);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
