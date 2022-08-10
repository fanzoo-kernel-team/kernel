namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities
{
    public class User : Domain.Entities.RefreshTokens.Users.Guid.User<RefreshToken>
    {
        protected User() : base(10, 10) { }

        protected override RefreshToken CreateToken() => throw new NotImplementedException();
    }
}
