using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementAppRepository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quantity_measurements",
                columns: table => new
                {
                    operation_id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    operation_type = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    operand1_value = table.Column<double>(type: "float", nullable: false),
                    operand1_unit = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    operand1_measurement = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    operand2_value = table.Column<double>(type: "float", nullable: true),
                    operand2_unit = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    operand2_measurement = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    result_value = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    result_unit = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    result_measurement = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    is_error = table.Column<bool>(type: "bit", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quantity_measurements", x => x.operation_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quantity_measurements");
        }
    }
}
