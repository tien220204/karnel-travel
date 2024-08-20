using Microsoft.AspNetCore.Identity;

namespace KarnelTravel.Domain.Entities.Features.Users;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; }

    public int DisplayOrder { get; set; }

    public string Group { get; set; } = null!;

    public string GroupDisplayName { get; set; }

    public virtual ApplicationRole Role { get; set; } = null!;
}
