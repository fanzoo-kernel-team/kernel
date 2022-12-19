using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using Fanzoo.Kernel.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Domain.Entities.Users
{
    public class User : User<UserIdentifierValue, Guid, EmailUsernameValue>
    {
        protected User() : base(10) { }

        public static ValueResult<User, Error> Create(EmailUsernameValue username, EmailValue email, HashedPasswordValue password)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = password,
                ForcePasswordChange = true,
                IsActive = true
            };

            return user;
        }
    }
}
