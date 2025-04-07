using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SourceBaseBE.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlarmTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlarmTypeName = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AreaCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AreaCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AreaName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListAuthGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListAuthPermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RtspLink = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    CameraName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CameraLocation = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    FrameRate = table.Column<int>(type: "integer", nullable: true),
                    OutPutWidth = table.Column<int>(type: "integer", nullable: true),
                    OutPutHeight = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataTypeEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTypeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntryRequestTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryRequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntryTransactionTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryTransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FCM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    UploadedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SerialCode = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "character varying(511)", maxLength: 511, nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    SendDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShiftTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestCRUD2s",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCRUD2s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthGroupEntityAuthPermissionEntity",
                columns: table => new
                {
                    AuthGroupsId = table.Column<long>(type: "bigint", nullable: false),
                    AuthPermissionsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthGroupEntityAuthPermissionEntity", x => new { x.AuthGroupsId, x.AuthPermissionsId });
                    table.ForeignKey(
                        name: "FK_AuthGroupEntityAuthPermissionEntity_AuthGroups_AuthGroupsId",
                        column: x => x.AuthGroupsId,
                        principalTable: "ListAuthGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthGroupEntityAuthPermissionEntity_AuthPermissions_AuthPer~",
                        column: x => x.AuthPermissionsId,
                        principalTable: "ListAuthPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CameraSettings",
                columns: table => new
                {
                    CameraId = table.Column<long>(type: "bigint", nullable: false),
                    AreaCodeId = table.Column<long>(type: "bigint", nullable: false),
                    FilePathId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraSettings", x => new { x.AreaCodeId, x.CameraId });
                    table.ForeignKey(
                        name: "FK_CameraSettings_AreaCodes_AreaCodeId",
                        column: x => x.AreaCodeId,
                        principalTable: "AreaCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CameraSettings_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CameraSettings_Files_FilePathId",
                        column: x => x.FilePathId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Category = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    ProductSKU = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    ImageFileId = table.Column<long>(type: "bigint", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodTypes_Files_ImageFileId",
                        column: x => x.ImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IdentityCards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CiNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FrontImageFileId = table.Column<long>(type: "bigint", nullable: false),
                    BackImageFileId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityCards_Files_BackImageFileId",
                        column: x => x.BackImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityCards_Files_FrontImageFileId",
                        column: x => x.FrontImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    ProductTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ShiftTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_ShiftTypes_ShiftTypeId",
                        column: x => x.ShiftTypeId,
                        principalTable: "ShiftTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: true),
                    FactoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Departments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FactoryId = table.Column<long>(type: "bigint", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workshops_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockKeepingUnits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManufactoryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockKeepingUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockKeepingUnits_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    WorkshopId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lines_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    MachineOperatingInstructionFileId = table.Column<long>(type: "bigint", nullable: true),
                    FileId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Machines_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Supplier = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    Model = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    MaxOperationTime = table.Column<long>(type: "bigint", nullable: true),
                    RunTime = table.Column<long>(type: "bigint", nullable: true),
                    OperationStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Manufacturer = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Group = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    Category = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    SerialCode = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    MachineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipments_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Supplier = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    Model = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    MaxOperationTime = table.Column<long>(type: "bigint", nullable: true),
                    OperationStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OpenCloseLimit = table.Column<long>(type: "bigint", nullable: true),
                    OpenCloseCount = table.Column<long>(type: "bigint", nullable: true),
                    Manufacturer = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Group = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    Category = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    MachineId = table.Column<long>(type: "bigint", nullable: true),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Devices_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Devices_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    EnviromentVarName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    LastUpdatedValue = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    StandardValue = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    DefaultValue = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    UnitOfMeasurement = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    TableSerial = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ReadWrite = table.Column<int>(type: "INT", nullable: true),
                    Publisher = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    Tags = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    MinTimeSavingDataIntervals = table.Column<int>(type: "integer", nullable: true),
                    DataTypeId = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    MachineId = table.Column<long>(type: "bigint", nullable: true),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: true),
                    DeviceId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameters_DataTypeEntity_DataTypeId",
                        column: x => x.DataTypeId,
                        principalTable: "DataTypeEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parameters_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parameters_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parameters_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parameters_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Limitations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UpperLimit = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    TargetValue = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    LowerLimit = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ParameterId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Limitations_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Alarms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlarmTypeId = table.Column<long>(type: "bigint", nullable: false),
                    EntryRequestId = table.Column<long>(type: "bigint", nullable: false),
                    AlarmAreaId = table.Column<long>(type: "bigint", nullable: false),
                    AlarmName = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    AlarmTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FilePath = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alarms_AlarmTypes_AlarmTypeId",
                        column: x => x.AlarmTypeId,
                        principalTable: "AlarmTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alarms_AreaCodes_AlarmAreaId",
                        column: x => x.AlarmAreaId,
                        principalTable: "AreaCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaCodeEntityEmployeeEntity",
                columns: table => new
                {
                    AreaCodesId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaCodeEntityEmployeeEntity", x => new { x.AreaCodesId, x.EmployeesId });
                    table.ForeignKey(
                        name: "FK_AreaCodeEntityEmployeeEntity_AreaCodes_AreaCodesId",
                        column: x => x.AreaCodesId,
                        principalTable: "AreaCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaCodeEntityEntryRequestEntity",
                columns: table => new
                {
                    AreaCodesId = table.Column<long>(type: "bigint", nullable: false),
                    EntryRequestsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaCodeEntityEntryRequestEntity", x => new { x.AreaCodesId, x.EntryRequestsId });
                    table.ForeignKey(
                        name: "FK_AreaCodeEntityEntryRequestEntity_AreaCodes_AreaCodesId",
                        column: x => x.AreaCodesId,
                        principalTable: "AreaCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthGroupEntityUserEntity",
                columns: table => new
                {
                    AuthGroupsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthGroupEntityUserEntity", x => new { x.AuthGroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AuthGroupEntityUserEntity_AuthGroups_AuthGroupsId",
                        column: x => x.AuthGroupsId,
                        principalTable: "ListAuthGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthPermissionEntityUserEntity",
                columns: table => new
                {
                    AuthPermissionsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthPermissionEntityUserEntity", x => new { x.AuthPermissionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AuthPermissionEntityUserEntity_AuthPermissions_AuthPerm~",
                        column: x => x.AuthPermissionsId,
                        principalTable: "ListAuthPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastLogin = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    AuthAccountTypeId = table.Column<long>(type: "bigint", nullable: true),
                    AuthTokenId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CompanyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: true),
                    Avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    License = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AccountTypes_AuthAccountTypeId",
                        column: x => x.AuthAccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_AuthTokens_AuthTokenId",
                        column: x => x.AuthTokenId,
                        principalTable: "AuthTokens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "CHAR(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "VARCHAR(4000)", maxLength: 4000, nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    SendDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    MessageType = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    AuthGroupId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AuthGroups_AuthGroupId",
                        column: x => x.AuthGroupId,
                        principalTable: "ListAuthGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "ListUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestCRUDs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTimeValue = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StringValue = table.Column<string>(type: "text", nullable: false),
                    LongValue = table.Column<long>(type: "bigint", nullable: false),
                    IntValue = table.Column<int>(type: "integer", nullable: false),
                    ShortValue = table.Column<short>(type: "smallint", nullable: false),
                    DoubleValue = table.Column<double>(type: "double precision", nullable: false),
                    BoolValue = table.Column<bool>(type: "boolean", nullable: false),
                    TimeIntervalInSeconds = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeValue2 = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StringValue2 = table.Column<string>(type: "text", nullable: true),
                    LongValue2 = table.Column<long>(type: "bigint", nullable: true),
                    IntValue2 = table.Column<int>(type: "integer", nullable: true),
                    ShortValue2 = table.Column<short>(type: "smallint", nullable: true),
                    DoubleValue2 = table.Column<double>(type: "double precision", nullable: true),
                    BoolValue2 = table.Column<bool>(type: "boolean", nullable: true),
                    TimeIntervalInSeconds2 = table.Column<long>(type: "bigint", nullable: true),
                    ReviewerId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCRUDs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCRUDs_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "ListUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "refTestCRUDTestCRUD2",
                columns: table => new
                {
                    TestCRUD2sId = table.Column<long>(type: "bigint", nullable: false),
                    TestCRUDsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refTestCRUDTestCRUD2", x => new { x.TestCRUD2sId, x.TestCRUDsId });
                    table.ForeignKey(
                        name: "FK_refTestCRUDTestCRUD2_TestCRUD2s_TestCRUD2sId",
                        column: x => x.TestCRUD2sId,
                        principalTable: "TestCRUD2s",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_refTestCRUDTestCRUD2_TestCRUDs_TestCRUDsId",
                        column: x => x.TestCRUDsId,
                        principalTable: "TestCRUDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certificatess",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CertificateNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FrontImageFileId = table.Column<long>(type: "bigint", nullable: false),
                    BackImageFileId = table.Column<long>(type: "bigint", nullable: false),
                    TypeCertificate = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificatess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificatess_Files_BackImageFileId",
                        column: x => x.BackImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificatess_Files_FrontImageFileId",
                        column: x => x.FrontImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverManageByOwner",
                columns: table => new
                {
                    ManageById = table.Column<long>(type: "bigint", nullable: false),
                    ManagerDriversId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverManageByOwner", x => new { x.ManageById, x.ManagerDriversId });
                });

            migrationBuilder.CreateTable(
                name: "DriverRegistrations",
                columns: table => new
                {
                    EntryRequestId = table.Column<long>(type: "bigint", nullable: false),
                    DriverId = table.Column<long>(type: "bigint", nullable: false),
                    IsMain = table.Column<bool>(type: "Bool", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverRegistrations", x => new { x.DriverId, x.EntryRequestId });
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "CHAR(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "CHAR(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "CHAR(100)", maxLength: 100, nullable: true),
                    JobTitle = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    ImagePathId = table.Column<long>(type: "bigint", nullable: true),
                    DateJoined = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    ShiftId = table.Column<long>(type: "bigint", nullable: true),
                    IdentityCardId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SerialCode = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    EntryRequestCount = table.Column<int>(type: "integer", nullable: false),
                    LastEntryRequestId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Files_ImagePathId",
                        column: x => x.ImagePathId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_IdentityCards_IdentityCardId",
                        column: x => x.IdentityCardId,
                        principalTable: "IdentityCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntryCaptures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryRequestId = table.Column<long>(type: "bigint", nullable: false),
                    CapturedBy = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    MeasuredWeight = table.Column<float>(type: "REAL", nullable: false),
                    LicensePlateScanned = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    InOutType = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    ImageFileId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryCaptures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryCaptures_Employees_CapturedBy",
                        column: x => x.CapturedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryCaptures_Files_ImageFileId",
                        column: x => x.ImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryReportLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryReportId = table.Column<long>(type: "bigint", nullable: false),
                    ChangedBy = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryReportLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryReportLogs_Employees_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryRequestId = table.Column<long>(type: "bigint", nullable: false),
                    ReportBy = table.Column<long>(type: "bigint", nullable: false),
                    ReportStatus = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    MeasuredWeight = table.Column<decimal>(type: "numeric", nullable: false),
                    LicensePlateScanned = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    InOutType = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    ImageFileId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryReports_Employees_ReportBy",
                        column: x => x.ReportBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryReports_Files_ImageFileId",
                        column: x => x.ImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryRequestLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryRequestId = table.Column<long>(type: "bigint", nullable: false),
                    ChangedBy = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryRequestLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryRequestLogs_Employees_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QrCode = table.Column<string>(type: "CHAR(6)", nullable: true),
                    OwnerDriverId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TimeOut = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: true),
                    TimeApproved = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RegistrationStatus = table.Column<string>(type: "text", nullable: false),
                    EntryRequestTypeId = table.Column<long>(type: "bigint", nullable: false),
                    EntryTransactionTypeId = table.Column<long>(type: "bigint", nullable: false),
                    IsGuest = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryRequests_Employees_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntryRequests_Employees_OwnerDriverId",
                        column: x => x.OwnerDriverId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryRequests_EntryRequestTypes_EntryRequestTypeId",
                        column: x => x.EntryRequestTypeId,
                        principalTable: "EntryRequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryRequests_EntryTransactionTypes_EntryTransactionTypeId",
                        column: x => x.EntryTransactionTypeId,
                        principalTable: "EntryTransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodRegistrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GoodsTypeId = table.Column<long>(type: "bigint", nullable: false),
                    EntryRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodRegistrations_EntryRequests_EntryRequestId",
                        column: x => x.EntryRequestId,
                        principalTable: "EntryRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodRegistrations_GoodTypes_GoodsTypeId",
                        column: x => x.GoodsTypeId,
                        principalTable: "GoodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryRequestId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    CiNumber = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DrivingLicense = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "DATE", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guests_EntryRequests_EntryRequestId",
                        column: x => x.EntryRequestId,
                        principalTable: "EntryRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleTypeId = table.Column<long>(type: "bigint", nullable: false),
                    OwnerDriverId = table.Column<long>(type: "bigint", nullable: false),
                    ImageFileId = table.Column<long>(type: "bigint", nullable: true),
                    TankNumber = table.Column<int>(type: "integer", nullable: false),
                    SealNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LicensePlateImage = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    LastEntryRequestId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Employees_OwnerDriverId",
                        column: x => x.OwnerDriverId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_EntryRequests_LastEntryRequestId",
                        column: x => x.LastEntryRequestId,
                        principalTable: "EntryRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_Files_ImageFileId",
                        column: x => x.ImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_AlarmAreaId",
                table: "Alarms",
                column: "AlarmAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_AlarmTypeId",
                table: "Alarms",
                column: "AlarmTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_EntryRequestId",
                table: "Alarms",
                column: "EntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaCodeEntityEmployeeEntity_EmployeesId",
                table: "AreaCodeEntityEmployeeEntity",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaCodeEntityEntryRequestEntity_EntryRequestsId",
                table: "AreaCodeEntityEntryRequestEntity",
                column: "EntryRequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaCodes_AreaCode",
                table: "AreaCodes",
                column: "AreaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthGroupEntityAuthPermissionEntity_AuthPermissionsId",
                table: "AuthGroupEntityAuthPermissionEntity",
                column: "AuthPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthGroupEntityUserEntity_UsersId",
                table: "AuthGroupEntityUserEntity",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthPermissionEntityUserEntity_UsersId",
                table: "AuthPermissionEntityUserEntity",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthAccountTypeId",
                table: "ListUser",
                column: "AuthAccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthTokenId",
                table: "ListUser",
                column: "AuthTokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "ListUser",
                column: "EditerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CameraSettings_CameraId",
                table: "CameraSettings",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraSettings_FilePathId",
                table: "CameraSettings",
                column: "FilePathId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificatess_BackImageFileId",
                table: "Certificatess",
                column: "BackImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificatess_EmployeeId",
                table: "Certificatess",
                column: "EditerId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificatess_FrontImageFileId",
                table: "Certificatess",
                column: "FrontImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FactoryId",
                table: "Departments",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_EquipmentId",
                table: "Devices",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_LineId",
                table: "Devices",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_MachineId",
                table: "Devices",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverManageByOwner_ManagerDriversId",
                table: "DriverManageByOwner",
                column: "ManagerDriversId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverRegistrations_EntryRequestId",
                table: "DriverRegistrations",
                column: "EntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IdentityCardId",
                table: "Employees",
                column: "IdentityCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ImagePathId",
                table: "Employees",
                column: "ImagePathId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LastEntryRequestId",
                table: "Employees",
                column: "LastEntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ShiftId",
                table: "Employees",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryCaptures_CapturedBy",
                table: "EntryCaptures",
                column: "CapturedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntryCaptures_EntryRequestId_InOutType",
                table: "EntryCaptures",
                columns: new[] { "EntryRequestId", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntryCaptures_ImageFileId",
                table: "EntryCaptures",
                column: "ImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReportLogs_ChangedBy",
                table: "EntryReportLogs",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReportLogs_EntryReportId",
                table: "EntryReportLogs",
                column: "EntryReportId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_EntryRequestId",
                table: "EntryReports",
                column: "EntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_ImageFileId",
                table: "EntryReports",
                column: "ImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryReports_ReportBy",
                table: "EntryReports",
                column: "ReportBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequestLogs_ChangedBy",
                table: "EntryRequestLogs",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequestLogs_EntryRequestId",
                table: "EntryRequestLogs",
                column: "EntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequests_ApprovedBy",
                table: "EntryRequests",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequests_EntryRequestTypeId",
                table: "EntryRequests",
                column: "EntryRequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequests_EntryTransactionTypeId",
                table: "EntryRequests",
                column: "EntryTransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequests_OwnerDriverId",
                table: "EntryRequests",
                column: "OwnerDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequests_QrCode",
                table: "EntryRequests",
                column: "QrCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntryRequests_VehicleId",
                table: "EntryRequests",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_LineId",
                table: "Equipments",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_MachineId",
                table: "Equipments",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Factories_OrganizationId",
                table: "Factories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodRegistrations_EntryRequestId",
                table: "GoodRegistrations",
                column: "EntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodRegistrations_GoodsTypeId",
                table: "GoodRegistrations",
                column: "GoodsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodTypes_ImageFileId",
                table: "GoodTypes",
                column: "ImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_EntryRequestId",
                table: "Guests",
                column: "EntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityCards_BackImageFileId",
                table: "IdentityCards",
                column: "BackImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityCards_FrontImageFileId",
                table: "IdentityCards",
                column: "FrontImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Limitations_ParameterId",
                table: "Limitations",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_WorkshopId",
                table: "Lines",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_FileId",
                table: "Machines",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_LineId",
                table: "Machines",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthGroupId",
                table: "Messages",
                column: "AuthGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "EditerId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_DataTypeId",
                table: "Parameters",
                column: "DataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_DeviceId",
                table: "Parameters",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_EquipmentId",
                table: "Parameters",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_LineId",
                table: "Parameters",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_MachineId",
                table: "Parameters",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_refTestCRUDTestCRUD2_TestCRUDsId",
                table: "refTestCRUDTestCRUD2",
                column: "TestCRUDsId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ShiftTypeId",
                table: "Shifts",
                column: "ShiftTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockKeepingUnits_ProductId",
                table: "StockKeepingUnits",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCRUDs_ReviewerId",
                table: "TestCRUDs",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ImageFileId",
                table: "Vehicles",
                column: "ImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LastEntryRequestId",
                table: "Vehicles",
                column: "LastEntryRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerDriverId",
                table: "Vehicles",
                column: "OwnerDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_FactoryId",
                table: "Workshops",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alarms_EntryRequests_EntryRequestId",
                table: "Alarms",
                column: "EntryRequestId",
                principalTable: "EntryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AreaCodeEntityEmployeeEntity_Employees_EmployeesId",
                table: "AreaCodeEntityEmployeeEntity",
                column: "EmployeesId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AreaCodeEntityEntryRequestEntity_EntryRequests_EntryRequest~",
                table: "AreaCodeEntityEntryRequestEntity",
                column: "EntryRequestsId",
                principalTable: "EntryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthGroupEntityUserEntity_Users_UsersId",
                table: "AuthGroupEntityUserEntity",
                column: "UsersId",
                principalTable: "ListUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthPermissionEntityUserEntity_Users_UsersId",
                table: "AuthPermissionEntityUserEntity",
                column: "UsersId",
                principalTable: "ListUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "ListUser",
                column: "EditerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificatess_Employees_EmployeeId",
                table: "Certificatess",
                column: "EditerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverManageByOwner_Employees_ManageById",
                table: "DriverManageByOwner",
                column: "ManageById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverManageByOwner_Employees_ManagerDriversId",
                table: "DriverManageByOwner",
                column: "ManagerDriversId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverRegistrations_Employees_DriverId",
                table: "DriverRegistrations",
                column: "DriverId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverRegistrations_EntryRequests_EntryRequestId",
                table: "DriverRegistrations",
                column: "EntryRequestId",
                principalTable: "EntryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EntryRequests_LastEntryRequestId",
                table: "Employees",
                column: "LastEntryRequestId",
                principalTable: "EntryRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryCaptures_EntryRequests_EntryRequestId",
                table: "EntryCaptures",
                column: "EntryRequestId",
                principalTable: "EntryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryReportLogs_EntryReports_EntryReportId",
                table: "EntryReportLogs",
                column: "EntryReportId",
                principalTable: "EntryReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryReports_EntryRequests_EntryRequestId",
                table: "EntryReports",
                column: "EntryRequestId",
                principalTable: "EntryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryRequestLogs_EntryRequests_EntryRequestId",
                table: "EntryRequestLogs",
                column: "EntryRequestId",
                principalTable: "EntryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryRequests_Vehicles_VehicleId",
                table: "EntryRequests",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EntryRequests_LastEntryRequestId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_EntryRequests_LastEntryRequestId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.DropTable(
                name: "AreaCodeEntityEmployeeEntity");

            migrationBuilder.DropTable(
                name: "AreaCodeEntityEntryRequestEntity");

            migrationBuilder.DropTable(
                name: "AuthGroupEntityAuthPermissionEntity");

            migrationBuilder.DropTable(
                name: "AuthGroupEntityUserEntity");

            migrationBuilder.DropTable(
                name: "AuthPermissionEntityUserEntity");

            migrationBuilder.DropTable(
                name: "CameraSettings");

            migrationBuilder.DropTable(
                name: "Certificatess");

            migrationBuilder.DropTable(
                name: "DriverManageByOwner");

            migrationBuilder.DropTable(
                name: "DriverRegistrations");

            migrationBuilder.DropTable(
                name: "EntryCaptures");

            migrationBuilder.DropTable(
                name: "EntryReportLogs");

            migrationBuilder.DropTable(
                name: "EntryRequestLogs");

            migrationBuilder.DropTable(
                name: "FCM");

            migrationBuilder.DropTable(
                name: "GoodRegistrations");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Limitations");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "refTestCRUDTestCRUD2");

            migrationBuilder.DropTable(
                name: "StockKeepingUnits");

            migrationBuilder.DropTable(
                name: "AlarmTypes");

            migrationBuilder.DropTable(
                name: "ListAuthPermission");

            migrationBuilder.DropTable(
                name: "AreaCodes");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "EntryReports");

            migrationBuilder.DropTable(
                name: "GoodTypes");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "ListAuthGroup");

            migrationBuilder.DropTable(
                name: "TestCRUD2s");

            migrationBuilder.DropTable(
                name: "TestCRUDs");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "DataTypeEntity");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "ListUser");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "AuthTokens");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropTable(
                name: "EntryRequests");

            migrationBuilder.DropTable(
                name: "EntryRequestTypes");

            migrationBuilder.DropTable(
                name: "EntryTransactionTypes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "IdentityCards");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Factories");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "ShiftTypes");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
