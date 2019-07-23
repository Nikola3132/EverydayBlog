using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryDayBlog.Data.Migrations
{
    public partial class imageUrlNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CloudUrl",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CloudUrl",
                table: "Images",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
