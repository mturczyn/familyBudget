using AutoFixture;
using FamilyBudget.DAL;
using FamilyBudget.WebApp.Pages.Expenses;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace FamilyBudget.Tests
{
    public class ShareWithOtherModelTests
    {
        private readonly IFixture _autoFixture = new Fixture();

        public ShareWithOtherModelTests()
        {
            // Also we could define Customization to ignore navigation properties that cause
            // recursion exception.
            _autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        // If we would apply more loosely coupled classes behind interfaces, it would simplify
        // the tests.
        // On the other hand Arrange section contains some boiler plate code that could be moved 
        // to some utility class or at least to private method allowing easy arrange for
        // each similair test.
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

            context.AddRange(new[] { currentUser, otherUser });
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