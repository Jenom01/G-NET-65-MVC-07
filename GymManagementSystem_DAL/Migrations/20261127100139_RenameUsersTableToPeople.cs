using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem_DAL.Migrations;

/// <inheritdoc />
public partial class _20261127100139_RenameUsersTableToPeople : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Bookings_GymUsers_MemberId",
            table: "Bookings");

        migrationBuilder.DropForeignKey(
            name: "FK_HealthRecords_GymUsers_MemberId",
            table: "HealthRecords");

        migrationBuilder.DropForeignKey(
            name: "FK_Memberships_GymUsers_MemberId",
            table: "Memberships");

        migrationBuilder.DropForeignKey(
            name: "FK_Sessions_GymUsers_TrainerId",
            table: "Sessions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GymUsers",
            table: "GymUsers");

        migrationBuilder.DropCheckConstraint(
            name: "CK_Users_Email",
            table: "GymUsers");

        migrationBuilder.DropCheckConstraint(
            name: "CK_Users_Phone",
            table: "GymUsers");

        migrationBuilder.DropCheckConstraint(
            name: "CK_Users_Trainer_Speciality",
            table: "GymUsers");

        migrationBuilder.RenameTable(
            name: "GymUsers",
            newName: "People");

        migrationBuilder.RenameColumn(
            name: "UserType",
            table: "People",
            newName: "PersonType");

        migrationBuilder.RenameIndex(
            name: "IX_GymUsers_Phone",
            table: "People",
            newName: "IX_People_Phone");

        migrationBuilder.RenameIndex(
            name: "IX_GymUsers_Email",
            table: "People",
            newName: "IX_People_Email");

        migrationBuilder.AddPrimaryKey(
            name: "PK_People",
            table: "People",
            column: "Id");

        migrationBuilder.AddCheckConstraint(
            name: "CK_People_Email",
            table: "People",
            sql: "[Email] LIKE '_%@_%._%'");

        migrationBuilder.AddCheckConstraint(
            name: "CK_People_Phone",
            table: "People",
            sql: "LEN([Phone]) = 11 AND [Phone] LIKE '01[0125]%' AND [Phone] NOT LIKE '%[^0-9]%'");

        migrationBuilder.AddCheckConstraint(
            name: "CK_People_Trainer_Speciality",
            table: "People",
            sql: "[PersonType] <> 'Trainer' OR [Speciality] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Bookings_People_MemberId",
            table: "Bookings",
            column: "MemberId",
            principalTable: "People",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_HealthRecords_People_MemberId",
            table: "HealthRecords",
            column: "MemberId",
            principalTable: "People",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Memberships_People_MemberId",
            table: "Memberships",
            column: "MemberId",
            principalTable: "People",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Sessions_People_TrainerId",
            table: "Sessions",
            column: "TrainerId",
            principalTable: "People",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Bookings_People_MemberId",
            table: "Bookings");

        migrationBuilder.DropForeignKey(
            name: "FK_HealthRecords_People_MemberId",
            table: "HealthRecords");

        migrationBuilder.DropForeignKey(
            name: "FK_Memberships_People_MemberId",
            table: "Memberships");

        migrationBuilder.DropForeignKey(
            name: "FK_Sessions_People_TrainerId",
            table: "Sessions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_People",
            table: "People");

        migrationBuilder.DropCheckConstraint(
            name: "CK_People_Email",
            table: "People");

        migrationBuilder.DropCheckConstraint(
            name: "CK_People_Phone",
            table: "People");

        migrationBuilder.DropCheckConstraint(
            name: "CK_People_Trainer_Speciality",
            table: "People");

        migrationBuilder.RenameTable(
            name: "People",
            newName: "GymUsers");

        migrationBuilder.RenameColumn(
            name: "PersonType",
            table: "GymUsers",
            newName: "UserType");

        migrationBuilder.RenameIndex(
            name: "IX_People_Phone",
            table: "GymUsers",
            newName: "IX_GymUsers_Phone");

        migrationBuilder.RenameIndex(
            name: "IX_People_Email",
            table: "GymUsers",
            newName: "IX_GymUsers_Email");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GymUsers",
            table: "GymUsers",
            column: "Id");

        migrationBuilder.AddCheckConstraint(
            name: "CK_Users_Email",
            table: "GymUsers",
            sql: "[Email] LIKE '_%@_%._%'");

        migrationBuilder.AddCheckConstraint(
            name: "CK_Users_Phone",
            table: "GymUsers",
            sql: "LEN([Phone]) = 11 AND [Phone] LIKE '01[0125]%' AND [Phone] NOT LIKE '%[^0-9]%'");

        migrationBuilder.AddCheckConstraint(
            name: "CK_Users_Trainer_Speciality",
            table: "GymUsers",
            sql: "[UserType] <> 'Trainer' OR [Speciality] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Bookings_GymUsers_MemberId",
            table: "Bookings",
            column: "MemberId",
            principalTable: "GymUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_HealthRecords_GymUsers_MemberId",
            table: "HealthRecords",
            column: "MemberId",
            principalTable: "GymUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Memberships_GymUsers_MemberId",
            table: "Memberships",
            column: "MemberId",
            principalTable: "GymUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Sessions_GymUsers_TrainerId",
            table: "Sessions",
            column: "TrainerId",
            principalTable: "GymUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
