using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MachineMonitoring.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MachineLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineId = table.Column<int>(type: "INTEGER", nullable: false),
                    OperatorId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProducedPerMinute = table.Column<int>(type: "INTEGER", nullable: false),
                    MachineStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LogTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineLogs_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineLogs_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "CNC - 01" },
                    { 2, "CNC - 02" },
                    { 3, "MILLING - 01" },
                    { 4, "MILLING - 02" },
                    { 5, "PRESS - 01" },
                    { 6, "PRESS - 02" },
                    { 7, "ASSEMBLY - 01" }
                });

            migrationBuilder.InsertData(
                table: "Operators",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Jhon Doe" },
                    { 2, "Jane Doe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineLogs_LogTime",
                table: "MachineLogs",
                column: "LogTime");

            migrationBuilder.CreateIndex(
                name: "IX_MachineLogs_MachineId",
                table: "MachineLogs",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineLogs_OperatorId",
                table: "MachineLogs",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Machine_Name_Unique",
                table: "Machines",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operator_Name_Unique",
                table: "Operators",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineLogs");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "Operators");
        }
    }
}
