using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsBacklog.Migrations.Application
{
    public partial class taskcompletaionreason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskCompletionReason",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskCompletionReason",
                table: "Tasks");
        }
    }
}
