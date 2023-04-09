using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;

namespace Fanzoo.Kernel.Testing.Web.Razor.Modules.Users.Data.Mapping
{
    public class UserMap : MutableEntityClassMap<User, UserIdentifierValue, Guid>
    {
        public UserMap() : base()
        {
            MapValueObject(e => e.Username);

            MapValueObject(e => e.Email);

            MapValueObject(e => e.Password);

            MapNameValue(e => e.Name);

            HasMany(e => e.Roles)
                .Table("UserRole")
                .KeyColumn("UserId")
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Not.Inverse()
                .Component(c => c.Map(x => x.Id, "RoleId"))
                .Access
                    .CamelCaseField(Prefix.Underscore)
                .Cascade
                    .AllDeleteOrphan();

            Map(e => e.LastLogin);
            Map(e => e.FailedLoginAttempts);
            Map(e => e.IsLockedOut);
            Map(e => e.LastPasswordChange);
            Map(e => e.LastAuthenticationChange);
            Map(e => e.ForcePasswordChange);
            Map(e => e.IsActive);

            Not.LazyLoad();
        }
    }
}
