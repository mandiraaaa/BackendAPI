using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class twelthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customerId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "paidAmount",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "tax",
                table: "Billings",
                newName: "Amount");

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhoneNumber",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "installmentName",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerPhoneNumber",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "installmentName",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Billings",
                newName: "tax");

            migrationBuilder.AddColumn<Guid>(
                name: "customerId",
                table: "Billings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "paidAmount",
                table: "Billings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "Billings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
