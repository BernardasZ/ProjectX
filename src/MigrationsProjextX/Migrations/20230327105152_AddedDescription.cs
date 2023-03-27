using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationsProjextX.Migrations;

/// <inheritdoc />
public partial class AddedDescription : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "Name",
			table: "Tasks",
			newName: "Title");

		migrationBuilder.AddColumn<string>(
			name: "Description",
			table: "Tasks",
			type: "nvarchar(1000)",
			maxLength: 1000,
			nullable: false,
			defaultValue: "");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Description",
			table: "Tasks");

		migrationBuilder.RenameColumn(
			name: "Title",
			table: "Tasks",
			newName: "Name");
	}
}