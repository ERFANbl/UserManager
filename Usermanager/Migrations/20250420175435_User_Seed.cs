using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Usermanager.Migrations
{
    /// <inheritdoc />
    public partial class User_Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "CityId", "FirstName", "LastName", "PersonnelNumber", "PhoneNumber", "ProvinceId" },
                values: new object[,]
                {
                    { 1, 1, "علی", "رضایی", "EMP1001", "09123456789", 1 },
                    { 2, 2, "فاطمه", "محمدی", "EMP1002", "09129876543", 2 },
                    { 3, 3, "رضا", "کریمی", "EMP1003", "09151234567", 3 },
                    { 4, 4, "سارا", "نجفی", "EMP1004", "09351234567", 4 },
                    { 5, 5, "محمد", "حسینی", "EMP1005", "09161234567", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
