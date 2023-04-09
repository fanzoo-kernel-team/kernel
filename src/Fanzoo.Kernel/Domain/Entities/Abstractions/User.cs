namespace Fanzoo.Kernel.Domain.Entities.Users
{
    public interface IUser<out TIdentifier, TPrimitive, out TUsername> : IMutableEntity
    where TIdentifier : IdentifierValue<TPrimitive>
    where TPrimitive : notnull, new()
    where TUsername : IUsernameValue
    {
        TIdentifier Id { get; }

        TUsername Username { get; }

        EmailValue Email { get; }

        HashedPasswordValue Password { get; }

        NameValue Name { get; }

        bool IsLockedOut { get; }

        bool IsActive { get; }

        DateTime LastAuthenticationChange { get; }

        void RecordValidLogin();

        void RecordInvalidLogin();

        void SignOut();

        void Lock();

        void Unlock();

        void UpdatePassword(HashedPasswordValue password);

        bool RequiresAuthentication(DateTime persistedLastAuthenticationChange);
    }

    public interface IUser<out TIdentifier, TPrimitive, out TUsername, TRoleValue, TRolePrimitive> : IUser<TIdentifier, TPrimitive, TUsername>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TRoleValue : IRoleValue<TRolePrimitive>
        where TRolePrimitive : notnull
    {
        IEnumerable<TRoleValue> Roles { get; }

        bool HasRole(TRoleValue role);

        void AddRole(TRoleValue role);

        bool CanAddRole(TRoleValue role);
    }

    public interface IUser<out TIdentifier, TPrimitive, out TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive> : IUser<TIdentifier, TPrimitive, TUsername>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive>
        where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
        where TTokenPrimitive : notnull, new()
    {
        IEnumerable<TRefreshToken> RefreshTokens { get; }

        TRefreshToken AddRefreshToken(DateTime expirationDate, IPAddressValue ipAddress);

        void RevokeAllTokens();

        void RevokeToken(RefreshTokenValue token);
    }

    public interface IUser<out TIdentifier, TPrimitive, out TUsername, TRoleValue, TRolePrimitive, TRefreshToken, TTokenIdentifier, TTokenPrimitive> :
        IUser<TIdentifier, TPrimitive, TUsername, TRoleValue, TRolePrimitive>,
        IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TIdentifier : IdentifierValue<TPrimitive>
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TRoleValue : IRoleValue<TRolePrimitive>
            where TRolePrimitive : notnull
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()
    {

    }

    public abstract class User<TIdentifier, TPrimitive, TUsername> : AggregateRoot<TIdentifier, TPrimitive>, IUser<TIdentifier, TPrimitive, TUsername>
        where TIdentifier : IdentifierValue<TPrimitive>, new()
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
    {
        private readonly int _maxFailedLogins;

        protected User(int maxFailedLogins)
        {
            _maxFailedLogins = maxFailedLogins;
        }

        public virtual TUsername Username { get; protected set; } = default!;

        public virtual EmailValue Email { get; protected set; } = default!;

        public virtual HashedPasswordValue Password { get; protected set; } = default!;

        public NameValue Name { get; protected set; } = default!;

        public virtual DateTime? LastLogin { get; protected set; }

        public virtual int FailedLoginAttempts { get; protected set; } = 0;

        public virtual bool IsLockedOut { get; protected set; } = false;

        public virtual DateTime? LastPasswordChange { get; protected set; }

        public virtual DateTime LastAuthenticationChange { get; protected set; }

        public virtual bool ForcePasswordChange { get; protected set; } = false;

        public virtual bool IsActive { get; protected set; }

        public virtual bool RequiresAuthentication(DateTime persistedLastAuthenticationChange) => LastAuthenticationChange > persistedLastAuthenticationChange;

        public virtual void RecordValidLogin()
        {
            LastLogin = SystemDateTime.Now;
            FailedLoginAttempts = 0;

            OnRecordValidLogin();
        }

        public virtual void RecordInvalidLogin()
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= _maxFailedLogins)
            {
                Lock();
            }

            OnRecordInvalidLogin();
        }

        public virtual void SignOut() => OnSignOut();

        public virtual void Lock()
        {
            IsLockedOut = true;
            LastAuthenticationChange = SystemDateTime.Now;

            OnLock();

        }

        public virtual void Unlock()
        {
            IsLockedOut = false;
            FailedLoginAttempts = 0;

            OnUnlock();
        }

        public virtual void UpdatePassword(HashedPasswordValue password)
        {
            Password = password;
            LastPasswordChange = SystemDateTime.Now;
            LastAuthenticationChange = SystemDateTime.Now;
            ForcePasswordChange = false;

            OnUpdatePassword();
        }

        public virtual void ResetPassword(HashedPasswordValue password)
        {
            Password = password;
            LastPasswordChange = SystemDateTime.Now;
            LastAuthenticationChange = SystemDateTime.Now;
            ForcePasswordChange = true;

            OnResetPassword();

        }

        public virtual void Deactivate()
        {
            IsActive = false;
            LastAuthenticationChange = SystemDateTime.Now;

            OnDeactivate();
        }

        public virtual void Activate()
        {
            IsActive = true;

            OnActivate();
        }

        protected virtual void OnRecordValidLogin() { }

        protected virtual void OnRecordInvalidLogin() { }

        protected virtual void OnSignOut() { }

        protected virtual void OnActivate() { }

        protected virtual void OnDeactivate() { }

        protected virtual void OnLock() { }

        protected virtual void OnUnlock() { }

        protected virtual void OnUpdatePassword() { }

        protected virtual void OnResetPassword() { }
    }

    public abstract class User<TIdentifier, TPrimitive, TUsername, TRoleValue, TRolePrimitive> : User<TIdentifier, TPrimitive, TUsername>, IUser<TIdentifier, TPrimitive, TUsername, TRoleValue, TRolePrimitive>
        where TIdentifier : IdentifierValue<TPrimitive>, new()
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TRoleValue : IRoleValue<TRolePrimitive>
        where TRolePrimitive : notnull, new()
    {
        private readonly IList<TRoleValue> _roles = new List<TRoleValue>();

        protected User(int maxFailedLogins) : base(maxFailedLogins) { }

        public virtual IEnumerable<TRoleValue> Roles => _roles;

        public virtual bool HasRole(TRoleValue role) => _roles.Contains(role);

        public virtual void AddRole(TRoleValue role)
        {
            if (CanAddRole(role))
            {
                _roles.Add(role);
            }
            else
            {
                throw new InvalidOperationException(Errors.Entities.User.RoleCannotBeAddedInTheCurrentState.Message);
            }
        }

        public abstract bool CanAddRole(TRoleValue role);
    }

    public abstract class User<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive> :
        User<TIdentifier, TPrimitive, TUsername>,
        IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TIdentifier : IdentifierValue<TPrimitive>, new()
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()
    {

        private readonly int _numberOfInactiveTokensToStore;
        private readonly IList<TRefreshToken> _refreshTokens = new List<TRefreshToken>();

        protected User(int maxFailedLogins, int numberOfInactiveTokensToStore) : base(maxFailedLogins)
        {
            Guard.Against.LessThan(numberOfInactiveTokensToStore, 0, nameof(numberOfInactiveTokensToStore));

            _numberOfInactiveTokensToStore = numberOfInactiveTokensToStore;
        }

        public IEnumerable<TRefreshToken> RefreshTokens => _refreshTokens;

        public virtual TRefreshToken AddRefreshToken(DateTime expirationDate, IPAddressValue ipAddress)
        {
            //remove inactive tokens
            RemoveInActiveRefreshTokens();

            var token = CreateToken(expirationDate, ipAddress);

            _refreshTokens.Add(token);

            return token;
        }

        public virtual void RevokeToken(RefreshTokenValue token) => RefreshTokens.Single(t => t.Token == token).Revoke();

        public void RevokeAllTokens() =>
            RefreshTokens
                .Where(t => t.IsActive)
                    .ToList()
                        .ForEach(t => t.Revoke());

        private void RemoveInActiveRefreshTokens()
        {
            var inActiveTokens = _refreshTokens
                .Where(token => !token.IsActive)
                    .OrderBy(token => token.Issued)
                        .ToArray();

            var tokensToRemove = inActiveTokens
                .Take(
                    Math.Max(inActiveTokens.Length - _numberOfInactiveTokensToStore, 0))
                        .ToArray();

            _refreshTokens.RemoveAll(token => tokensToRemove.Contains(token));
        }

        protected abstract TRefreshToken CreateToken(DateTime expirationDate, IPAddressValue ipAddress);

        protected override void OnLock() => InvalidateAllTokens();

        protected override void OnUpdatePassword() => InvalidateAllTokens();

        protected override void OnResetPassword() => InvalidateAllTokens();

        protected override void OnDeactivate() => InvalidateAllTokens();

        private void InvalidateAllTokens()
        {
            foreach (var token in _refreshTokens)
            {
                token.Revoke();
            }
        }
    }

    public abstract class User<TIdentifier, TPrimitive, TUsername, TRoleValue, TRolePrimitive, TRefreshToken, TTokenIdentifier, TTokenPrimitive> :
        User<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>,
        IUser<TIdentifier, TPrimitive, TUsername, TRoleValue, TRolePrimitive, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
            where TIdentifier : IdentifierValue<TPrimitive>, new()
            where TPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive>
            where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
            where TTokenPrimitive : notnull, new()
            where TRoleValue : IRoleValue<TRolePrimitive>
            where TRolePrimitive : notnull, new()
    {
        private readonly IList<TRoleValue> _roles = new List<TRoleValue>();

        protected User(int maxFailedLogins, int numberOfInactiveTokensToStore) : base(maxFailedLogins, numberOfInactiveTokensToStore) { }

        public virtual IEnumerable<TRoleValue> Roles => _roles;

        public virtual bool HasRole(TRoleValue role) => _roles.Contains(role);

        public virtual void AddRole(TRoleValue role)
        {
            if (CanAddRole(role))
            {
                _roles.Add(role);
            }
            else
            {
                throw new InvalidOperationException(Errors.Entities.User.RoleCannotBeAddedInTheCurrentState.Message);
            }
        }

        public abstract bool CanAddRole(TRoleValue role);

    }
}