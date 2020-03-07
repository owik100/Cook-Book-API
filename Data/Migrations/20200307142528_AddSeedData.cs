using Microsoft.EntityFrameworkCore.Migrations;

namespace Cook_Book_API.Data.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "RecipesId", "Ingredients", "Instruction", "Name", "NameOfImage" },
                values: new object[] { 1, "Ziemniaki;Sól", "Pokrój i usmaż ziemniaki. Posól.", "Frytki", null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "RecipesId", "Ingredients", "Instruction", "Name", "NameOfImage" },
                values: new object[] { 2, "Chleb;Masło", "Posmaruj chleb masłem.", "Kanapka", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "RecipesId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "RecipesId",
                keyValue: 2);
        }
    }
}
