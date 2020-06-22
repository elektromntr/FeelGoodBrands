using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Configuration.Migrations
{
    public partial class mediasReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Medias");

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Medias",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Medias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Attachments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_BrandId",
                table: "Medias",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_BrandId",
                table: "Attachments",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Brands_BrandId",
                table: "Attachments",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Brands_BrandId",
                table: "Medias",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Brands_BrandId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Brands_BrandId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Medias_BrandId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_BrandId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Attachments");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Medias",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
