using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Interfaces;
using Shopping.Shared;

namespace Shopping.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : AppControllerBase
{
    private readonly ICacheService _cacheService;
    public CurrencyController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    [HttpPost("exchange-rate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetExchangeRate([FromBody] CurrencyExchangeRateDto rate)
    {
        await _cacheService.SetAsync($"CURRENCY_{rate.CurrencyCode}", rate);
        return Ok();
    }

    [HttpGet("exchange-rate")]
    public async Task<IActionResult> GetExchangeRate([FromQuery] string currencyCode)
    {
        var rate = await _cacheService.GetAsync<CurrencyExchangeRateDto>($"CURRENCY_{currencyCode}");

        if (rate == null)
            return NotFound();

        return Ok(rate.Rate);
    }
}
