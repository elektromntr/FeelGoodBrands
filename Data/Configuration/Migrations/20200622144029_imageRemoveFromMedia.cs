using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Configuration.Migrations
{
    public partial class imageRemoveFromMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Attachments_ImageId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Medias_ImageId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Medias");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Medias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Medias_ImageId",
                table: "Medias",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Attachments_ImageId",
                table: "Medias",
                column: "ImageId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
