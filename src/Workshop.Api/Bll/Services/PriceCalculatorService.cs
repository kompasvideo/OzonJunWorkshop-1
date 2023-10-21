using Workshop.Api.Bll.Models;
using Workshop.Api.Bll.Services.Interfaces;
using Workshop.Api.Dal.Entities;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private const double volumeRatio = 3.27d;
    private readonly IStorageRepository _storageRepository;

    public PriceCalculatorService(IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }
    public double CalculatePrice(GoodModel[] goods)
    {
        if (goods.Any())
        {
            throw new ArgumentException("Список товаров пустой");
        }
        var volume = goods.Sum(x => x.Length * x.Height * x.Width);
        var price = volumeRatio * volume / 1000;
        _storageRepository.Save(new StorageEntity(volume,price, DateTime.UtcNow));
        return price;
    }

    public CalculationLogModel[] QueryLog(int take)
    {
        if (take <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(take), take, "Take должно быть больше нуля");
        }
        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();
        return log
            .Select(x => new CalculationLogModel(
                x.Volume, 
                x.Price))
            .ToArray();
    }
}