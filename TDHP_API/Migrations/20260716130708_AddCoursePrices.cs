using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDHP_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCoursePrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceAdults1",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceAdults2",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceKids1",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceKids2",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceAdults1",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PriceAdults2",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PriceKids1",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PriceKids2",
                table: "Courses");
        }
    }
}
