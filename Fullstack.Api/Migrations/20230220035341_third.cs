using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fullstack.Api.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
