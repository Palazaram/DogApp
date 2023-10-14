using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DogApp.Data
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    Color = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    TailLength = table.Column<double>(type: "FLOAT", nullable: false),
                    Weight = table.Column<double>(type: "FLOAT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.Id);
                    table.CheckConstraint("CK_TailLength", "TailLength >= 0");
                    table.CheckConstraint("CK_Weight", "Weight >= 0");
                });

            migrationBuilder.InsertData(
                table: "Dogs",
                columns: new[] { "Id", "Color", "Name", "TailLength", "Weight" },
                values: new object[,]
                {
                    { 1, "Black", "Jessy", 25.0, 5.0 },
                    { 2, "White", "Neo", 15.0, 3.0 },
                    { 3, "Brown", "Doggy", 50.0, 15.0 },
                    { 4, "White&Brown", "Chessy", 10.0, 2.0 },
                    { 5, "Gray", "Bob", 60.0, 20.0 },
                    { 6, "Black", "Jessy", 25.0, 5.0 },
                    { 7, "Gold", "Marko", 70.0, 30.0 },
                    { 8, "Black", "Fido", 100.0, 25.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dogs");
        }
    }
}
