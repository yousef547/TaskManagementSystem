using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Projects");
        }
    }
}
