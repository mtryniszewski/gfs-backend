﻿// <auto-generated />
using System;
using GFS.Data.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GFS.Data.EFCore.Migrations
{
    [DbContext(typeof(GfsDbContext))]
    [Migration("20180819092321_testszafek")]
    partial class testszafek
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GFS.Data.Model.Entities.Fabric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsArchival");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ProducerCode")
                        .IsRequired();

                    b.Property<int>("ProducerId");

                    b.Property<float>("Thickness");

                    b.HasKey("Id");

                    b.HasIndex("ProducerId");

                    b.ToTable("Fabrics");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.Furniture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FurnitureType");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("OrderId");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Furnitures");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.GFS.Data.Model.Entities.TriangularFormatter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float?>("BottomBorderThickness");

                    b.Property<int>("Count");

                    b.Property<float?>("CutterDepth");

                    b.Property<float?>("CutterLength");

                    b.Property<float?>("CutterWidth");

                    b.Property<int>("FabricId");

                    b.Property<int>("FurnitureId");

                    b.Property<float?>("HypotenuseBorderThickness");

                    b.Property<float>("HypotenuseLength");

                    b.Property<bool>("IsBottomBorder");

                    b.Property<bool>("IsHypotenuseBorder");

                    b.Property<bool>("IsLeftBorder");

                    b.Property<bool>("IsMilling");

                    b.Property<float?>("LeftBorderThickness");

                    b.Property<float?>("LeftSpace");

                    b.Property<float>("Length");

                    b.Property<int?>("Milling");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<float>("Thickness");

                    b.Property<float?>("TopSpace");

                    b.Property<float>("Width");

                    b.HasKey("Id");

                    b.HasIndex("FabricId");

                    b.HasIndex("FurnitureId");

                    b.ToTable("TriangularFormatters");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.LFormatter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<float?>("CutterDepth");

                    b.Property<float?>("CutterLength");

                    b.Property<float?>("CutterWidth");

                    b.Property<float>("Depth1");

                    b.Property<float?>("Depth1BorderThickness");

                    b.Property<float>("Depth2");

                    b.Property<float?>("Depth2BorderThickness");

                    b.Property<int>("FabricId");

                    b.Property<int>("FurnitureId");

                    b.Property<float>("Indentation1");

                    b.Property<float?>("Indentation1BorderThickness");

                    b.Property<float>("Indentation2");

                    b.Property<float?>("Indentation2BorderThickness");

                    b.Property<bool>("IsDepth1Border");

                    b.Property<bool>("IsDepth2Border");

                    b.Property<bool>("IsIndentation1Border");

                    b.Property<bool>("IsIndentation2Border");

                    b.Property<bool>("IsMilling");

                    b.Property<bool>("IsWidth1Border");

                    b.Property<bool>("IsWidth2Border");

                    b.Property<float?>("LeftSpace");

                    b.Property<int?>("Milling");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<float>("Thickness");

                    b.Property<float?>("TopSpace");

                    b.Property<float>("Width1");

                    b.Property<float?>("Width1BorderThickness");

                    b.Property<float>("Width2");

                    b.Property<float?>("Width2BorderThickness");

                    b.HasKey("Id");

                    b.HasIndex("FabricId");

                    b.HasIndex("FurnitureId");

                    b.ToTable("LFormatters");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<bool>("IsArchival");

                    b.Property<bool>("IsConfirmed");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.PentagonFormatter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float?>("BottomBorderThickness");

                    b.Property<int>("Count");

                    b.Property<float?>("CutterDepth");

                    b.Property<float?>("CutterLength");

                    b.Property<float?>("CutterWidth");

                    b.Property<float>("Depth1");

                    b.Property<float>("Depth2");

                    b.Property<int>("FabricId");

                    b.Property<int>("FurnitureId");

                    b.Property<float?>("HypotenuseBorderThickness");

                    b.Property<bool>("IsBottomBorder");

                    b.Property<bool>("IsHypotenuseBorder");

                    b.Property<bool>("IsLeftBorder");

                    b.Property<bool>("IsMilling");

                    b.Property<bool>("IsRightBorder");

                    b.Property<bool>("IsTopBorder");

                    b.Property<float?>("LeftBorderThickness");

                    b.Property<float?>("LeftSpace");

                    b.Property<int?>("Milling");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<float?>("RightBorderThickness");

                    b.Property<float>("Thickness");

                    b.Property<float?>("TopBorderThickness");

                    b.Property<float?>("TopSpace");

                    b.Property<float>("Width1");

                    b.Property<float>("Width2");

                    b.HasKey("Id");

                    b.HasIndex("FabricId");

                    b.HasIndex("FurnitureId");

                    b.ToTable("PentagonFormatters");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.Producer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Email");

                    b.Property<bool>("IsArchival");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("Producers");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.RectangularFormatter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float?>("BottomBorderThickness");

                    b.Property<int>("Count");

                    b.Property<float?>("CutterDepth");

                    b.Property<float?>("CutterLength");

                    b.Property<float?>("CutterWidth");

                    b.Property<int>("FabricId");

                    b.Property<int>("FurnitureId");

                    b.Property<bool>("IsBottomBorder");

                    b.Property<bool>("IsLeftBorder");

                    b.Property<bool>("IsMilling");

                    b.Property<bool>("IsRightBorder");

                    b.Property<bool>("IsTopBorder");

                    b.Property<float?>("LeftBorderThickness");

                    b.Property<float?>("LeftSpace");

                    b.Property<float>("Length");

                    b.Property<int?>("Milling");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<float?>("RightBorderThickness");

                    b.Property<float>("Thickness");

                    b.Property<float?>("TopBorderThickness");

                    b.Property<float?>("TopSpace");

                    b.Property<float>("Width");

                    b.HasKey("Id");

                    b.HasIndex("FabricId");

                    b.HasIndex("FurnitureId");

                    b.ToTable("RectangularFormatters");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AccountActivationToken");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsArchival");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordResetToken");

                    b.Property<int>("Permissions");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.Fabric", b =>
                {
                    b.HasOne("GFS.Data.Model.Entities.Producer", "Producer")
                        .WithMany("Fabrics")
                        .HasForeignKey("ProducerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.Furniture", b =>
                {
                    b.HasOne("GFS.Data.Model.Entities.Order", "Order")
                        .WithMany("Furnitures")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.GFS.Data.Model.Entities.TriangularFormatter", b =>
                {
                    b.HasOne("GFS.Data.Model.Entities.Fabric", "Fabric")
                        .WithMany("TriangularFormatters")
                        .HasForeignKey("FabricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GFS.Data.Model.Entities.Furniture", "Furniture")
                        .WithMany("TriangularFormatters")
                        .HasForeignKey("FurnitureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.LFormatter", b =>
                {
                    b.HasOne("GFS.Data.Model.Entities.Fabric", "Fabric")
                        .WithMany("LFormatters")
                        .HasForeignKey("FabricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GFS.Data.Model.Entities.Furniture", "Furniture")
                        .WithMany("LFormatters")
                        .HasForeignKey("FurnitureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.Order", b =>
                {
                    b.HasOne("GFS.Data.Model.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.PentagonFormatter", b =>
                {
                    b.HasOne("GFS.Data.Model.Entities.Fabric", "Fabric")
                        .WithMany("PentagonFormatters")
                        .HasForeignKey("FabricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GFS.Data.Model.Entities.Furniture", "Furniture")
                        .WithMany("PentagonFormatters")
                        .HasForeignKey("FurnitureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GFS.Data.Model.Entities.RectangularFormatter", b =>
                {
                    b.HasOne("GFS.Data.Model.Entities.Fabric", "Fabric")
                        .WithMany("RectangularFormatters")
                        .HasForeignKey("FabricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GFS.Data.Model.Entities.Furniture", "Furniture")
                        .WithMany("RectangularFormatters")
                        .HasForeignKey("FurnitureId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
