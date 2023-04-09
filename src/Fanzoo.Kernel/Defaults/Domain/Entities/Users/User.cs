using Fanzoo.Kernel.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Domain.Entities.Users
{
    public class User : User<UserIdentifierValue, Guid, EmailUsernameValue>
    {
        protected User() : base(10) { }

        public static ValueResult<User, Error> Create(EmailUsernameValue username, EmailValue email, HashedPasswordValue password, NameValue name)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = password,
                Name = name,
                ForcePasswordChange = true,
                IsActive = true
            };

            return user;
        }
    }
}
