using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication.EfModel;

namespace WebApplication.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20210902151739_Seed")]
    public class Seed: Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Persons(Name) VALUES('Wilhelms')");
        }
    }
}