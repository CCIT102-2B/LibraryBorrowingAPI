using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowsController : ControllerBase
{
    private readonly List<Book>         _books;
    private readonly List<Member>       _members;
    private readonly List<BorrowRecord> _records;

    public BorrowsController(List<Book> books, List<Member> members, List<BorrowRecord> records)
    {
        _books   = books;
        _members = members;
        _records = records;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BorrowRecord>> GetAll() => Ok(_records);

    [HttpPost]
    public ActionResult<BorrowRecord> Borrow([FromBody] BorrowRequest request)
    {
        var book = _books.FirstOrDefault(b => b.Id == request.BookId);
        if (book is null)      return NotFound($"Book {request.BookId} not found.");
        if (!book.IsAvailable) return BadRequest($"Book '{book.Title}' is not available.");

        var member = _members.FirstOrDefault(m => m.Id == request.MemberId);
        if (member is null)    return NotFound($"Member {request.MemberId} not found.");

        var record = new BorrowRecord
        {
            Id         = _records.Count == 0 ? 1 : _records.Max(r => r.Id) + 1,
            BookId     = request.BookId,
            MemberId   = request.MemberId,
            BorrowDate = DateTime.Now,
            IsReturned = false
        };

        book.IsAvailable = false;
        _records.Add(record);
        return CreatedAtAction(nameof(GetAll), new { id = record.Id }, record);
    }

    [HttpPut("{id:int}/return")]
    public IActionResult Return(int id)
    {
        var record = _records.FirstOrDefault(r => r.Id == id);
        if (record is null)    return NotFound($"Borrow record {id} not found.");
        if (record.IsReturned) return BadRequest("This book has already been returned.");

        record.IsReturned = true;
        record.ReturnDate = DateTime.Now;

        var book = _books.FirstOrDefault(b => b.Id == record.BookId);
        if (book is not null) book.IsAvailable = true;

        return NoContent();
    }
}

public record BorrowRequest(int BookId, int MemberId);
