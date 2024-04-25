using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcBach.Migrations
{
    /// <inheritdoc />
    public partial class Cre_Table_HS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HS",
                columns: table => new
                {
                    HocsinhID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HS", x => x.HocsinhID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HS");
        }
    }
}
