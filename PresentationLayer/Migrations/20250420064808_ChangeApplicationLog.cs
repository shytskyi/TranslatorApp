using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresentationLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeApplicationLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ApplicationLogs",
                newName: "ResponseBody");

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "ApplicationLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ApplicationLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QueryString",
                table: "ApplicationLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestBody",
                table: "ApplicationLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "ApplicationLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "ApplicationLogs");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "ApplicationLogs");

            migrationBuilder.DropColumn(
                name: "QueryString",
                table: "ApplicationLogs");

            migrationBuilder.DropColumn(
                name: "RequestBody",
                table: "ApplicationLogs");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "ApplicationLogs");

            migrationBuilder.RenameColumn(
                name: "ResponseBody",
                table: "ApplicationLogs",
                newName: "Message");
        }
    }
}
