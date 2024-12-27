using Microsoft.EntityFrameworkCore;
using TestTask.API.Controllers;
using TestTask.Data.Entities;
using TestTask.Utils.DTO;

namespace TestTask.API.Tests;

// First test task: Ensure the tests in this class are successful
public class PurchaseTests : BaseTest
{
    private const int ItemCost = 100;
    private const int MaxPurchasableItemCount = 5;
    private const int InitialUserBalance = ItemCost * MaxPurchasableItemCount;

    protected override async Task SetupBase()
    {
        await UsersMother.SeedUsersAsync(InitialUserBalance);
        await ItemsMother.SeedItemsAsync(InitialUserBalance);
        await UserItemsMother.SeedUserItemsAsync();
    }

    [Test]
    public async Task BuyAsync_ShouldPurchaseItemAndUpdateBalance()
    {
        // Arrange
        var user = await Context.DbContext.Users.FirstAsync();
        var item = await Context.DbContext.Items.FirstAsync();

        // Act
        await Rait<MarketController>().Call(controller => controller.BuyAsync(user.Id, item.Id));

        // Assert
        var totalUserItems = await Context.DbContext.UserItems.Where(x => x.ItemId == item.Id).CountAsync();
        Assert.That(totalUserItems, Is.EqualTo(1));
    }


    [Test]
    public async Task BuyAsync_ShouldHandleConcurrentPurchases()
    {
        // Arrange
        var user = await Context.DbContext.Users.FirstAsync();
        var item = await Context.DbContext.Items.FirstAsync();
        var initialUserItemCount = await Context.DbContext.UserItems.CountAsync();

        // Act
        var tasks = Enumerable.Range(0, 5).Select(_ =>
            Rait<MarketController>().Call(controller => controller.BuyAsync(user.Id, item.Id))
        );

        await Task.WhenAll(tasks);

        // Assert
        var finalUserItemCount = await Context.DbContext.UserItems.CountAsync();
        Assert.That(finalUserItemCount, Is.EqualTo(initialUserItemCount + MaxPurchasableItemCount));
    }

    [Test]
    public async Task GetMostPopularItemsReport_ReturnsTop3PurchasedItems()
    {
        // Arrange
        List<PopularityReportLineDto> expectedReport = [
            new() { ItemName = "Item 5", Year = 2024, MaxDailyPurchase = 4 },
            new() { ItemName = "Item 4", Year = 2024, MaxDailyPurchase = 3 },
            new() { ItemName = "Item 3", Year = 2024, MaxDailyPurchase = 2 },
        ];

        // Act
        var result = await Rait<MarketController>().Call(controller => controller.GetMostPopularItemsReport());

        Assert.That(result!.Value, Is.EqualTo(expectedReport));
    }
}