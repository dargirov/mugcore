﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MugStore.Data;

namespace MugStore.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799");

            modelBuilder.Entity("MugStore.Data.Models.Bulletin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<bool>("Verified");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Bulletins");
                });

            modelBuilder.Entity("MugStore.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasMaxLength(110);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MugStore.Data.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("Highlight");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("PostCode");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("MugStore.Data.Models.Courier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Info");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("MugStore.Data.Models.DeliveryInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<int>("CityId");

                    b.Property<string>("Comment");

                    b.Property<int?>("CourierId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CourierId");

                    b.ToTable("DeliveryInfoes");
                });

            modelBuilder.Entity("MugStore.Data.Models.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsNew");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("MugStore.Data.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Dpi");

                    b.Property<int>("Height");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<int?>("OrderId");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<double>("Rotation");

                    b.Property<int>("Width");

                    b.Property<double>("Y");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OrderId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("MugStore.Data.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(100);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Level");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("MugStore.Data.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<int>("ConfirmationStatus");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("DeliveryInfoId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int>("OrderStatus");

                    b.Property<int>("PaymentMethod");

                    b.Property<decimal>("PriceCustomer");

                    b.Property<decimal>("PriceDelivery");

                    b.Property<decimal>("PriceSupplier");

                    b.Property<int?>("ProductId");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryInfoId");

                    b.HasIndex("ProductId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MugStore.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("FullDescription")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("PageDescription")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("PageTitle")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("ShortDescription")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("MugStore.Data.Models.PostTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.HasKey("Id");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("MugStore.Data.Models.PostTagPost", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("PostTagId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Id");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("PostId", "PostTagId");

                    b.HasIndex("PostTagId");

                    b.ToTable("PostTagPosts");
                });

            modelBuilder.Entity("MugStore.Data.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasMaxLength(310);

                    b.Property<bool>("Active");

                    b.Property<int>("CategoryId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LinkToSource")
                        .HasMaxLength(300);

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("PageTitle")
                        .HasMaxLength(300);

                    b.Property<string>("PreviewData");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MugStore.Data.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("Preview3d");

                    b.Property<int>("ProductId");

                    b.Property<bool>("Thumb");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("MugStore.Data.Models.ProductTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.HasKey("Id");

                    b.ToTable("ProductTags");
                });

            modelBuilder.Entity("MugStore.Data.Models.ProductTagProduct", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("ProductTagId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Id");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("ProductId", "ProductTagId");

                    b.HasIndex("ProductTagId");

                    b.ToTable("ProductTagProducts");
                });

            modelBuilder.Entity("MugStore.Data.Models.DeliveryInfo", b =>
                {
                    b.HasOne("MugStore.Data.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MugStore.Data.Models.Courier", "Courier")
                        .WithMany()
                        .HasForeignKey("CourierId");
                });

            modelBuilder.Entity("MugStore.Data.Models.Image", b =>
                {
                    b.HasOne("MugStore.Data.Models.Order", "Order")
                        .WithMany("Images")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("MugStore.Data.Models.Order", b =>
                {
                    b.HasOne("MugStore.Data.Models.DeliveryInfo", "DeliveryInfo")
                        .WithMany()
                        .HasForeignKey("DeliveryInfoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MugStore.Data.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("MugStore.Data.Models.PostTagPost", b =>
                {
                    b.HasOne("MugStore.Data.Models.Post", "Post")
                        .WithMany("Tags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MugStore.Data.Models.PostTag", "PostTag")
                        .WithMany("Posts")
                        .HasForeignKey("PostTagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MugStore.Data.Models.Product", b =>
                {
                    b.HasOne("MugStore.Data.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MugStore.Data.Models.ProductImage", b =>
                {
                    b.HasOne("MugStore.Data.Models.Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MugStore.Data.Models.ProductTagProduct", b =>
                {
                    b.HasOne("MugStore.Data.Models.Product", "Product")
                        .WithMany("Tags")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MugStore.Data.Models.ProductTag", "ProductTag")
                        .WithMany("Products")
                        .HasForeignKey("ProductTagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
