using Microsoft.EntityFrameworkCore;
using Model;

class DeskDb : DbContext
{
    public DeskDb(DbContextOptions<DeskDb> options)
        : base(options) { }

    public DbSet<Desk> desks => Set<Desk>();
}