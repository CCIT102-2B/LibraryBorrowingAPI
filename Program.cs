using LibraryAPI.Models;

var builder = WebApplication.CreateBuilder(args);

var books = new List<Book>
{
    new() { Id = 1, Title = "Clean Code",               Author = "Robert C. Martin", ISBN = "978-0132350884", IsAvailable = true  },
    new() { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt",       ISBN = "978-0135957059", IsAvailable = true  },
    new() { Id = 3, Title = "C# in Depth",              Author = "Jon Skeet",         ISBN = "978-1617294532", IsAvailable = false }
};

var members = new List<Member>
{
    new() { Id = 1, Name = "Alice Johnson", StudentId = "STU001", Email = "alice@uni.edu", RfidValue = "A1B2C3D4" },
    new() { Id = 2, Name = "Bob Smith",     StudentId = "STU002", Email = "bob@uni.edu",   RfidValue = "E5F6A7B8" }
};

var borrowRecords = new List<BorrowRecord>
{
    new() { Id = 1, BookId = 3, MemberId = 1, BorrowDate = DateTime.Now.AddDays(-5), IsReturned = false }
};

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "Library Borrowing API",
        Version     = "v1",
        Description = "A demo in-memory library system for students."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSingleton(books);
builder.Services.AddSingleton(members);
builder.Services.AddSingleton(borrowRecords);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("DevCors");
app.UseAuthorization();
app.MapControllers();

app.Run("http://localhost:5000");
