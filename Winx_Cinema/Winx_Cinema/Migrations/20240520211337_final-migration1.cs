using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Winx_Cinema.Migrations
{
    /// <inheritdoc />
    public partial class finalmigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "06b7173c-4e2e-4496-868b-811f09e34f14", "06b7173c-4e2e-4496-868b-811f09e34f14", "adminUser", "ADMINUSER" },
                    { "e7df795b-97bd-40fe-b7bc-2a908b1a61e9", "e7df795b-97bd-40fe-b7bc-2a908b1a61e9", "ordinaryUser", "ORDINARYUSER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06b7173c-4e2e-4496-868b-811f09e34f14");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7df795b-97bd-40fe-b7bc-2a908b1a61e9");
        }
    }
}
