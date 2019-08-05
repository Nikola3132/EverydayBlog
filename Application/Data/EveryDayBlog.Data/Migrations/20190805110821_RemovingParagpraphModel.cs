using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryDayBlog.Data.Migrations
{
    public partial class RemovingParagpraphModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paragraphs");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Sections",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Sections",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                maxLength: 56,
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Sections");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Sections",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 56);

            migrationBuilder.CreateTable(
                name: "Paragraphs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ImageId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MainText = table.Column<string>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    SectionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraphs_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paragraphs_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_ImageId",
                table: "Paragraphs",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_IsDeleted",
                table: "Paragraphs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Paragraphs_SectionId",
                table: "Paragraphs",
                column: "SectionId");
        }
    }
}
