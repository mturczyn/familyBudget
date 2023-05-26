using FamilyBudget.DAL;
using FamilyBudget.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.WebApp.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly FamilyBudgetDbContext _context;

        public IndexModel(FamilyBudgetDbContext context)
        {
            _context = context;
        }

        public IList<Expense> Expense { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Expenses != null)
            {
                Expense = await _context.Expenses.ToListAsync();
            }
        }
    }
}
