using Microsoft.EntityFrameworkCore;
using Blinky_Back_End.DbModels;

namespace Blinky_Back_End;
class BookingDb : DbContext
{
    public BookingDb(DbContextOptions<BookingDb> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
        .HasMany(room => room.Desks);

        modelBuilder.Entity<Desk>()
                .HasMany(Desk => Desk.Bookings);


    }

    public DbSet<Booking> bookings => Set<Booking>();
    public DbSet<Room> rooms => Set<Room>();
    public DbSet<Desk> desks => Set<Desk>();
}