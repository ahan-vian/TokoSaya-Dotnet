using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokoSaya.Migrations
{
    /// <inheritdoc />
    public partial class TambahCatatanPesanan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNote",
                table: "OrderHeaders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNote",
                table: "OrderHeaders");
        }
    }
}
