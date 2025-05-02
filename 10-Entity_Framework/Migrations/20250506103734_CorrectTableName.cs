using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _10_Entity_Framework.Migrations
{
    /// <inheritdoc />
    public partial class CorrectTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterests_Cites_CityId",
                table: "PointOfInterests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cites",
                table: "Cites");

            migrationBuilder.RenameTable(
                name: "Cites",
                newName: "Cities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterests_Cities_CityId",
                table: "PointOfInterests",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterests_Cities_CityId",
                table: "PointOfInterests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "Cites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cites",
                table: "Cites",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterests_Cites_CityId",
                table: "PointOfInterests",
                column: "CityId",
                principalTable: "Cites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
