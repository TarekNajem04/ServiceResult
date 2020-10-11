using System;
using ServiceResult.Domain.Models;

namespace ServiceResult.Domain.Entities
{
    public class Entity : Model, IEntity
    {
        public Guid Id { get; set; }
    }
}