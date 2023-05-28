using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudget.DAL.Data
{
    public class UserExpenseSharingOption
    {
        [Key]
        public Guid Id { get; set; }

        public Guid FamilyBudgetUserId { get; set; }

        public FamilyBudgetUser FamilyBudgetUser { get; set; }

        [ForeignKey(nameof(SharedWithUser))]
        public Guid SharedWithUserId { get; set; }

        [DeleteBehavior(DeleteBehavior.Restrict)]
        public FamilyBudgetUser SharedWithUser { get; set; }
    }
}
