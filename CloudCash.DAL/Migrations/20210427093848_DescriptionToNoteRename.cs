using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudCash.DAL.Migrations
{
    public partial class DescriptionToNoteRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Payments",
                newName: "Note");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Payments",
                newName: "Description");
        }
    }
}
