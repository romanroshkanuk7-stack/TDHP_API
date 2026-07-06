using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDHP_API.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ButtonLink",
                table: "Programs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Programs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Programs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonLink",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Programs");
        }
    }
}
