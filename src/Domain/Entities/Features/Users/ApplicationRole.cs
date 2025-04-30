//using KarnelTravel.Domain.Common;
//using Microsoft.AspNetCore.Identity;

//namespace KarnelTravel.Domain.Entities.Features.Users;

//public class ApplicationRole : IdentityRole, IAuditableEntity
//{
//    public DateTimeOffset Created { get; set; }

//    public string CreatedBy { get; set; }

//    public DateTimeOffset LastModified { get; set; }

//    public string LastModifiedBy { get; set; }

//    public bool IsActive { get; set; }

//    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

//    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = new List<ApplicationRoleClaim>();
//}
