namespace FamilyBudget.WebApp.Pages.ViewObjects.Users;

public class UserDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public bool IsSharedWith { get; set; }
}
