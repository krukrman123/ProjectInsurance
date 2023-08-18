using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectInsurance.Migrations
{
    public partial class CreateWiewAllProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql
                (
                    @"CREATE PROCEDURE ViewUserInsuredInsuranceEvent
                    AS
                    SELECT
	                  AspNetUsers.Id as UserId,
	                  Insured.Id as InsuredId,
	                  Insurance.Id as InsuranceId,
	                  Event.Id as EventId
	                FROM AspNetUsers
	                LEFT JOIN Insured
	                  ON AspNetUsers.Id = Insured.UserId
	                LEFT JOIN Insurance
	                  ON Insured.Id = Insurance.InsuranceHolderId
	                LEFT JOIN Event
	                  ON Insurance.Id = Event.InsuranceId"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
