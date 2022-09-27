using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsBacklog.Migrations.Application
{
    public partial class sqlbaseforprojects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Wikis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Wikis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Wikis",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Wikis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "WikiOlds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "WikiOlds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WikiOlds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "WikiOlds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ProjectNews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "ProjectNews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectNews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ProjectNews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ProjectMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "ProjectMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ProjectMembers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Files",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "FileChangeLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "FileChangeLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FileChangeLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "FileChangeLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Wikis");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Wikis");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Wikis");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Wikis");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "WikiOlds");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "WikiOlds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WikiOlds");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "WikiOlds");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ProjectNews");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ProjectNews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectNews");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ProjectNews");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "FileChangeLogs");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "FileChangeLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FileChangeLogs");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "FileChangeLogs");
        }
    }
}
