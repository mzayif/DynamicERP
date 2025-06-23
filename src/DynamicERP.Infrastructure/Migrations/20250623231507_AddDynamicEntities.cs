using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntitySchemas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Entity tipi (Customer, Product, Order, etc.)"),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Entity'nin görünen adı (Müşteri, Ürün, Sipariş, etc.)"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Entity'nin açıklaması"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Entity'nin hangi tenant'a ait olduğu"),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 1, comment: "Şema versiyonu"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Entity'nin aktif olup olmadığı"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Oluşturulma tarihi"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Güncellenme tarihi"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntitySchemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntitySchemas_Tenants",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DynamicEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchemaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Bu verinin hangi entity tipine ait olduğu"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Bu verinin hangi tenant'a ait olduğu"),
                    Data = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false, comment: "Verinin JSON formatında saklanan içeriği"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active", comment: "Verinin durumu (Active, Inactive, Deleted, etc.)"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Bu veriyi oluşturan kullanıcı"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Bu veriyi son güncelleyen kullanıcı"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Oluşturulma tarihi"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Güncellenme tarihi"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicEntities_Creator_Users",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DynamicEntities_EntitySchemas",
                        column: x => x.SchemaId,
                        principalTable: "EntitySchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DynamicEntities_Tenants",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DynamicEntities_Updater_Users",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchemaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Bu alanın hangi entity'ye ait olduğu"),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Alan adı (Name, Email, Phone, etc.)"),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Alanın görünen adı (Müşteri Adı, E-posta, Telefon, etc.)"),
                    FieldType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Alan tipi (Text, Number, Date, Boolean, Dropdown, etc.)"),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Veri tipi (string, int, datetime, bool, etc.)"),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Alan zorunlu mu?"),
                    IsSearchable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Alan aranabilir mi?"),
                    IsSortable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Alan sıralanabilir mi?"),
                    DefaultValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Alanın varsayılan değeri"),
                    MaxLength = table.Column<int>(type: "int", nullable: true, comment: "Alanın maksimum uzunluğu"),
                    MinLength = table.Column<int>(type: "int", nullable: true, comment: "Alanın minimum uzunluğu"),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true, comment: "Alanın maksimum değeri"),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true, comment: "Alanın minimum değeri"),
                    Options = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Dropdown alanları için seçenekler (JSON formatında)"),
                    ValidationRules = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Validation kuralları (JSON formatında)"),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Alanın sıralama indeksi"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Oluşturulma tarihi"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Güncellenme tarihi"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldDefinitions_EntitySchemas",
                        column: x => x.SchemaId,
                        principalTable: "EntitySchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntities_CreatedBy",
                table: "DynamicEntities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntities_SchemaId_TenantId",
                table: "DynamicEntities",
                columns: new[] { "SchemaId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntities_Status",
                table: "DynamicEntities",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntities_TenantId",
                table: "DynamicEntities",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntities_UpdatedBy",
                table: "DynamicEntities",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntitySchemas_EntityType_TenantId",
                table: "EntitySchemas",
                columns: new[] { "EntityType", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntitySchemas_TenantId",
                table: "EntitySchemas",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldDefinitions_SchemaId_FieldName",
                table: "FieldDefinitions",
                columns: new[] { "SchemaId", "FieldName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicEntities");

            migrationBuilder.DropTable(
                name: "FieldDefinitions");

            migrationBuilder.DropTable(
                name: "EntitySchemas");
        }
    }
}
