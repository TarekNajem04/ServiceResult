using System;
using AutoMapper;
using ServiceResult.Domain.DataTransferObjects;
using ServiceResult.Domain.Entities;

namespace ServiceResult
{
    /// <summary>
    /// This is the base class for all services in our projects
    /// </summary>
    public abstract class Service : IService
    {
        protected Service(IMapper mapper) => Mapper = mapper;

        protected IMapper Mapper { get; set; }

        public virtual TDTO Map<TEntity, TDTO>(TEntity entity)
            where TEntity : class, IEntity, new()
            where TDTO : class, IDTO, new()
            => Mapper.Map<TDTO>(entity);

        public virtual TEntity Map<TEntity, TDTO>(TDTO dto)
            where TEntity : class, IEntity, new()
            where TDTO : class, IDTO, new()
            => Mapper.Map<TEntity>(dto);
    }

    public abstract class Service<TEntity, TDTO> : Service
            where TEntity : Entity, new()
            where TDTO : DTO, new()
    {
        protected Service(IMapper mapper) : base(mapper) { }

        public virtual TDTO GetById(Guid id) => Map(new TEntity());
        public virtual TDTO Map(TEntity entity) => base.Map<TEntity, TDTO>(entity);

        public virtual TEntity Map(TDTO dto) => base.Map<TEntity, TDTO>(dto);
    }
}