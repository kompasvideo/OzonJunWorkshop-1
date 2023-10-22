using Workshop.Api.Dal.Entities;

namespace Workshop.Api.Dal.Repositories.Interfaces;

public interface IStorageRepository
{
    void Save(StorageEntity entity);
    StorageEntity[] Query();
}