using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddmanymanyUserBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_User_OwnerId",
                table: "Board");

            migrationBuilder.DropIndex(
                name: "IX_Board_OwnerId",
                table: "Board");

            migrationBuilder.CreateTable(
                name: "BoardMember",
                columns: table => new
                {
                    BoardId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMember", x => new { x.BoardId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_BoardMember_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardMember_User_MemberId",
                        column: x => x.MemberId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMember_MemberId",
                table: "BoardMember",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardMember");

            migrationBuilder.CreateIndex(
                name: "IX_Board_OwnerId",
                table: "Board",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_User_OwnerId",
                table: "Board",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
