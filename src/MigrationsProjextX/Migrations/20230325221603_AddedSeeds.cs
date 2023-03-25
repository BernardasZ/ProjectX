using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MigrationsProjextX.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermissionControllers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Authentication" },
                    { 2, "Tasks" },
                    { 3, "Users" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FailedLoginCount", "IsTokenUsed", "Name", "PassHash", "RoleId", "TokenExpirationTime", "TokenHash" },
                values: new object[] { 1, "admin@projectx.com", 0, null, "projectxadmin", "E5scnWql/WJsaL0tYvsNKVbYP8ZJR0s0WNNhCjLlcXw=", 1, null, null });

            migrationBuilder.InsertData(
                table: "PermissionActions",
                columns: new[] { "Id", "ControllerId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Logout" },
                    { 2, 1, "ChangePassword" },
                    { 3, 1, "All" },
                    { 4, 2, "GetAllByUserId" },
                    { 5, 2, "Create" },
                    { 6, 2, "GetById" },
                    { 7, 2, "Update" },
                    { 8, 2, "Delete" },
                    { 9, 2, "All" },
                    { 10, 3, "GetAll" },
                    { 11, 3, "GetById" },
                    { 12, 3, "Update" },
                    { 13, 3, "Delete" },
                    { 14, 3, "All" }
                });

            migrationBuilder.InsertData(
                table: "PermissionMappings",
                columns: new[] { "Id", "ActionId", "AllowAllActions", "ControllerId", "RoleId" },
                values: new object[,]
                {
                    { 1, 3, true, 1, 1 },
                    { 2, 9, true, 2, 1 },
                    { 3, 14, true, 3, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "PermissionControllers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PermissionControllers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PermissionControllers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
