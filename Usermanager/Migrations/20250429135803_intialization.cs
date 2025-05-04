using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Usermanager.Migrations
{
    /// <inheritdoc />
    public partial class intialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Accessibility = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupRole",
                columns: table => new
                {
                    GroupsId = table.Column<int>(type: "int", nullable: false),
                    RolesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRole", x => new { x.GroupsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_GroupRole_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PersonnelNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "تهران" },
                    { 2, "البرز" },
                    { 3, "اصفهان" },
                    { 4, "فارس" },
                    { 5, "خراسان رضوی" },
                    { 6, "آذربایجان شرقی" },
                    { 7, "آذربایجان غربی" },
                    { 8, "گیلان" },
                    { 9, "مازندران" },
                    { 10, "کرمان" },
                    { 11, "هرمزگان" },
                    { 12, "خوزستان" },
                    { 13, "کردستان" },
                    { 14, "کرمانشاه" },
                    { 15, "یزد" },
                    { 16, "زنجان" },
                    { 17, "همدان" },
                    { 18, "قزوین" },
                    { 19, "قم" },
                    { 20, "سمنان" },
                    { 21, "ایلام" },
                    { 22, "کهگیلویه و بویراحمد" },
                    { 23, "چهارمحال و بختیاری" },
                    { 24, "لرستان" },
                    { 25, "مرکزی" },
                    { 26, "بوشهر" },
                    { 27, "سیستان و بلوچستان" },
                    { 28, "گلستان" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "ProvinceId" },
                values: new object[,]
                {
                    { 1, "تهران", 1 },
                    { 2, "ری", 1 },
                    { 3, "دماوند", 1 },
                    { 4, "پردیس", 1 },
                    { 5, "ورامین", 1 },
                    { 6, "شهریار", 1 },
                    { 7, "اسلام‌شهر", 1 },
                    { 8, "ملارد", 1 },
                    { 9, "پاکدشت", 1 },
                    { 10, "کرج", 2 },
                    { 11, "نظرآباد", 2 },
                    { 12, "طالقان", 2 },
                    { 13, "اشتهارد", 2 },
                    { 14, "فردیس", 2 },
                    { 15, "اصفهان", 3 },
                    { 16, "کاشان", 3 },
                    { 17, "خمینی‌شهر", 3 },
                    { 18, "نجف‌آباد", 3 },
                    { 19, "شهرضا", 3 },
                    { 20, "فلاورجان", 3 },
                    { 21, "مبارکه", 3 },
                    { 22, "شیراز", 4 },
                    { 23, "مرودشت", 4 },
                    { 24, "جهرم", 4 },
                    { 25, "لار", 4 },
                    { 26, "فسا", 4 },
                    { 27, "کازرون", 4 },
                    { 28, "داراب", 4 },
                    { 29, "مشهد", 5 },
                    { 30, "نیشابور", 5 },
                    { 31, "سبزوار", 5 },
                    { 32, "تربت‌حیدریه", 5 },
                    { 33, "قوچان", 5 },
                    { 34, "چناران", 5 },
                    { 35, "تبریز", 6 },
                    { 36, "مراغه", 6 },
                    { 37, "مرند", 6 },
                    { 38, "میانه", 6 },
                    { 39, "اهر", 6 },
                    { 40, "بناب", 6 },
                    { 41, "ارومیه", 7 },
                    { 42, "خوی", 7 },
                    { 43, "بوکان", 7 },
                    { 44, "مهاباد", 7 },
                    { 45, "سلماس", 7 },
                    { 46, "میاندوآب", 7 },
                    { 47, "رشت", 8 },
                    { 48, "لاهیجان", 8 },
                    { 49, "انزلی", 8 },
                    { 50, "آستارا", 8 },
                    { 51, "تالش", 8 },
                    { 52, "صومعه‌سرا", 8 },
                    { 53, "ساری", 9 },
                    { 54, "بابل", 9 },
                    { 55, "آمل", 9 },
                    { 56, "نور", 9 },
                    { 57, "تنکابن", 9 },
                    { 58, "قائم‌شهر", 9 },
                    { 59, "کرمان", 10 },
                    { 60, "رفسنجان", 10 },
                    { 61, "جیرفت", 10 },
                    { 62, "سیرجان", 10 },
                    { 63, "بم", 10 },
                    { 64, "بندرعباس", 11 },
                    { 65, "قشم", 11 },
                    { 66, "بندر لنگه", 11 },
                    { 67, "میناب", 11 },
                    { 68, "حاجی‌آباد", 11 },
                    { 69, "اهواز", 12 },
                    { 70, "آبادان", 12 },
                    { 71, "خرمشهر", 12 },
                    { 72, "دزفول", 12 },
                    { 73, "ماهشهر", 12 },
                    { 74, "شادگان", 12 },
                    { 75, "سنندج", 13 },
                    { 76, "سقز", 13 },
                    { 77, "بانه", 13 },
                    { 78, "بیجار", 13 },
                    { 79, "قروه", 13 },
                    { 80, "کرمانشاه", 14 },
                    { 81, "اسلام‌آباد غرب", 14 },
                    { 82, "سرپل ذهاب", 14 },
                    { 83, "قصر شیرین", 14 },
                    { 84, "سنقر", 14 },
                    { 85, "یزد", 15 },
                    { 86, "میبد", 15 },
                    { 87, "اردکان", 15 },
                    { 88, "ابرکوه", 15 },
                    { 89, "تفت", 15 },
                    { 90, "زنجان", 16 },
                    { 91, "ابهر", 16 },
                    { 92, "خرمدره", 16 },
                    { 93, "طارم", 16 },
                    { 94, "ماه‌نشان", 16 },
                    { 95, "همدان", 17 },
                    { 96, "ملایر", 17 },
                    { 97, "نهاوند", 17 },
                    { 98, "تویسرکان", 17 },
                    { 99, "رزن", 17 },
                    { 100, "قزوین", 18 },
                    { 101, "الوند", 18 },
                    { 102, "آبیک", 18 },
                    { 103, "تاکستان", 18 },
                    { 104, "قم", 19 },
                    { 105, "سمنان", 20 },
                    { 106, "شاهرود", 20 },
                    { 107, "دامغان", 20 },
                    { 108, "گرمسار", 20 },
                    { 109, "ایلام", 21 },
                    { 110, "دهلران", 21 },
                    { 111, "مهران", 21 },
                    { 112, "آبدانان", 21 },
                    { 113, "یاسوج", 22 },
                    { 114, "گچساران", 22 },
                    { 115, "دهدشت", 22 },
                    { 116, "شهرکرد", 23 },
                    { 117, "فارسان", 23 },
                    { 118, "بروجن", 23 },
                    { 119, "لردگان", 23 },
                    { 120, "خرم‌آباد", 24 },
                    { 121, "بروجرد", 24 },
                    { 122, "دورود", 24 },
                    { 123, "الیگودرز", 24 },
                    { 124, "اراک", 25 },
                    { 125, "ساوه", 25 },
                    { 126, "خمین", 25 },
                    { 127, "محلات", 25 },
                    { 128, "بوشهر", 26 },
                    { 129, "برازجان", 26 },
                    { 130, "جم", 26 },
                    { 131, "دیر", 26 },
                    { 132, "کنگان", 26 },
                    { 133, "زاهدان", 27 },
                    { 134, "چابهار", 27 },
                    { 135, "ایرانشهر", 27 },
                    { 136, "خاش", 27 },
                    { 137, "گرگان", 28 },
                    { 138, "گنبد", 28 },
                    { 139, "علی‌آباد", 28 },
                    { 140, "آق‌قلا", 28 },
                    { 141, "کلاله", 28 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId",
                table: "Cities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRole_RolesId",
                table: "GroupRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DepartmentID",
                table: "Groups",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_People_CityId",
                table: "People",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_GroupID",
                table: "People",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupRole");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
