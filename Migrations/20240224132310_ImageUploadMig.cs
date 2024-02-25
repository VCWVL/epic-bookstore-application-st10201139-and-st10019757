using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicBookstoreSprint.Migrations
{
    public partial class ImageUploadMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Book",
                newName: "ImageMimeType");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Book",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "ImageMimeType",
                table: "Book",
                newName: "ImageUrl");
        }
    }
}
