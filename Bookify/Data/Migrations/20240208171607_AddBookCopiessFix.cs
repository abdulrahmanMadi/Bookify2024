using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookCopiessFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Books_BookId",
                table: "BookCopies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCopies",
                table: "BookCopies");

            migrationBuilder.RenameTable(
                name: "BookCopies",
                newName: "Copies");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopies_BookId",
                table: "Copies",
                newName: "IX_Copies_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Copies",
                table: "Copies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Copies_Books_BookId",
                table: "Copies",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Copies_Books_BookId",
                table: "Copies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Copies",
                table: "Copies");

            migrationBuilder.RenameTable(
                name: "Copies",
                newName: "BookCopies");

            migrationBuilder.RenameIndex(
                name: "IX_Copies_BookId",
                table: "BookCopies",
                newName: "IX_BookCopies_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCopies",
                table: "BookCopies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Books_BookId",
                table: "BookCopies",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
