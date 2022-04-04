using System;

namespace DBE.ENERGY.Core.Entities
{
    public class UserRoleEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public Guid RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
