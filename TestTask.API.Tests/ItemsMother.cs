namespace TestTask.API.Tests;

public static class ItemsMother
{
    public static async Task SeedItemsAsync(int itemCost)
    {
        await Context.DbContext.Items.AddRangeAsync([
            new()
            {
                Name = "Item 1",
                Cost = itemCost
            },
            new()
            {
                Name = "Item 2",
                Cost = itemCost
            },
            new()
            {
                Name = "Item 3",
                Cost = itemCost
            },
            new()
            {
                Name = "Item 4",
                Cost = itemCost
            },
            new()
            {
                Name = "Item 5",
                Cost = itemCost
            },
        ]);

        await Context.DbContext.SaveChangesAsync();
    }
}
