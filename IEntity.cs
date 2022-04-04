using System;

namespace DBE.ENERGY.Core.Interfaces
{
    public interface IEntity
    {
        object Id { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        Guid? ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        bool IsDeleted { get; set; }
    }

    public interface IEntity<T> : IEntity
    {
        new T Id { get; set; }
    }
}
