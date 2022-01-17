using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovumLoginServer.Migrations
{
    public partial class StoreGameSessionWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameSessionId",
                schema: "Accounts",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameSessionId",
                schema: "Accounts",
                table: "Users");
        }
    }
}
