using ServiceResult.Domain.DataTransferObjects;
using ServiceResult.Domain.Entities;

namespace ServiceResult
{
    public interface IService
    {
        TDTO Map<TEntity, TDTO>(TEntity entity)
            where TEntity : class, IEntity, new()
            where TDTO : class, IDTO, new();

        TEntity Map<TEntity, TDTO>(TDTO dto)
            where TEntity : class, IEntity, new()
            where TDTO : class, IDTO, new();
    }
}