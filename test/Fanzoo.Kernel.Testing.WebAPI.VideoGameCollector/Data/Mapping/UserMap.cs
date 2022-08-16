﻿using Fanzoo.Kernel.Data.Mapping;
using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Mapping
{
    public class UserMap : MutableEntityClassMap<User, UserIdentifierValue, Guid>
    {
        public UserMap() : base()
        {
            MapValueObject(e => e.Username);

            MapValueObject(e => e.Email);

            MapValueObject(e => e.Password);

            HasMany(e => e.RefreshTokens, "UserId");

            //HasMany(e => e.Roles)
            //    .Table("UserRole")
            //    .KeyColumn("UserId")
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Not.Inverse()
            //    .Component(c => c.Map(x => x.Id, "RoleId"))
            //    .Access
            //        .CamelCaseField(Prefix.Underscore)
            //    .Cascade
            //        .AllDeleteOrphan();

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