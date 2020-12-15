﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v214 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    Version = table.Column<long>(nullable: true),
                    GroupId = table.Column<long>(nullable: true),
                    AccessLevel = table.Column<int>(nullable: false),
                    PreprocessEntryPoint = table.Column<string>(nullable: true),
                    PreprocessRepositoryGitId = table.Column<long>(nullable: true),
                    PreprocessRepositoryName = table.Column<string>(nullable: true),
                    PreprocessRepositoryOwner = table.Column<string>(nullable: true),
                    PreprocessRepositoryBranch = table.Column<string>(nullable: true),
                    PreprocessRepositoryCommitId = table.Column<string>(nullable: true),
                    PreprocessContainerRegistryId = table.Column<long>(nullable: true),
                    PreprocessContainerImage = table.Column<string>(nullable: true),
                    PreprocessContainerTag = table.Column<string>(nullable: true),
                    PreprocessCpu = table.Column<int>(nullable: false),
                    PreprocessMemory = table.Column<int>(nullable: false),
                    PreprocessGpu = table.Column<int>(nullable: false),
                    TrainingEntryPoint = table.Column<string>(nullable: true),
                    TrainingRepositoryGitId = table.Column<long>(nullable: true),
                    TrainingRepositoryName = table.Column<string>(nullable: true),
                    TrainingRepositoryOwner = table.Column<string>(nullable: true),
                    TrainingRepositoryBranch = table.Column<string>(nullable: true),
                    TrainingRepositoryCommitId = table.Column<string>(nullable: true),
                    TrainingContainerRegistryId = table.Column<long>(nullable: true),
                    TrainingContainerImage = table.Column<string>(nullable: true),
                    TrainingContainerTag = table.Column<string>(nullable: true),
                    TrainingCpu = table.Column<int>(nullable: false),
                    TrainingMemory = table.Column<int>(nullable: false),
                    TrainingGpu = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Registries_PreprocessContainerRegistryId",
                        column: x => x.PreprocessContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Templates_Registries_TrainingContainerRegistryId",
                        column: x => x.TrainingContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateTenantMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TemplateId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateTenantMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateTenantMaps_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateTenantMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_PreprocessContainerRegistryId",
                table: "Templates",
                column: "PreprocessContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TrainingContainerRegistryId",
                table: "Templates",
                column: "TrainingContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateTenantMaps_TenantId",
                table: "TemplateTenantMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateTenantMaps_TemplateId_TenantId",
                table: "TemplateTenantMaps",
                columns: new[] { "TemplateId", "TenantId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateTenantMaps");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
