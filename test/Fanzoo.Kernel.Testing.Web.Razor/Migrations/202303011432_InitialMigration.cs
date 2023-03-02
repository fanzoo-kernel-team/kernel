namespace Fanzoo.Kernel.Testing.Web.Razor.Migrations
{
    [Migration(202303011432)]
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

            Create.Index("User", "Username").Unique();
            Create.Index("User", "Email").Unique();

            Create.ForeignKey("UserRole", "User");
            Create.ForeignKey("UserRole", "Role");

            var billw = Guid.NewGuid();
            var bob = Guid.NewGuid();

            Insert.IntoTable("User").Row(new { Id = billw, Username = "billw@fanzootechnology.com", Email = "billw@fanzootechnology.com", Password = "ACCY+b4bvLeFcGENH/SFOrpZCi45WE5bhJDcho+8kT8UX+lzS0kOk46x6DUkvQH5Jw==", LastAuthenticationChange = DateTime.Now, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "system" });
            Insert.IntoTable("User").Row(new { Id = bob, Username = "bob@fanzootechnology.com", Email = "bob@fanzootechnology.com", Password = "ACCY+b4bvLeFcGENH/SFOrpZCi45WE5bhJDcho+8kT8UX+lzS0kOk46x6DUkvQH5Jw==", LastAuthenticationChange = DateTime.Now, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = "system" });

            Insert.IntoTable("Role").Row(new { ApplicationRoleValue.Administrator.Id, ApplicationRoleValue.Administrator.Name, CreatedDate = DateTime.Now, CreatedBy = "system" });
            Insert.IntoTable("Role").Row(new { ApplicationRoleValue.User.Id, ApplicationRoleValue.User.Name, CreatedDate = DateTime.Now, CreatedBy = "system" });

            Insert.IntoTable("UserRole").Row(new { UserId = billw, RoleId = ApplicationRoleValue.Administrator.Id });
            Insert.IntoTable("UserRole").Row(new { UserId = bob, RoleId = ApplicationRoleValue.User.Id });
        }

        public override void Down() { }
    }
}
