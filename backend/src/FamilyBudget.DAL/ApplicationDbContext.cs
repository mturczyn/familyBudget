using FamilyBudget.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyBudget.DAL
{
    public class FamilyBudgetDbContext : IdentityDbContext<FamilyBudgetUser>
    {
        public FamilyBudgetDbContext(DbContextOptions<FamilyBudgetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
    }

    public class FamilyBudgetDbContextFactory : IDesignTimeDbContextFactory<FamilyBudgetDbContext>
    {
        public FamilyBudgetDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FamilyBudgetDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=FamilyBudget;User Id=sa;Password=MyStrong!Password;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new FamilyBudgetDbContext(optionsBuilder.Options);
        }
    }
}
