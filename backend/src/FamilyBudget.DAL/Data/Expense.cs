using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [ForeignKey(nameof(IdentityUser))]
    public int IdentityUserId { get; set; }

    public IdentityUser IdentityUser { get; set; }
}