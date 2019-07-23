using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryDayBlog.Data.Migrations
{
    public partial class ImageByteRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageByte",
                table: "Images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageByte",
                table: "Images",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
