using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restapi_crud_practice.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientStruct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Clients",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Clients",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Clients",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordChangedAt",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PasswordMaxAgeDays",
                table: "Clients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Clients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordChangedAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PasswordMaxAgeDays",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Clients",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Clients",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Clients",
                newName: "Email");
        }
    }
}
