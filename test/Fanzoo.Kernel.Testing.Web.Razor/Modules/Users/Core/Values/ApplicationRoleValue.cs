using Fanzoo.Kernel.Defaults.Domain.Values;

namespace Fanzoo.Kernel.Testing.Web.Razor.Modules.Users.Core.Values
{
    public sealed class ApplicationRoleValue : RoleValue<ApplicationRoleValue>
    {
        public static readonly ApplicationRoleValue User = new(new Guid(GuidCatalog.Roles.User), StringCatalog.Roles.User);
        public static readonly ApplicationRoleValue Administrator = new(new Guid(GuidCatalog.Roles.Administrator), StringCatalog.Roles.Administrator);

        private ApplicationRoleValue() { } //ORM

        private ApplicationRoleValue(Guid id, string name) : base(id, name) { }
    }
}
