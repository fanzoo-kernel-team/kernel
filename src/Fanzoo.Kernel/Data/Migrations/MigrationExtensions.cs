using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Create.Index;
using FluentMigrator.Builders.Create.Table;

namespace FluentMigrator
{
    public static class MigrationExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax AsImmutable(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax table) =>
            table
                .WithColumn("Id")
                    .AsGuid()
                    .PrimaryKey()
                    .NotNullable()
                .WithColumn("CreatedDate")
                    .AsDateTime()
                    .NotNullable()
                .WithColumn("CreatedBy")
                    .AsString(DatabaseCatalog.FieldLength.Default)
                    .NotNullable();

        public static ICreateTableColumnOptionOrWithColumnSyntax AsMutable(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax table) =>
            table
                .AsImmutable()
                .WithColumn("UpdatedDate")
                    .AsDateTime()
                    .Nullable()
                .WithColumn("UpdatedBy")
                    .AsString(DatabaseCatalog.FieldLength.Default)
                    .Nullable();

        public static ICreateTableColumnOptionOrWithColumnSyntax WithNameColumn(this ICreateTableColumnOptionOrWithColumnSyntax table, bool required = true) =>
            table
                .WithColumn("Name")
                    .AsString(DatabaseCatalog.FieldLength.Default)
                    .Required(required);

        public static ICreateTableColumnOptionOrWithColumnSyntax WithEmailColumn(this ICreateTableColumnOptionOrWithColumnSyntax table, string prefix = "", bool required = true) =>
            table
                .WithColumn(prefix + "Email")
                    .AsString(DatabaseCatalog.FieldLength.Email)
                    .Required(required);

        public static ICreateTableColumnOptionOrWithColumnSyntax WithPhoneColumn(this ICreateTableColumnOptionOrWithColumnSyntax table, string prefix = "", bool required = true) =>
            table
                .WithColumn(prefix + "Phone")
                    .AsString(DatabaseCatalog.FieldLength.Phone)
                    .Required(required);

        public static ICreateTableColumnOptionOrWithColumnSyntax WithAddressColumns(this ICreateTableColumnOptionOrWithColumnSyntax table, string prefix = "", bool required = true) =>
            table
                .WithColumn(prefix + "PrimaryAddress")
                    .AsString(DatabaseCatalog.FieldLength.Default)
                    .Required(required)
                .WithColumn(prefix + "SecondaryAddress")
                    .AsString(DatabaseCatalog.FieldLength.Default)
                    .Required(false)
                .WithColumn(prefix + "City")
                    .AsString(DatabaseCatalog.FieldLength.Default)
                    .Required(required)
                .WithColumn(prefix + "Region")
                    .AsString(DatabaseCatalog.FieldLength.Region)
                    .Required(required)
                .WithColumn(prefix + "PostalCode")
                    .AsString(DatabaseCatalog.FieldLength.PostalCode)
                    .Required(required);

        public static ICreateTableColumnOptionOrWithColumnSyntax WithMoneyColumns(this ICreateTableColumnOptionOrWithColumnSyntax table, string prefix = "", bool required = true) =>
            table
                .WithColumn(prefix + "Amount")
                    .AsDecimal()
                    .Required(required)
                .WithColumn(prefix + "Currency")
                    .AsString(DatabaseCatalog.FieldLength.CurrencyCode)
                    .Required(required);

        public static ICreateTableColumnOptionOrWithColumnSyntax WithActiveColumn(this ICreateTableColumnOptionOrWithColumnSyntax table) =>
            table
                .WithColumn("IsActive")
                    .AsBoolean()
                    .NotNullable()
                    .WithDefaultValue(true);

        public static ICreateTableColumnOptionOrWithColumnSyntax WithForeignKeyColumn(this ICreateTableColumnOptionOrWithColumnSyntax table, string toTable, bool required = true) =>
            table
                .WithColumn($"{toTable}Id")
                    .AsGuid()
                    .Required(required);

        public static void ForeignKey(this ICreateExpressionRoot create, string fromTable, string toTable, string? foreignColumn = null /*Rule onDelete = Rule.SetDefault, Rule onUpdate = Rule.SetDefault*/)
        {
            foreignColumn ??= $"{toTable}Id";

            create.ForeignKey($"FK_{fromTable}_{toTable}_{foreignColumn}")
                .FromTable(fromTable)
                    .ForeignColumn(foreignColumn)
                        .ToTable(toTable)
                            .PrimaryColumn("Id");
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax Required(this ICreateTableColumnOptionOrWithColumnSyntax table, bool required) => required ? table.NotNullable() : table.Nullable();

        public static ICreateIndexColumnOptionsSyntax Index(this ICreateExpressionRoot create, string table, string column) =>
            create.Index($"IX_{table}_{column}")
                .OnTable(table)
                    .OnColumn(column);

        public static ICreateTableColumnOptionOrWithColumnSyntax WithDateTimeColumn(this ICreateTableColumnOptionOrWithColumnSyntax table, string columnName, bool required = true) =>
            table
                .WithColumn(columnName)
                    .AsDateTime2()
                        .Required(required);

    }
}
