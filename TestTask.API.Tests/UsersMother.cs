using TestTask.Data.Entities;

namespace TestTask.API.Tests;

public static class UsersMother
{
    public static async Task SeedUsersAsync(int initialUserBalance)
    {
        await Context.DbContext.Users.AddAsync(new User
        {
            Email = "Email@gmail.com",
            Balance = initialUserBalance
        });

        await Context.DbContext.SaveChangesAsync();
    }
}
