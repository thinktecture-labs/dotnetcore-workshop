// <auto-generated />
using System;
using GrpcService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GrpcService.Migrations
{
    [DbContext(typeof(AkteDbContext))]
    [Migration("20210921135135_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("GrpcService.Models.Akte", b =>
                {
                    b.Property<Guid>("AktenNummer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("AktenNummer");

                    b.HasIndex("Name");

                    b.ToTable("Akten");
                });
#pragma warning restore 612, 618
        }
    }
}
