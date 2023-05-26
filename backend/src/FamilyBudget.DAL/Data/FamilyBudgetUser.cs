using FamilyBudget.Domain;
using Microsoft.AspNetCore.Identity;

public class FamilyBudgetUser : IdentityUser
{
    public ICollection<Expense> Expenses { get; set; }
}