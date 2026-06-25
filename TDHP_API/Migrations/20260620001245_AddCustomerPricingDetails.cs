using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDHP_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerPricingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LessonsFrequency",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Customers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LessonsFrequency",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Customers");
        }
    }
}
