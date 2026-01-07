using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiKnowledgeAssistant.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialJobExecutions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_executions",
                columns: table => new
                {
                    job_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    workflow_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    environment = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    executed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_executions", x => x.job_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_executions_workflow_id_environment_executed_at",
                table: "job_executions",
                columns: new[] { "workflow_id", "environment", "executed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_job_executions_workflow_id_environment_status_executed_at",
                table: "job_executions",
                columns: new[] { "workflow_id", "environment", "status", "executed_at" });

            SeedInitialData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_executions");
        }

        private static void SeedInitialData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO job_executions (job_id, workflow_id, environment, status, executed_at)
                SELECT 'job-1', 'wf-1', 'prod', 0, '2025-01-01T10:00:00Z'
                WHERE NOT EXISTS (SELECT 1 FROM job_executions WHERE job_id = 'job-1');

                INSERT INTO job_executions (job_id, workflow_id, environment, status, executed_at)
                SELECT 'job-2', 'wf-1', 'prod', 1, '2025-01-01T10:05:00Z'
                WHERE NOT EXISTS (SELECT 1 FROM job_executions WHERE job_id = 'job-2');

                INSERT INTO job_executions (job_id, workflow_id, environment, status, executed_at)
                SELECT 'job-3', 'wf-1', 'prod', 1, '2025-01-01T10:10:00Z'
                WHERE NOT EXISTS (SELECT 1 FROM job_executions WHERE job_id = 'job-3');

                INSERT INTO job_executions (job_id, workflow_id, environment, status, executed_at)
                SELECT 'job-4', 'wf-1', 'prod', 0, '2025-01-01T10:20:00Z'
                WHERE NOT EXISTS (SELECT 1 FROM job_executions WHERE job_id = 'job-4');

                INSERT INTO job_executions (job_id, workflow_id, environment, status, executed_at)
                SELECT 'job-5', 'wf-1', 'prod', 1, '2025-01-01T10:30:00Z'
                WHERE NOT EXISTS (SELECT 1 FROM job_executions WHERE job_id = 'job-5');
            ");
        }

    }
}
