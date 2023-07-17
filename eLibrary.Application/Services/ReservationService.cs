using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Common.enums;
using eLibrary.Domain.dtos.bookservice;
using eLibrary.Domain.dtos.Reservations;
using eLibrary.Domain.Entities;
using eLibrary.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eLibrary.Application.Services;

public interface IReservationService
{
    Task<(bool success, string message)> ReserveBook(ReserveBookRequest request);
    Task<(bool success, string message)> SubscribeToBook(SubscribeBookRequest request);
    Task<(bool success, List<ReservationDetail> reservations)> GetReservations();
}

public class ReservationService : IReservationService
{
    private readonly LibraryDbContext _context;

    public ReservationService(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<(bool success, string message)> ReserveBook(ReserveBookRequest request)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == request.BookId);
        if (book.Status == BookStatus.Reserved) return (false, "already reserved");
        if (book.Status == BookStatus.Borrowed) return (false, "already borrowed");


         var newReservation = new Reservation()
         {
             UserId = ((await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email))!).Id,
             BookId = request.BookId,
             TimeInHours = request.TimeInHours
         };

        _context.Reservations.Add(newReservation);
        await _context.SaveChangesAsync();
        return (true, "Book reserved");
    }

    public async Task<(bool success, string message)> SubscribeToBook(SubscribeBookRequest request)
    {
        var possibleSub = await _context.BookSubscriptions
            .FirstOrDefaultAsync(
                bs => bs.SubscriberId == request.UserId
                      && bs.BookId == request.BookId
                      && !bs.HasNotified
            );

        if (possibleSub != null) return (false, "Already subscribed");

        var newSub = new BookSubscription
        {
            BookId = request.BookId,
            HasNotified = false,
            SubscriberId = request.UserId
        };

        _context.BookSubscriptions.Add(newSub);
        await _context.SaveChangesAsync();

        return (true, "Subscribed");
    }

    public async Task<(bool success, List<ReservationDetail> reservations)> GetReservations()
    {
        var reservations = await (from reservation in _context.Reservations
            join user in _context.Users on reservation.UserId equals user.Id
            join book in _context.Books on reservation.BookId equals book.Id 
            select new ReservationDetail
            {
                Email = user.Email,
                BookTitle = book.Title,
                Status = reservation.State.ToString(),
                Id = reservation.Id,
                TimeInHours = reservation.TimeInHours,

            }).ToListAsync();

        return (true, reservations);
    }
}