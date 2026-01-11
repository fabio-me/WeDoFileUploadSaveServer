using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeDoFileUploadSaveServer.Migrations
{
    /// <inheritdoc />
    public partial class fileDb_newName_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentBytes",
                table: "FileDb",
                newName: "Data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "FileDb",
                newName: "ContentBytes");
        }
    }
}
