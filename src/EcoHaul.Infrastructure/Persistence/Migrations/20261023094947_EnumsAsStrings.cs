using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoHaul.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EnumsAsStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ConvertPickupEnum(migrationBuilder, "Material", """
                CASE [Material]
                    WHEN 0 THEN N'Paper'
                    WHEN 1 THEN N'Plastic'
                    WHEN 2 THEN N'Glass'
                    WHEN 3 THEN N'Electronics'
                    ELSE N'Paper'
                END
                """);

            ConvertPickupEnum(migrationBuilder, "Status", """
                CASE [Status]
                    WHEN 0 THEN N'Draft'
                    WHEN 1 THEN N'Scheduled'
                    WHEN 2 THEN N'Collected'
                    ELSE N'Draft'
                END
                """);

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                table: "Notifications",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE [Notifications]
                SET [StateName] = CASE [State]
                    WHEN 0 THEN N'Queued'
                    WHEN 1 THEN N'Accepted'
                    WHEN 2 THEN N'Failed'
                    ELSE N'Queued'
                END
                """);

            migrationBuilder.DropColumn(name: "State", table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "StateName",
                table: "Notifications",
                newName: "State");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Notifications",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ConvertPickupEnumDown(migrationBuilder, "Material", """
                CASE [Material]
                    WHEN N'Paper' THEN 0
                    WHEN N'Plastic' THEN 1
                    WHEN N'Glass' THEN 2
                    WHEN N'Electronics' THEN 3
                    ELSE 0
                END
                """);

            ConvertPickupEnumDown(migrationBuilder, "Status", """
                CASE [Status]
                    WHEN N'Draft' THEN 0
                    WHEN N'Scheduled' THEN 1
                    WHEN N'Collected' THEN 2
                    ELSE 0
                END
                """);

            migrationBuilder.AddColumn<int>(
                name: "StateValue",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE [Notifications]
                SET [StateValue] = CASE [State]
                    WHEN N'Queued' THEN 0
                    WHEN N'Accepted' THEN 1
                    WHEN N'Failed' THEN 2
                    ELSE 0
                END
                """);

            migrationBuilder.DropColumn(name: "State", table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "StateValue",
                table: "Notifications",
                newName: "State");

            migrationBuilder.AlterColumn<int>(
                name: "State",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        private static void ConvertPickupEnum(MigrationBuilder migrationBuilder, string column, string caseSql)
        {
            var temp = column + "Name";
            migrationBuilder.AddColumn<string>(
                name: temp,
                table: "Pickups",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.Sql($"UPDATE [Pickups] SET [{temp}] = {caseSql}");
            migrationBuilder.DropColumn(name: column, table: "Pickups");
            migrationBuilder.RenameColumn(name: temp, table: "Pickups", newName: column);
            migrationBuilder.AlterColumn<string>(
                name: column,
                table: "Pickups",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);
        }

        private static void ConvertPickupEnumDown(MigrationBuilder migrationBuilder, string column, string caseSql)
        {
            var temp = column + "Value";
            migrationBuilder.AddColumn<int>(
                name: temp,
                table: "Pickups",
                type: "int",
                nullable: true);

            migrationBuilder.Sql($"UPDATE [Pickups] SET [{temp}] = {caseSql}");
            migrationBuilder.DropColumn(name: column, table: "Pickups");
            migrationBuilder.RenameColumn(name: temp, table: "Pickups", newName: column);
            migrationBuilder.AlterColumn<int>(
                name: column,
                table: "Pickups",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
