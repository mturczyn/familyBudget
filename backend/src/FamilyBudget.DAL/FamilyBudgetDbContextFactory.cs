using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyBudget.DAL
{
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
