using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Guid;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Entities
{
    public class User : Domain.Entities.RefreshTokens.Users.Guid.User
    {
        protected User() : base(10, 10) { }

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

        protected override RefreshToken CreateToken(DateTime expirationDate, IPAddressValue ipAddress) => RefreshToken.Create(expirationDate, ipAddress).Value;
    }
}
