using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudCash.DAL.Migrations
{
    public partial class VAT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VatLevel",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatLevel",
                table: "Products");
        }
    }
}
