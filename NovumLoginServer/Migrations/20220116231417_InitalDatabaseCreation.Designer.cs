﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NovumLoginServer.DBModels;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NovumLoginServer.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20220116231417_InitalDatabaseCreation")]
    partial class InitalDatabaseCreation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Accounts")
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NovumLoginServer.DBModels.Session", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(56)
                        .HasColumnType("character varying(56)");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.ToTable("Sessions", "Accounts");
                });

            modelBuilder.Entity("NovumLoginServer.DBModels.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Passhash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Users", "Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
