namespace Fanzoo.Kernel.Defaults.Domain.Values.Identifiers
{
    public class UserIdentifierValue : GuidIdentifierValue<UserIdentifierValue>
    {
        public UserIdentifierValue() : base() { }

        public UserIdentifierValue(Guid id) : base(id) { }
    }

    public class RefreshTokenIdentifierValue : GuidIdentifierValue<RefreshTokenIdentifierValue>
    {
        public RefreshTokenIdentifierValue() : base() { }

        public RefreshTokenIdentifierValue(Guid id) : base(id) { }
    }
}