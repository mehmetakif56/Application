using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBE.ENERGY.Core.Entities
{
    public class UserEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsExternal { get; set; }
        public Guid? CustomerId { get; set; }
        public virtual CustomerEntity Customer { get; set; }
        public Guid? EmployeeId { get; set; }
        public string ShadowPassword { get; set; }
        public virtual EmployeeEntity Employee { get; set; }
        public virtual ICollection<TaskUserAuthRoleEntity> TaskUserAuthRoles { get; set; }
        public virtual ICollection<CustomClaimEntity> CustomClaims { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public virtual ICollection<TaskUserOwnerRoleEntity> TaskUserOwnerRoles { get; set; }
        public virtual ICollection<FacilityResponsibleEntity> FacilityResponsibles { get; set; }
        [NotMapped]
        public Guid[] SelectedRoleIds { get; set; }

    }
}
