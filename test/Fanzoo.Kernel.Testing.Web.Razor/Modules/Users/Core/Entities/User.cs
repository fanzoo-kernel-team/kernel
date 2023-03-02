using Fanzoo.Kernel.Defaults.Domain.Entities.Users;

namespace Fanzoo.Kernel.Testing.Web.Razor.Modules.Users.Core.Entities
{
    public class User : User<ApplicationRoleValue>
    {
        protected User() : base(10) { }

        public override bool CanAddRole(ApplicationRoleValue role) => !Roles.Any();
    }
}
