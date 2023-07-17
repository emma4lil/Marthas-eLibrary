using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common.enums;
using eLibrary.Domain.dtos.bookservice;
using eLibrary.Domain.Entities;
using eLibrary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace eLibrary.Application.Services;

public interface IBookService
{
    Task<(bool success, string message, List<Books> booksList)> GetBooks();
    Task<(bool success, string message, List<Books> booksList)> LookUp(LookUpParam lookUp);
    Task<(bool success, string message)> PreLoadBooks();
    Task<(bool, Book? book)> GetBookDetail(Guid id);
}

public class BookService : IBookService
{
    private readonly LibraryDbContext _context;

    public BookService(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<(bool success, string message, List<Books> booksList)> GetBooks()
    {
        var books = await (from book in _context.Books.AsNoTracking()
            select new Books
            {
                Title = book.Title,
                CoverUrl = book.CoverUrl,
                Id = book.Id,
                Status = book.Status.ToString(),
                Description = book.Description,
            }).ToListAsync();

        return (true, "List of books", books);
    }

    public async Task<(bool success, string message, List<Books> booksList)> LookUp(LookUpParam lookUp)
    {
        lookUp.key = lookUp.key.ToLower();
        if (lookUp.key == "all")
        {
            return (true, "all books", (await GetBooks()).booksList);
        }
        var books = await (from book in _context.Books
                    .AsNoTracking()
                where book.Title.ToLower().Contains(lookUp.key)
                      || book.Description.ToLower().Contains(lookUp.key)
                      || book.Author.ToLower().Contains(lookUp.key)
                select new Books
                {
                    Title = book.Title,
                    CoverUrl = book.CoverUrl,
                    Id = book.Id,
                    Status = book.Status.ToString(),
                })
            .ToListAsync();

        if(!books.Any()) return (false, "Nothing was found", books);

        return (true, $"{books.Count} books found", books);
    }

    public async Task<(bool success, string message)> PreLoadBooks()
    {
        var books = new List<Book>
        {
            new()
            {
                Title = "ABCs of Science",
                CoverUrl =
                    "https://manybooks.net/sites/default/files/styles/220x330sc/public/old-covers/cover-auto-5349.jpg?itok=-w4gFXdq",
                CreatedAt = DateTime.Now,
                Author = "Oliver Charlce",
                Description = "The quick brown rabbit jumps over the lazy goat",
                ISBN = "IDX2233",
                PublicationDate = DateTime.Now.AddDays(-230),
                Status = BookStatus.Available
            },
            new()
            {
                Title = "Little Women",
                CoverUrl =
                    "https://manybooks.net/sites/default/files/styles/220x330sc/public/old-covers/cover-orig-498.jpg?itok=zY7xCyMC",
                CreatedAt = DateTime.Now,
                Author = "Louis. M",
                Description = "The quick brown Cow jumps over the lazy dog",
                ISBN = "IDV2039",
                PublicationDate = DateTime.Now.AddDays(-113),
                Status = BookStatus.Available
            },
            new()
            {
                Title = "Pinnochio",
                CoverUrl =
                    "https://manybooks.net/sites/default/files/styles/220x330sc/public/old-covers/cover-orig-14299.jpg?itok=wPpEKSe6",
                CreatedAt = DateTime.Now,
                Author = "Lewis C.",
                Description = "The quick brown fox jumps over the lazy cat",
                ISBN = "NIG0001",
                PublicationDate = DateTime.Now.AddDays(-23),
                Status = BookStatus.Available
            }
        };

        _context.Books.AddRange(books);
        await _context.SaveChangesAsync();

        return (true, "List of books");
    }

    public async Task<(bool, Book? book)> GetBookDetail(Guid id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        book.StatusDescription = book.Status
            .ToString();
        return (true, book);
    }
}