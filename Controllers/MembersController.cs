using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly List<Member> _members;

    public MembersController(List<Member> members) => _members = members;

    [HttpGet]
    public ActionResult<IEnumerable<Member>> GetAll() => Ok(_members);

    [HttpGet("{id:int}")]
    public ActionResult<Member> GetById(int id)
    {
        var member = _members.FirstOrDefault(m => m.Id == id);
        return member is null ? NotFound($"Member {id} not found.") : Ok(member);
    }

    [HttpPost]
    public ActionResult<Member> Create([FromBody] Member member)
    {
        member.Id = _members.Count == 0 ? 1 : _members.Max(m => m.Id) + 1;
        _members.Add(member);
        return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Member updated)
    {
        var existing = _members.FirstOrDefault(m => m.Id == id);
        if (existing is null) return NotFound($"Member {id} not found.");
        existing.Name      = updated.Name;
        existing.StudentId = updated.StudentId;
        existing.Email     = updated.Email;
        existing.RfidValue = updated.RfidValue;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var member = _members.FirstOrDefault(m => m.Id == id);
        if (member is null) return NotFound($"Member {id} not found.");
        _members.Remove(member);
        return NoContent();
    }
}
