using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDHP_API.Migrations
{
    /// <inheritdoc />
    public partial class AddSortIndexToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "Courses");
        }
    }
}
