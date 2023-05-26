using FamilyBudget.DAL;
using FamilyBudget.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyBudget.WebApp.Pages.Expenses
{
    public class CreateModel : PageModel
    {
        private readonly FamilyBudgetDbContext _context;

        public CreateModel(FamilyBudgetDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
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

            _context.Expenses.Add(Expense);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
