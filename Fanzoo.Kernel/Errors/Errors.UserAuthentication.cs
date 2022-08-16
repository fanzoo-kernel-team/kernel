namespace Fanzoo.Kernel
{
    public static partial class Errors
    {
        public static class UserAuthentication
        {
            public static Error UserNotFound => new("user.not.found", "User not found.");

            public static Error PasswordVerificationFailed => new("password.verification.failed", "Failed to verify password.");

            public static Error AccountIsLocked => new("account.is.locked", "Account has been locked.");

            public static Error AccountIsNotActive => new("account.is.not.active", "Account is not active.");

            public static Error InvalidRefreshToken => new("invalid.refresh.token", "Invalid token.");

            public static Error TokenIsExpired => new("expired.token", "The token is expired.");

            public static Error TokenIsRevoked => new("revoked.token", "The token is revoked.");
        }
    }
}
