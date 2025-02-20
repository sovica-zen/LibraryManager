using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using System.Diagnostics;
using SQLitePCL;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LibraryManagementSystem.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int lastID = 0, int pageSize = 10)
        {
            
            var books =  await _context.Books
                .OrderBy(b=>b.ID)
                .Where(b=>b.ID >= lastID)
                .Take(pageSize)
                .Include(b => b.Author)
                .ToListAsync();
            var next = lastID + pageSize;
            Response.Headers.Append("Link", 
                String.Format("https://localhost:7078/books?lastID={0}&pageSize={1}",next, pageSize));
            return books;
        }


        // PUT: books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            var author = await _context.Authors.FindAsync(book.AuthorId);
            if (author == null) {
                return BadRequest("author doesn't exist in database");
            }
            if (book.PublicationYear > DateTime.Now.Year) {
                ModelState.AddModelError("invalid date", "date of publication cannot be in the future");
                return BadRequest(ModelState);
            }
            book.ID = null;
            book.Author = author;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooks", new { id = book.ID }, book);
        }

        // DELETE: books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksSearch(string query)//do similarity search
        {
            //var books = await _context.Books.Where(b => b.Title.Equals(query)).ToListAsync();

            var books = await _context.Books.ToListAsync();
            List<Book> similar = [];

            foreach (var b in books) {
                Console.WriteLine(Levenstein(b.Title, query));
                if (Levenstein(b.Title, query) <= 1) {
                    similar.Add(b);
                }
            }

            if (books == null || similar == null)
            {
                return NotFound();
            }

            if (books.Count == 0) {
                return NoContent();
            }

            return similar;
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }

        //private List<string> SimilarStrings(string original) {
        //    List<string> modified = [];
        //    for(int i=0; i<original.Length; i++) {
        //        modified.Add(original.Remove(i, 1));
        //    }

        //    return modified;
        //}

        //https://en.wikipedia.org/wiki/Levenshtein_distance#Definition
        private int Levenstein(string first, string second) {
            List<string> modified = [];
            if (first.Length == 0) return second.Length;
            if (second.Length == 0) return first.Length;
            if (first[0].ToString().ToLower() == second[0].ToString().ToLower()) {
                return Levenstein(first[1..], second[1..]);
            }
            else {
                List<int> temp = [Levenstein(first[1..], second), Levenstein(first, second[1..]), Levenstein(first[1..], second[1..])];
                return 1 + temp.Min();
            }

        }
    }
}
