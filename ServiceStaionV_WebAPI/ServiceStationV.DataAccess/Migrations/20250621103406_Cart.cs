using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceStationV.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavourites_Services_ServiceId",
                table: "UserFavourites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavourites_Users_UserId",
                table: "UserFavourites");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.UserId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_Carts_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ServiceId",
                table: "Carts",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavourites_Services_ServiceId",
                table: "UserFavourites",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavourites_Users_UserId",
                table: "UserFavourites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavourites_Services_ServiceId",
                table: "UserFavourites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavourites_Users_UserId",
                table: "UserFavourites");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavourites_Services_ServiceId",
                table: "UserFavourites",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavourites_Users_UserId",
                table: "UserFavourites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
