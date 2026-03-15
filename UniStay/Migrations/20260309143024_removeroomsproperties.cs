using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.Migrations
{
    /// <inheritdoc />
    public partial class removeroomsproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FloorMaterial",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "HasAC",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "HasBalcony",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "HasFridge",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "HasPrivateBathroom",
                table: "Room");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FloorMaterial",
                table: "Room",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAC",
                table: "Room",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasBalcony",
                table: "Room",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFridge",
                table: "Room",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPrivateBathroom",
                table: "Room",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }
    }
}
