using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Configuration.Migrations
{
    public partial class attachmentsCarousel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarouselOrder",
                table: "Attachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "InCarousel",
                table: "Attachments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarouselOrder",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "InCarousel",
                table: "Attachments");
        }
    }
}
