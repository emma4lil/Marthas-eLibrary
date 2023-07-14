using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Entities;

namespace eLibrary.Domain.Contracts
{
    public interface IBookShelf
    {
        Task<(bool Success, string Message)> AddBookAsync();
        Task<(bool Success, string Message)> RemoveBookAsync();
        Task<(bool Success, string Message)> GetBookAsync();
        Task<(bool Success, string Message, List<Book> Books)> GetBooksAsync();
    }
}
