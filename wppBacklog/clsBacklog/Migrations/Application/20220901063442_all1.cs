using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsBacklog.Migrations.Application
{
    public partial class all1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ParentWikiId",
                table: "Wikis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ParentWikiId",
                table: "WikiOlds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
       
            migrationBuilder.AlterColumn<string>(
                name: "ParentWikiId",
                table: "Wikis",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ParentWikiId",
                table: "WikiOlds",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
