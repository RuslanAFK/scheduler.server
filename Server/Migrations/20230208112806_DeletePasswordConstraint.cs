using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class DeletePasswordConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
