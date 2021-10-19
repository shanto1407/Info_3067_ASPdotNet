﻿// <auto-generated />
using System;
using Casestudy.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Casestudy.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200617213124_azurez")]
    partial class azurez
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CaseStudyAPI.DAL.DomainClasses.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("OrderAmount")
                        .HasColumnType("money");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CaseStudyAPI.DAL.DomainClasses.OrderLineItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("QtyBackOrdered")
                        .HasColumnType("int");

                    b.Property<int>("QtyOrdered")
                        .HasColumnType("int")
                        .HasMaxLength(15);

                    b.Property<int>("QtySold")
                        .HasColumnType("int");

                    b.Property<decimal>("SellingPrice")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderLineItems");
                });

            modelBuilder.Entity("Casestudy.DAL.DomainClasses.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<byte[]>("Timer")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Casestudy.DAL.DomainClasses.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Casestudy.DAL.DomainClasses.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("BrandId")
                        .HasColumnType("int");

                    b.Property<decimal>("CostPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("GraphicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MSRP")
                        .HasColumnType("money");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QtyOnBackOrder")
                        .HasColumnType("int");

                    b.Property<decimal>("QtyOnHand")
                        .HasColumnType("money");

                    b.Property<byte[]>("Timer")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CaseStudyAPI.DAL.DomainClasses.OrderLineItem", b =>
                {
                    b.HasOne("CaseStudyAPI.DAL.DomainClasses.Order", "Order")
                        .WithMany("OrderLineItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casestudy.DAL.DomainClasses.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Casestudy.DAL.DomainClasses.Product", b =>
                {
                    b.HasOne("Casestudy.DAL.DomainClasses.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId");
                });
#pragma warning restore 612, 618
        }
    }
}
