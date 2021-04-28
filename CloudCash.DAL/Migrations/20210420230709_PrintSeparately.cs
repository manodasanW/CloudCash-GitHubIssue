using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudCash.DAL.Migrations
{
    public partial class PrintSeparately : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PrintSeparately",
                table: "ProductCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrintSeparately",
                table: "ProductCategories");
        }
    }
}
