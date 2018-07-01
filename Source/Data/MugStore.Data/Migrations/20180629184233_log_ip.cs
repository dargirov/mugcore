using Microsoft.EntityFrameworkCore.Migrations;

namespace MugStore.Data.Migrations
{
    public partial class log_ip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Logs",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Logs");
        }
    }
}
