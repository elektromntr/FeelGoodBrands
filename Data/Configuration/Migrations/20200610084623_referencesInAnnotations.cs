using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Configuration.Migrations
{
    public partial class referencesInAnnotations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Medias_ImageId",
                table: "Medias",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_CoverId",
                table: "Brands",
                column: "CoverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Attachments_CoverId",
                table: "Brands",
                column: "CoverId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Attachments_ImageId",
                table: "Medias",
                column: "ImageId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Attachments_CoverId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Attachments_ImageId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Medias_ImageId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Brands_CoverId",
                table: "Brands");
        }
    }
}
