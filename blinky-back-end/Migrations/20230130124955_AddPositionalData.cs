using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blinkybackend.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "posX",
                table: "desks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "posY",
                table: "desks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "posX",
                table: "desks");

            migrationBuilder.DropColumn(
                name: "posY",
                table: "desks");
        }
    }
}
