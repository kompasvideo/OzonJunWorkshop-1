using Workshop.Api.Bll.Models;
using Workshop.Api.Bll.Services.Interfaces;
using Workshop.Api.Dal.Entities;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private const double volumeRatio = 3.27d;
    private const double weightRatio = 1.34d;
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

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);
        var resultPrice = Math.Max(volumePrice, weightPrice);
        _storageRepository.Save(new StorageEntity(volume, resultPrice, DateTime.UtcNow, weight));
        return resultPrice;
    }

    private static double CalculatePriceByVolume(GoodModel[] goods, out int volume)
    {
        volume = goods.Sum(x => x.Length * x.Height * x.Width);
        return volumeRatio * volume / 1000.0d;
    }

    private static double CalculatePriceByWeight(GoodModel[] goods, out double weight)
    {
        weight = goods.Sum(x => x.Weight);
        return weightRatio * weight;
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
                x.Price,
                x.Weight))
            .ToArray();
    }
}