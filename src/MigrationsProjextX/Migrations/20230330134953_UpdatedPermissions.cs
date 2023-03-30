using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationsProjextX.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "LogoutAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "ChangePasswordAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "CheckSession");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ControllerId", "Name" },
                values: new object[] { 1, "All" });

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "GetAllByUserIdAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "CreateAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "GetByIdAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "UpdateAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "DeleteAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ControllerId", "Name" },
                values: new object[] { 2, "All" });

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "GetAllAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "GetByIdAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "UpdateAsync");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "DeleteAsync");

            migrationBuilder.InsertData(
                table: "PermissionActions",
                columns: new[] { "Id", "ControllerId", "Name" },
                values: new object[] { 15, 3, "All" });

            migrationBuilder.UpdateData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ActionId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 2,
                column: "ActionId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 3,
                column: "ActionId",
                value: 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Logout");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "ChangePassword");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "All");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ControllerId", "Name" },
                values: new object[] { 2, "GetAllByUserId" });

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Create");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "GetById");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Update");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Delete");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "All");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ControllerId", "Name" },
                values: new object[] { 3, "GetAll" });

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "GetById");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Update");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "Delete");

            migrationBuilder.UpdateData(
                table: "PermissionActions",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "All");

            migrationBuilder.UpdateData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ActionId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 2,
                column: "ActionId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "PermissionMappings",
                keyColumn: "Id",
                keyValue: 3,
                column: "ActionId",
                value: 14);
        }
    }
}
