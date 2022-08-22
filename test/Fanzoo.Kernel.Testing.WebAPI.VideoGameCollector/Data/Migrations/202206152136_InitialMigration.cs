namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Migrations
{
    [Migration(202206152136)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .AsMutable()
                    .WithEmailColumn()
                    .WithColumn("Username")
                        .AsString(DatabaseCatalog.FieldLength.Email)
                        .NotNullable()
                    .WithColumn("Password")
                        .AsString(DatabaseCatalog.FieldLength.Short)
                        .NotNullable()
                    .WithColumn("LastLogin")
                        .AsDateTime2()
                        .Nullable()
                    .WithColumn("FailedLoginAttempts")
                        .AsInt32()
                        .NotNullable()
                        .WithDefaultValue(0)
                    .WithColumn("IsLockedOut")
                        .AsBoolean()
                        .NotNullable()
                        .WithDefaultValue(false)
                    .WithColumn("LastPasswordChange")
                        .AsDateTime2()
                        .Nullable()
                    .WithColumn("LastAuthenticationChange")
                        .AsDateTime2()
                        .NotNullable()
                    .WithColumn("ForcePasswordChange")
                        .AsBoolean()
                        .NotNullable()
                        .WithDefaultValue(false)
                    .WithActiveColumn();

            Create.Table("RefreshToken")
                .AsMutable()
                    .WithForeignKeyColumn("User")
                    .WithColumn("Token")
                        .AsString(DatabaseCatalog.FieldLength.Long)
                        .NotNullable()
                    .WithColumn("Issued")
                        .AsDateTime2()
                        .NotNullable()
                    .WithColumn("ExpirationDate")
                        .AsDateTime2()
                        .NotNullable()
                    .WithColumn("Revoked")
                        .AsDateTime2()
                        .Nullable()
                    .WithColumn("IPAddress")
                        .AsString(DatabaseCatalog.FieldLength.Default)
                        .NotNullable();

            Create.Table("Game")
                .AsMutable()
                    .WithColumn("Name")
                        .AsString()
                        .NotNullable();

            Create.Index("User", "Username").Unique();
            Create.Index("User", "Email").Unique();

            Create.ForeignKey("RefreshToken", "User");

            Create.Index("RefreshToken", "Token").Unique();
        }

        public override void Down() { }
    }
}
