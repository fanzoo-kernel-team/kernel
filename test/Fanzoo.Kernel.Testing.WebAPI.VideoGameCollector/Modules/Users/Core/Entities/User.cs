using Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Values;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Entities
{
    public class User : User<ApplicationRoleValue>
    {
        protected User() : base(10, 5) { }

        public override bool CanAddRole(ApplicationRoleValue role) => Roles.Count() == 0;
    }
}
