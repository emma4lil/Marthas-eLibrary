using eLibrary.Application.Services;
using eLibrary.Domain.dtos.bookservice;
using eLibrary.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLibrary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly LibraryDbContext _context;
    private readonly IBookService _bookService;

    public BookController(LibraryDbContext context, IBookService bookService)
    {
        _context = context;
        _bookService = bookService;
    }

    /// <summary>
    /// Returns all books in shelve
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-books")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBooks()
    {
        var booksResult = await _bookService.GetBooks();
        return Ok(booksResult.booksList);
    }

    [HttpGet("search/{keyword}")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Lookup([FromRoute] string keyword)
    {
        var books = await _bookService.LookUp(new LookUpParam { key = keyword });
        return Ok(books.booksList);
    }

    [HttpGet("detail/{id}")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetBookDetail([FromRoute] Guid id)
    {
        var book = await _bookService.GetBookDetail(id);
        return Ok(book.book);
    }

    /// <summary>
    /// Populates db with preset books
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("seed-books")]
    public async Task<IActionResult> BookSeeder()
    {
        var books = await _bookService.PreLoadBooks();
        return Ok("done");
    }


}