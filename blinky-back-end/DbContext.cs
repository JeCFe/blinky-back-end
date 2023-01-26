using Microsoft.EntityFrameworkCore;
using Blinky_Back_End.Model;

namespace Blinky_Back_End;
class DeskDb : DbContext
{
    public DeskDb(DbContextOptions<DeskDb> options)
        : base(options) { }

    public DbSet<Desk> desks => Set<Desk>();
}