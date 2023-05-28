using FamilyBudget.DAL.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class FamilyBudgetUser : IdentityUser<Guid>
{
    [Key]
    public override Guid Id { get; set; }

    public ICollection<Expense> Expenses { get; set; }

    public ICollection<UserExpenseSharingOption> ExpensesSharedWith { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}