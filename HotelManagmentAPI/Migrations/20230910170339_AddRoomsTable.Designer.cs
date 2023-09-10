﻿// <auto-generated />
using System;
using HotelManagmentAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelManagmentAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230910170339_AddRoomsTable")]
    partial class AddRoomsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HotelManagmentAPI.Models.Hotel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Rating")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St",
                            CreatedDate = new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2919),
                            Description = "A wonderful place to stay.",
                            Email = "hotel1@gmail.com",
                            ImageUrl = "https://example.com/hotel1.jpg",
                            Name = "Sample Hotel 1",
                            PhoneNumber = "123-456-7890",
                            Rating = 4.5m
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Elm St",
                            CreatedDate = new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2933),
                            Description = "Another great option for travelers.",
                            Email = "hotel2@gmail.com",
                            ImageUrl = "https://example.com/hotel2.jpg",
                            Name = "Sample Hotel 2",
                            PhoneNumber = "987-654-3210",
                            Rating = 4.0m
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Oak St",
                            CreatedDate = new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2936),
                            Description = "Experience luxury at its finest.",
                            Email = "hotel3@gmail.com",
                            ImageUrl = "https://example.com/hotel3.jpg",
                            Name = "Sample Hotel 3",
                            PhoneNumber = "555-123-4567",
                            Rating = 4.8m
                        },
                        new
                        {
                            Id = 4,
                            Address = "101 Pine St",
                            CreatedDate = new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2938),
                            Description = "Affordable and comfortable accommodations.",
                            Email = "hotel4@gmail.com",
                            ImageUrl = "https://example.com/hotel4.jpg",
                            Name = "Sample Hotel 4",
                            PhoneNumber = "777-999-8888",
                            Rating = 3.7m
                        },
                        new
                        {
                            Id = 5,
                            Address = "321 Cedar St",
                            CreatedDate = new DateTime(2023, 9, 10, 20, 3, 39, 28, DateTimeKind.Local).AddTicks(2940),
                            Description = "A cozy retreat for nature lovers.",
                            Email = "hotel5@gmail.com",
                            ImageUrl = "https://example.com/hotel5.jpg",
                            Name = "Sample Hotel 5",
                            PhoneNumber = "222-555-1111",
                            Rating = 4.2m
                        });
                });

            modelBuilder.Entity("HotelManagmentAPI.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("HotelId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<decimal>("PricePerNight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HotelId");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            HotelId = 1,
                            IsAvailable = true,
                            PricePerNight = 150.00m,
                            RoomNumber = "101"
                        },
                        new
                        {
                            Id = 2,
                            HotelId = 1,
                            IsAvailable = false,
                            PricePerNight = 175.00m,
                            RoomNumber = "102"
                        },
                        new
                        {
                            Id = 3,
                            HotelId = 1,
                            IsAvailable = false,
                            PricePerNight = 130.00m,
                            RoomNumber = "103"
                        },
                        new
                        {
                            Id = 4,
                            HotelId = 2,
                            IsAvailable = false,
                            PricePerNight = 150.00m,
                            RoomNumber = "Standard double room"
                        },
                        new
                        {
                            Id = 5,
                            HotelId = 2,
                            IsAvailable = true,
                            PricePerNight = 500.00m,
                            RoomNumber = "Deluxe sea view suite"
                        },
                        new
                        {
                            Id = 6,
                            HotelId = 3,
                            IsAvailable = true,
                            PricePerNight = 100.00m,
                            RoomNumber = "202A"
                        },
                        new
                        {
                            Id = 7,
                            HotelId = 4,
                            IsAvailable = false,
                            PricePerNight = 450.00m,
                            RoomNumber = "200"
                        },
                        new
                        {
                            Id = 8,
                            HotelId = 4,
                            IsAvailable = true,
                            PricePerNight = 500.00m,
                            RoomNumber = "201"
                        },
                        new
                        {
                            Id = 9,
                            HotelId = 5,
                            IsAvailable = false,
                            PricePerNight = 115.00m,
                            RoomNumber = "Standard single room"
                        },
                        new
                        {
                            Id = 10,
                            HotelId = 5,
                            IsAvailable = true,
                            PricePerNight = 200.00m,
                            RoomNumber = "Family room with two beds"
                        });
                });

            modelBuilder.Entity("HotelManagmentAPI.Models.Room", b =>
                {
                    b.HasOne("HotelManagmentAPI.Models.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });
#pragma warning restore 612, 618
        }
    }
}
