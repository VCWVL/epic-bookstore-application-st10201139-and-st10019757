using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicBookstoreSprint.Migrations
{
    public partial class updateImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Book",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Book");
        }
    }
}
