namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Entities
{
    public class User : Domain.Entities.RefreshTokens.Users.Guid.User<RefreshToken>
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

        protected override RefreshToken CreateToken() => throw new NotImplementedException();
    }
}
