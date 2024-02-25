using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpicBookstoreSprint.Migrations
{
    public partial class removedMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Book",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Book");
        }
    }
}
