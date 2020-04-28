using Microsoft.EntityFrameworkCore.Migrations;

namespace Cook_Book_API.Data.Migrations
{
    public partial class AddFavouritesRecipes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavouriteRecipes",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavouriteRecipes",
                table: "AspNetUsers");
        }
    }
}
