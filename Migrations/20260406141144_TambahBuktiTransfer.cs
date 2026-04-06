using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokoSaya.Migrations
{
    /// <inheritdoc />
    public partial class TambahBuktiTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentReceiptUrl",
                table: "OrderHeaders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentReceiptUrl",
                table: "OrderHeaders");
        }
    }
}
