using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDHP_API.Migrations
{
    /// <inheritdoc />
    public partial class SupportMultipleCoursesPerCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerCourses",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCourses", x => new { x.CoursesId, x.CustomersId });
                    table.ForeignKey(
                        name: "FK_CustomerCourses_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerCourses_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCourses_CustomersId",
                table: "CustomerCourses",
                column: "CustomersId");

            // Copy data to many-to-many join table
            migrationBuilder.Sql("INSERT INTO \"CustomerCourses\" (\"CoursesId\", \"CustomersId\") SELECT \"CourseId\", \"Id\" FROM \"Customers\" WHERE \"CourseId\" IS NOT NULL;");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Courses_CourseId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CourseId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Customers",
                type: "uuid",
                nullable: true);

            // Rollback data from join table back to single CourseId
            migrationBuilder.Sql("UPDATE \"Customers\" SET \"CourseId\" = (SELECT \"CoursesId\" FROM \"CustomerCourses\" WHERE \"CustomerCourses\".\"CustomersId\" = \"Customers\".\"Id\" LIMIT 1);");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CourseId",
                table: "Customers",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Courses_CourseId",
                table: "Customers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.DropTable(
                name: "CustomerCourses");
        }
    }
}
