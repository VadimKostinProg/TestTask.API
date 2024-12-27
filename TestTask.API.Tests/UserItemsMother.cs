using TestTask.Data.Entities;

namespace TestTask.API.Tests;

public static class UserItemsMother
{
    public static async Task SeedUserItemsAsync()
    {
        await Context.DbContext.UserItems.AddRangeAsync(GenerateUserItems());

        await Context.DbContext.SaveChangesAsync();
    }

    private static IEnumerable<UserItem> GenerateUserItems()
    {
        for (int itemId = 2, itemPurchases = 1; itemId <= 5; itemId++, itemPurchases++)
        {
            for (int i = 0; i < itemPurchases; i++)
            {
                yield return new()
                {
                    UserId = 1,
                    ItemId = itemId,
                    PurchaseDate = DateTime.Parse("2024-12-27T00:00:00").ToUniversalTime(),
                };
            };
        }
    }
}
