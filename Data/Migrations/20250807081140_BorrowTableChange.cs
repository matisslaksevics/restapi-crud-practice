using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restapi_crud_practice.Data.Migrations
{
    /// <inheritdoc />
    public partial class BorrowTableChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOverdue",
                table: "Borrows",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOverdue",
                table: "Borrows");
        }
    }
}
