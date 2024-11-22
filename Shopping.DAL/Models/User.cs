using Microsoft.AspNetCore.Identity;

namespace Shopping.DAL;

public partial class User : IdentityUser
{
    public string FullName { get; set; }
    public Role Role { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}
