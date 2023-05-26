using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Domain;

public class Expense
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

#warning introduce enum for that
    public string Category { get; set; }

    public double AmountSpent { get; set; }
}