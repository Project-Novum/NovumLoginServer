using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovumLoginServer.Migrations
{
    public partial class MovedSessionsToRedis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions",
                schema: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                schema: "Accounts",
                columns: table => new
                {
                    ID = table.Column<string>(type: "character varying(56)", maxLength: 56, nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.ID);
                });
        }
    }
}
