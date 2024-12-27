using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Data.Entities;
using TestTask.Utils.DTO;

namespace TestTask.Services;

public class MarketService
{
    private readonly TestDbContext _testDbContext;

    public MarketService(TestDbContext testDbContext)
    {
        _testDbContext = testDbContext;
    }

    public async Task<List<PopularityReportLineDto>> GetMostPopularItemsReport()
    {
        var result = await _testDbContext.UserItems
            .GroupBy(ui => new { ui.ItemId, Year = ui.PurchaseDate.Year, Day = ui.PurchaseDate.Date })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.ItemId,
                MaxDailyPurchase = g.Count()
            })
            .GroupBy(g => new { g.Year, g.ItemId })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.ItemId,
                MaxDailyPurchase = g.Max(x => x.MaxDailyPurchase)
            })
            .Join(_testDbContext.Items, g => g.ItemId, i => i.Id, (g, i) => new
            {
                g.Year,
                ItemName = i.Name,
                MaxDailyPurchase = g.MaxDailyPurchase
            })
            .GroupBy(x => x.Year)
            .Select(g => new
            {
                Year = g.Key,
                TopItems = g.OrderByDescending(x => x.MaxDailyPurchase).Take(3).ToList()
            })
            .OrderByDescending(g => g.Year)
            .ToListAsync();

        return result
            .SelectMany(g => g.TopItems.Select(x => new PopularityReportLineDto
            {
                Year = x.Year,
                ItemName = x.ItemName,
                MaxDailyPurchase = x.MaxDailyPurchase
            }))
            .ToList();
    }

    public async Task BuyAsync(int userId, int itemId)
    {
        var user = await _testDbContext.Users.FirstOrDefaultAsync(n => n.Id == userId);
        if (user == null)
            throw new Exception("User not found");
        var item = await _testDbContext.Items.FirstOrDefaultAsync(n => n.Id == itemId);
        if (item == null)
            throw new Exception("Item not found");

        if (user.Balance < item.Cost)
        {
            if (item == null)
                throw new Exception("Not enough balance");
        }

        user.Balance -= item.Cost;
        await _testDbContext.UserItems.AddAsync(new UserItem
        {
            UserId = userId,
            ItemId = itemId,
            PurchaseDate = DateTime.UtcNow
        });

        await _testDbContext.SaveChangesAsync();
    }
}