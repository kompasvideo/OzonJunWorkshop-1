using Microsoft.AspNetCore.Mvc;
using Workshop.Api.Bll.Models;
using Workshop.Api.Bll.Services.Interfaces;
using Workshop.Api.Requests.V2;
using Workshop.Api.Responses.V2;

namespace Workshop.Api.Controllers.V2;

[ApiController]
[Route("/v2/[controller]")]
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
                    x.wiight))
                    .ToArray());
        return new CalculateResponse(result);
    }
    
    [HttpPost("get-history")]
    public GetHistoryResponse[] GetHistory(GetHistoryRequest request)
    {
        var log = _priceCalculatorService.QueryLog(request.Take);
        return log
            .Select(x => new GetHistoryResponse(
                new CargoResponse(x.Volume, x.Weight),
                x.Price))
            .ToArray();
    }
}