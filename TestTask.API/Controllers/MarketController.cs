using Microsoft.AspNetCore.Mvc;
using TestTask.Services;
using TestTask.Utils.DTO;

namespace TestTask.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MarketController : ControllerBase
{
    private readonly MarketService _marketService;

    public MarketController(MarketService marketService)
    {
        _marketService = marketService;
    }

    [HttpGet("report")]
    public async Task<ActionResult<List<PopularityReportLineDto>>> GetMostPopularItemsReport()
    {
        return Ok(await _marketService.GetMostPopularItemsReport());
    }

    [HttpPost]
    public async Task BuyAsync(int userId, int itemId)
    {
        await _marketService.BuyAsync(userId, itemId);
    }
}