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
        }
    }
}
