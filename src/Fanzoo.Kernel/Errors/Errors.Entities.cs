namespace Fanzoo.Kernel
{
    public partial class Errors
    {
        public static class Entities
        {
            public static class User
            {
                public static Error RoleCannotBeAddedInTheCurrentState => new("user.role.cannot.be.added.in.current.state", "The role cannot be added in the current state.");
            }

            public static class RefreshToken
            {
                public static Error ExpirationDateMustBeInTheFuture => new("expirationdate.must.be.greaterthan.now", "Expiration date must be in the future.");
            }
        }
    }
}
