using System;

namespace MovieApp.Manager.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
    }
}
