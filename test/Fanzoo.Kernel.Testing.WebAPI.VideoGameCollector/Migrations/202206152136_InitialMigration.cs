namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Migrations
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
                    .WithColumn("FirstName")
                        .AsString(DatabaseCatalog.FieldLength.Short)
                        .NotNullable()
                    .WithColumn("LastName")
                        .AsString(DatabaseCatalog.FieldLength.Short)
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

            Create.Table("Role")
                .AsMutable()
                    .WithNameColumn();

            Create.Table("UserRole")
                .WithColumn("UserId")
                    .AsGuid()
                    .NotNullable()
                .WithColumn("RoleId")
                    .AsGuid()
                    .NotNullable();

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

            Create.ForeignKey("UserRole", "User");
            Create.ForeignKey("UserRole", "Role");

            Create.ForeignKey("RefreshToken", "User");

            Create.Index("RefreshToken", "Token").Unique();

            var billw = Guid.NewGuid();
            var bob = Guid.NewGuid();

            Insert.IntoTable("User").Row(new { Id = billw, Username = "billw@fanzootechnology.com", Email = "billw@fanzootechnology.com", Password = "AQAAAAIAAYagAAAAEBcptSgsDxw1gTqp23LLdjvDEn7SFGEINrxBoJHdATJC/qa+bXW5v8JE/gL2G0Fenw==", FirstName = "Bill", LastName = "Wheelock", LastAuthenticationChange = DateTime.Now, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "system" });
            Insert.IntoTable("User").Row(new { Id = bob, Username = "bob@fanzootechnology.com", Email = "bob@fanzootechnology.com", Password = "AQAAAAIAAYagAAAAEBcptSgsDxw1gTqp23LLdjvDEn7SFGEINrxBoJHdATJC/qa+bXW5v8JE/gL2G0Fenw==", FirstName = "Bob", LastName = "Builder", LastAuthenticationChange = DateTime.Now, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "system" });

            Insert.IntoTable("Role").Row(new { ApplicationRoleValue.Administrator.Id, ApplicationRoleValue.Administrator.Name, CreatedDate = DateTime.Now, CreatedBy = "system" });
            Insert.IntoTable("Role").Row(new { ApplicationRoleValue.User.Id, ApplicationRoleValue.User.Name, CreatedDate = DateTime.Now, CreatedBy = "system" });

            Insert.IntoTable("UserRole").Row(new { UserId = billw, RoleId = ApplicationRoleValue.Administrator.Id });
            Insert.IntoTable("UserRole").Row(new { UserId = bob, RoleId = ApplicationRoleValue.User.Id });

        }

        public override void Down() { }
    }
}
