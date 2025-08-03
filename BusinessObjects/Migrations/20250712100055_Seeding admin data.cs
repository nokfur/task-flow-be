using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Seedingadmindata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "Password", "Role", "Salt" },
                values: new object[] { "17d88407-91ce-4b33-8fbb-9639b12a495e", new DateTime(2025, 5, 22, 15, 30, 0, 0, DateTimeKind.Unspecified), "admin", "Admin", "Vcdy30nqeMZFh2FCVp2F8uktoTqQcJcKU6Bf0oS2o30=", "Admin", "8GpL6j9M7maqTG/s928A0w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "17d88407-91ce-4b33-8fbb-9639b12a495e");
        }
    }
}
