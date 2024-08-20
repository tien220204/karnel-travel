using Microsoft.AspNetCore.Identity;

namespace KarnelTravel.Domain.Entities.Features.Users;

public class ApplicationUserRole : IdentityUserRole<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ApplicationRole Role { get; set; } = null!;
}
