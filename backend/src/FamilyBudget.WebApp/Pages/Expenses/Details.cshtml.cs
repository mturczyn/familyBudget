using FamilyBudget.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.WebApp.Pages.Expenses;

public class DetailsModel : PageModel
{
    private readonly FamilyBudget.DAL.FamilyBudgetDbContext _context;

    public DetailsModel(FamilyBudget.DAL.FamilyBudgetDbContext context)
    {
        _context = context;
    }

    public Expense Expense { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null || _context.Expenses == null)
        {
            return NotFound();
        }

        var expense = await _context.Expenses.FirstOrDefaultAsync(m => m.Id == id);
        if (expense == null)
        {
            return NotFound();
        }
        else
        {
            Expense = expense;
        }
        return Page();
    }
}
