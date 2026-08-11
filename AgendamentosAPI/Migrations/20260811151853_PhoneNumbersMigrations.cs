using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendamentosAPI.Migrations
{
    /// <inheritdoc />
    public partial class PhoneNumbersMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "PhoneNumbers",
                table: "Customers",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumbers",
                table: "Customers");
        }
    }
}
