using FamilyBudget.DAL;
using FamilyBudget.DAL.Data;
using FamilyBudget.WebApp.Pages.ViewObjects.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.WebApp.Pages.Expenses;

[Authorize]
public class ShareWithOthersModel : PageModel
{
    private readonly FamilyBudgetDbContext _context;

    public ShareWithOthersModel(FamilyBudgetDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public IList<UserDto> Users { get; set; } = default!;

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        if (_context.Users is null)
        {
            return;
        }

        var sharedWith = await _context.Users
            .Where(x => x.UserName == User.Identity.Name)
            .SelectMany(x => x.ExpensesSharedWith)
            .ToListAsync(cancellationToken);

        var users = await _context.Users
            .Where(x => x.UserName != User.Identity.Name)
            .ToListAsync(cancellationToken);

        Users = users
            .Select(x => new UserDto()
            {
                Id = x.Id,
                UserName = x.UserName,
                IsSharedWith = sharedWith.Any(y => y.SharedWithUserId == x.Id),
            })
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var sharedWith = Users.Where(x => x.IsSharedWith).Select(x => x.Id);

        var currentUser = await _context.Users
            .Where(x => x.UserName == User.Identity.Name)
            .Include(x => x.ExpensesSharedWith)
            .FirstAsync();

        var removedShares = currentUser.ExpensesSharedWith
            .Where(x => !sharedWith.Contains(x.Id))
            .ToArray();

        var newShares = sharedWith
            .Where(x => !currentUser.ExpensesSharedWith.Any(y => y.Id == x))
            .ToArray();

        _context.UserExpenseSharingOptions.RemoveRange(removedShares);
        _context.UserExpenseSharingOptions.AddRange(newShares
            .Select(x => new UserExpenseSharingOption()
            {
                Id = Guid.NewGuid(),
                FamilyBudgetUserId = currentUser.Id,
                SharedWithUserId = x
            }));

        await _context.SaveChangesAsync(cancellationToken);

        return RedirectToPage("./Index");
    }
}
