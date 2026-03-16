using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalisApi.Migrations
{
    /// <inheritdoc />
    public partial class rutines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rutines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rutines_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RutineExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Exercise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reps = table.Column<int>(type: "int", nullable: false),
                    Series = table.Column<int>(type: "int", nullable: false),
                    Descanso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RutineExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RutineExercises_Rutines_RutineId",
                        column: x => x.RutineId,
                        principalTable: "Rutines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RutineExercises_RutineId",
                table: "RutineExercises",
                column: "RutineId");

            migrationBuilder.CreateIndex(
                name: "IX_Rutines_CategoryId",
                table: "Rutines",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RutineExercises");

            migrationBuilder.DropTable(
                name: "Rutines");
        }
    }
}
