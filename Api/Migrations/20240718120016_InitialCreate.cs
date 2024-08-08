using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                NIP = table.Column<long>(type: "bigint", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductCategories",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductCategories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CompanyAddresses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LocalNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PosteCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CompanyAddresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_CompanyAddresses_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Factures",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NumberFactures = table.Column<long>(type: "bigint", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Factures", x => x.Id);
                table.ForeignKey(
                    name: "FK_Factures_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Factures_Companies_UserCompanyId",
                    column: x => x.UserCompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.ForeignKey(
                    name: "FK_Products_ProductCategories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "ProductCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                PaymentDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                PaymantDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                FactureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Factures_FactureId",
                    column: x => x.FactureId,
                    principalTable: "Factures",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PdfFiles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FactureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PdfFiles", x => x.Id);
                table.ForeignKey(
                    name: "FK_PdfFiles_Factures_FactureId",
                    column: x => x.FactureId,
                    principalTable: "Factures",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FactureDetails",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UnitPriceNetto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                UnitPriceBrutto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                FactureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FactureDetails", x => x.Id);
                table.ForeignKey(
                    name: "FK_FactureDetails_Factures_FactureId",
                    column: x => x.FactureId,
                    principalTable: "Factures",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_FactureDetails_Products_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Products",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CompanyAddresses_CompanyId",
            table: "CompanyAddresses",
            column: "CompanyId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_FactureDetails_FactureId",
            table: "FactureDetails",
            column: "FactureId");

        migrationBuilder.CreateIndex(
            name: "IX_FactureDetails_ProductId",
            table: "FactureDetails",
            column: "ProductId");

        migrationBuilder.CreateIndex(
            name: "IX_Factures_CompanyId",
            table: "Factures",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_Factures_UserCompanyId",
            table: "Factures",
            column: "UserCompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_FactureId",
            table: "Payments",
            column: "FactureId");

        migrationBuilder.CreateIndex(
            name: "IX_PdfFiles_FactureId",
            table: "PdfFiles",
            column: "FactureId");

        migrationBuilder.CreateIndex(
            name: "IX_Products_CategoryId",
            table: "Products",
            column: "CategoryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CompanyAddresses");

        migrationBuilder.DropTable(
            name: "FactureDetails");

        migrationBuilder.DropTable(
            name: "Payments");

        migrationBuilder.DropTable(
            name: "PdfFiles");

        migrationBuilder.DropTable(
            name: "Products");

        migrationBuilder.DropTable(
            name: "Factures");

        migrationBuilder.DropTable(
            name: "ProductCategories");

        migrationBuilder.DropTable(
            name: "Companies");
    }
}
