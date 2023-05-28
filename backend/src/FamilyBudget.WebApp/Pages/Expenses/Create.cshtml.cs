using FamilyBudget.DAL;
using FamilyBudget.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.WebApp.Pages.Expenses
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly FamilyBudgetDbContext _context;

        public CreateModel(FamilyBudgetDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["FamilyBudgetUserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Expense Expense { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Expenses == null || Expense == null)
            {
                return Page();
            }

            var currentUserId = await _context.Users
                .Where(x => x.Email == User.Identity.Name)
                .Select(x => x.Id)
                .FirstAsync();

            Expense.FamilyBudgetUserId = currentUserId;

            _context.Expenses.Add(Expense);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
