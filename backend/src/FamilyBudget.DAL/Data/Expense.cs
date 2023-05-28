using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudget.DAL.Data;

public class Expense
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

#warning introduce enum for that
    public string Category { get; set; }

    public double AmountSpent { get; set; }

    [ForeignKey(nameof(FamilyBudgetUser))]
    public Guid? FamilyBudgetUserId { get; set; }

    public FamilyBudgetUser? FamilyBudgetUser { get; set; }
}