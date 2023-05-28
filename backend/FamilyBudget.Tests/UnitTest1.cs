using AutoFixture;
using FamilyBudget.DAL;
using FamilyBudget.WebApp.Pages.Expenses;
using FamilyBudget.WebApp.Pages.ViewObjects.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace FamilyBudget.Tests
{
    public class UnitTest1
    {
        private readonly IFixture _autoFixture = new Fixture();

        public UnitTest1()
        {
            _autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetSharedExpensesForUser_WhenUserHasNoSharedExpenses_ReturnsUsersWithFlagSetToFalse()
        {
            // Arrange
            var userName = "michal@familybudget.com";

            using var context = new FamilyBudgetDbContext(new DbContextOptionsBuilder<FamilyBudgetDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

            var currentUser = _autoFixture.Create<FamilyBudgetUser>();
            currentUser.UserName = userName;
            var otherUser = _autoFixture.Create<FamilyBudgetUser>();

            context.AddRange(new[] {currentUser, otherUser});
            context.SaveChanges();

            var identityMock = _autoFixture.Create<Mock<IIdentity>>();
            identityMock.SetupGet(x => x.Name).Returns(userName);

            var httpContextMock = _autoFixture.Create<Mock<HttpContext>>();
            httpContextMock.SetupGet(x => x.User).Returns(new ClaimsPrincipal(identityMock.Object));

            var model = new ShareWithOthersModel(context)
            {
                PageContext = new PageContext()
                {
                    HttpContext = httpContextMock.Object,
                }
            };

            // Act
            await model.OnGetAsync(default);

            // Assert
            model.Users.Should().NotBeEmpty();
            model.Users.Should().HaveCount(1);
            model.Users.Should().AllSatisfy(x => x.IsSharedWith.Should().BeFalse());
        }
    }
}