using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FamilyBudget.DAL;
using FamilyBudget.Domain;

namespace FamilyBudget.WebApp.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly FamilyBudget.DAL.FamilyBudgetDbContext _context;

        public IndexModel(FamilyBudget.DAL.FamilyBudgetDbContext context)
        {
            _context = context;
        }

        public IList<Expense> Expense { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Expenses != null)
            {
                Expense = await _context.Expenses
                .Include(e => e.FamilyBudgetUser).ToListAsync();
            }
        }
    }
}
