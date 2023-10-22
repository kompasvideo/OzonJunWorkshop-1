using Workshop.Api.Dal.Entities;
using Workshop.Api.Dal.Repositories.Interfaces;

namespace Workshop.Api.Dal.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly List<StorageEntity> _storage = new();
    public void Save(StorageEntity entity)
    {
        _storage.Add(entity);
    }

    public StorageEntity[] Query()
    {
        return _storage.ToArray();
    }
}