using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstNameLastNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7f688a03-dc67-44dd-8014-e82d25f53aea");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e72e48e1-9e65-46b1-af2f-cc8e3aa7d3d4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "27470982-5fd7-4ce7-afcf-f372f887114a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "a7fe4bf8-3e97-4a6c-89eb-c7c50fdeaf42");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 38, 53, 426, DateTimeKind.Utc).AddTicks(7959));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 38, 53, 426, DateTimeKind.Utc).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 38, 53, 426, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 38, 53, 426, DateTimeKind.Utc).AddTicks(9571));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5be09ef6-6eae-4ec8-9ee8-b2ace72a3699");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "7d5c68aa-1255-416e-962b-4f30304338a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "8f797b9c-d42e-458c-bb34-89b340fec0a6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "98f10fb3-009e-4567-bc61-3bc64b975b42");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 27, 11, 170, DateTimeKind.Utc).AddTicks(4859));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 27, 11, 170, DateTimeKind.Utc).AddTicks(6762));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 27, 11, 170, DateTimeKind.Utc).AddTicks(6767));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2026, 5, 15, 16, 27, 11, 170, DateTimeKind.Utc).AddTicks(6769));
        }
    }
}
