using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Winx_Cinema.Migrations
{
    /// <inheritdoc />
    public partial class change_hall_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Halls",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Halls");
        }
    }
}
