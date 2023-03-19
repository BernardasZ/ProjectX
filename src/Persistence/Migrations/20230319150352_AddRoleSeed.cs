using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
	/// <inheritdoc />
	public partial class AddRoleSeed : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "Roles",
				columns: new[] { "Id", "Name", "Value" },
				values: new object[,]
				{
					{ 1, "Admin", 0 },
					{ 2, "User", 1 },
					{ 3, "AllRoles", 255 }
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "Roles",
				keyColumn: "Id",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Roles",
				keyColumn: "Id",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Roles",
				keyColumn: "Id",
				keyValue: 3);
		}
	}
}