﻿using Fanzoo.Kernel.Data.Mapping;
using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Guid;
using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Data.Mapping
{
    public class RefreshTokenMap : MutableEntityClassMap<RefreshToken, RefreshTokenIdentifierValue, Guid>
    {
        public RefreshTokenMap() : base()
        {
            MapValueObject(e => e.Token);

            MapValueObject(e => e.IPAddress);

            Map(e => e.Issued);

            Map(e => e.ExpirationDate);

            Map(e => e.Revoked);

            Not.LazyLoad();
        }
    }
}
