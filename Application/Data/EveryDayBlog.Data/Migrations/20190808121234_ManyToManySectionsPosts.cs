using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryDayBlog.Data.Migrations
{
    public partial class ManyToManySectionsPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Posts_PostId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_PostId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Sections");

            migrationBuilder.CreateTable(
                name: "SectionsPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionsPosts", x => new { x.PostId, x.SectionId });
                    table.ForeignKey(
                        name: "FK_SectionsPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SectionsPosts_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionsPosts_SectionId",
                table: "SectionsPosts",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectionsPosts");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Sections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_PostId",
                table: "Sections",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Posts_PostId",
                table: "Sections",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
