using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fullstack.Api.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "employees",
                newName: "Phone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "employees",
                newName: "Number");
        }
    }
}
