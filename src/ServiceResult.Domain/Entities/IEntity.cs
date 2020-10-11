using System;
using ServiceResult.Domain.Models;

namespace ServiceResult.Domain.Entities
{
    public interface IEntity : IModel
    {
        Guid Id { get; set; }
    }
}