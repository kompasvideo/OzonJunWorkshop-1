using Microsoft.AspNetCore.Mvc;
using Workshop.Api.Bll.Models;
using Workshop.Api.Bll.Services.Interfaces;
using Workshop.Api.Requests.V1;
using Workshop.Api.Responses.V1;

namespace Workshop.Api.Controllers.V1;

[ApiController]
[Route("/v1/[controller]")]
public class DeliveryPriceController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;
    public DeliveryPriceController(IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }
    [HttpPost("calculate")]
    public CalculateResponse Calculate(CalculateRequest request)
    {
        var result = _priceCalculatorService.CalculatePrice(
            request.Goods
                .Select(x => new GoodModel(
                    x.height,
                    x.length,
                    x.width, 
                    0))
                    .ToArray());
        return new CalculateResponse(result);
    }
    [HttpPost("get-history")]
    public GetHistoryResponse[] GetHistory(GetHistoryRequest request)
    {
        var log = _priceCalculatorService.QueryLog(request.Take);
        return log
            .Select(x => new GetHistoryResponse(
                new CargoResponse(x.Volume),
                x.Price))
            .ToArray();
    }
}