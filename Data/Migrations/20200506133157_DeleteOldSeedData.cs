using Microsoft.EntityFrameworkCore.Migrations;

namespace Cook_Book_API.Data.Migrations
{
    public partial class DeleteOldSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Ingredients", "Instruction", "IsPublic", "Name", "NameOfImage", "UserId" },
                values: new object[] { 1, "Ziemniaki;Sól", "Pokrój i usmaż ziemniaki. Posól.", false, "Frytki", null, null });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Ingredients", "Instruction", "IsPublic", "Name", "NameOfImage", "UserId" },
                values: new object[] { 2, "Chleb;Masło", "Posmaruj chleb masłem.", false, "Kanapka", null, "7aa6bef9-e1ec-4c38-b70b-89270d1e6a25" });
        }
    }
}
