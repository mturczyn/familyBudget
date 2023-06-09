﻿using FamilyBudget.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.DAL
{
    public class FamilyBudgetDbContext : IdentityDbContext<FamilyBudgetUser, IdentityRole<Guid>, Guid>
    {
        public FamilyBudgetDbContext(DbContextOptions<FamilyBudgetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<UserExpenseSharingOption> UserExpenseSharingOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserExpenseSharingOption>(b =>
            {
                b.HasOne(x => x.FamilyBudgetUser)
                    .WithMany(x => x.ExpensesSharedWith)
                    .HasForeignKey(x => x.FamilyBudgetUserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasAlternateKey(x => new { x.FamilyBudgetUserId, x.SharedWithUserId });
            });

            builder.Entity<FamilyBudgetUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }
    }
}
