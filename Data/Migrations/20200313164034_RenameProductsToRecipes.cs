using Microsoft.EntityFrameworkCore.Migrations;

namespace Cook_Book_API.Data.Migrations
{
    public partial class RenameProductsToRecipes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Ingredients = table.Column<string>(nullable: true),
                    Instruction = table.Column<string>(nullable: true),
                    NameOfImage = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Ingredients", "Instruction", "Name", "NameOfImage", "UserId" },
                values: new object[] { 1, "Ziemniaki;Sól", "Pokrój i usmaż ziemniaki. Posól.", "Frytki", null, null });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "Ingredients", "Instruction", "Name", "NameOfImage", "UserId" },
                values: new object[] { 2, "Chleb;Masło", "Posmaruj chleb masłem.", "Kanapka", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    RecipesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.RecipesId);
                    table.ForeignKey(
                        name: "FK_Products_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "RecipesId", "Ingredients", "Instruction", "Name", "NameOfImage", "UserId" },
                values: new object[] { 1, "Ziemniaki;Sól", "Pokrój i usmaż ziemniaki. Posól.", "Frytki", null, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "RecipesId", "Ingredients", "Instruction", "Name", "NameOfImage", "UserId" },
                values: new object[] { 2, "Chleb;Masło", "Posmaruj chleb masłem.", "Kanapka", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                table: "Products",
                column: "UserId");
        }
    }
}
