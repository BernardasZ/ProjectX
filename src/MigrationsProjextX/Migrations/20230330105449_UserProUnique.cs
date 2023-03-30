using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationsProjextX.Migrations;

/// <inheritdoc />
public partial class UserProUnique : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder) =>
		migrationBuilder.CreateIndex(
			name: "IX_Users_Name_Email",
			table: "Users",
			columns: new[] { "Name", "Email" },
			unique: true);

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder) =>
		migrationBuilder.DropIndex(
			name: "IX_Users_Name_Email",
			table: "Users");
}