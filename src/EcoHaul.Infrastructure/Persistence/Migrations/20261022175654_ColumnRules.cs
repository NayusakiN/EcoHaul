using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoHaul.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ColumnRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WeightKg",
                table: "Pickups",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "SiteName",
                table: "Pickups",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Pickups",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "OutboxMessages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OperationId",
                table: "Notifications",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ETag",
                table: "LoadPhotos",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "LoadPhotos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BlobName",
                table: "LoadPhotos",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Pickups_CustomerId_RequestedOn",
                table: "Pickups",
                columns: new[] { "CustomerId", "RequestedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_SentAt_CreatedAt",
                table: "OutboxMessages",
                columns: new[] { "SentAt", "CreatedAt" },
                filter: "[SentAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PickupId",
                table: "Notifications",
                column: "PickupId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadPhotos_BlobName",
                table: "LoadPhotos",
                column: "BlobName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoadPhotos_PickupId",
                table: "LoadPhotos",
                column: "PickupId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadPhotos_Pickups_PickupId",
                table: "LoadPhotos",
                column: "PickupId",
                principalTable: "Pickups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Pickups_PickupId",
                table: "Notifications",
                column: "PickupId",
                principalTable: "Pickups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadPhotos_Pickups_PickupId",
                table: "LoadPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Pickups_PickupId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Pickups_CustomerId_RequestedOn",
                table: "Pickups");

            migrationBuilder.DropIndex(
                name: "IX_OutboxMessages_SentAt_CreatedAt",
                table: "OutboxMessages");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_PickupId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_LoadPhotos_BlobName",
                table: "LoadPhotos");

            migrationBuilder.DropIndex(
                name: "IX_LoadPhotos_PickupId",
                table: "LoadPhotos");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeightKg",
                table: "Pickups",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "SiteName",
                table: "Pickups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Pickups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "OutboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "OperationId",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ETag",
                table: "LoadPhotos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "LoadPhotos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "BlobName",
                table: "LoadPhotos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
