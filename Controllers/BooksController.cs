using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly List<Book> _books;

    public BooksController(List<Book> books) => _books = books;

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll() => Ok(_books);

    [HttpGet("{id:int}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return book is null ? NotFound($"Book {id} not found.") : Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> Create([FromBody] Book book)
    {
        book.Id = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
        book.IsAvailable = true;
        _books.Add(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Book updated)
    {
        var existing = _books.FirstOrDefault(b => b.Id == id);
        if (existing is null) return NotFound($"Book {id} not found.");
        existing.Title       = updated.Title;
        existing.Author      = updated.Author;
        existing.ISBN        = updated.ISBN;
        existing.IsAvailable = updated.IsAvailable;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound($"Book {id} not found.");
        _books.Remove(book);
        return NoContent();
    }
}
