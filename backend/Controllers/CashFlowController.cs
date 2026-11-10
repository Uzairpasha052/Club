using backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTO.CashIn;
using backend.DTO.CashOut;

namespace backend.Controllers;

[Route("api/cashFlow")]
[ApiController]
[Authorize]

public class CashFlowController : ControllerBase
{
    private readonly ICashService _cashService;

    public  CashFlowController(ICashService cashService)
    {
        _cashService = cashService;
    }

    [HttpPost("cashin")]
    public async Task<IActionResult> CreateCashIn(
        [FromBody] CashInDto dto)
    {
        var result = await _cashService.CashInAsync(dto);

        if (!result)
        {
            return BadRequest(new
            {
                message = "Unable to save cash in."
            });
        }

        return Ok(new
        {
            message = "Cash in saved successfully."
        });
    }

    [HttpGet("cashin")]
    public async Task<IActionResult> GetAllCashIn(
        [FromQuery] int? year,
        [FromQuery] int? month)
    {
        var result = await _cashService.GetAllCashInAsync(year, month);
        return Ok(result);
    }


    [HttpPost("cashout")]
    public async Task<IActionResult> CreateCashOut(
        [FromBody] CashOutDto dto)
    {
        var result = await _cashService.CashOutAsync(dto);

        if (!result)
        {
            return BadRequest(new
            {
                message = "Unable to save cash out."
            });
        }

        return Ok(new
        {
            message = "Cash out saved successfully."
        });
    }

     [HttpGet("cashout")]
    public async Task<IActionResult> GetAllCashOut(
        [FromQuery] int? year,
        [FromQuery] int? month)
    {
        var result = await _cashService.GetAllCashOutAsync(year, month);
        return Ok(result);
    }

}
