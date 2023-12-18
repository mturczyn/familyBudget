using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyBudget.DAL
{
    public class FamilyBudgetDbContextFactory : IDesignTimeDbContextFactory<FamilyBudgetDbContext>
    {
        public FamilyBudgetDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FamilyBudgetDbContext>();
            optionsBuilder.UseSqlServer("");

            return new FamilyBudgetDbContext(optionsBuilder.Options);
        }
    }
}
