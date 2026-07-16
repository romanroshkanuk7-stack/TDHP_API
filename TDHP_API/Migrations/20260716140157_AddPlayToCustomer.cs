using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDHP_API.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlayId",
                table: "Customers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PlayId",
                table: "Customers",
                column: "PlayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Plays_PlayId",
                table: "Customers",
                column: "PlayId",
                principalTable: "Plays",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Plays_PlayId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_PlayId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PlayId",
                table: "Customers");
        }
    }
}
