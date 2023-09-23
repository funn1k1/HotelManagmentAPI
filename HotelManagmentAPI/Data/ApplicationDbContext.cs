using HotelManagment_API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagment_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Id = 1,
                    Name = "Sample Hotel 1",
                    Address = "123 Main St",
                    ImageUrl = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    Description = "A wonderful place to stay.",
                    Rating = 4.5m,
                    PhoneNumber = "123-456-7890",
                    Email = "hotel1@gmail.com",
                    CreatedDate = DateTime.Now
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Sample Hotel 2",
                    Address = "456 Elm St",
                    ImageUrl = "https://www.travelandleisure.com/thmb/pCU_Y9fbQe4CT5Q73J9k2Bqd_bI=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/header-grand-velas-los-cabos-MXALLINC0222-46d3772ad56f4493a83e1bcb49e119f9.jpg",
                    Description = "Another great option for travelers.",
                    Rating = 4.0m,
                    PhoneNumber = "987-654-3210",
                    Email = "hotel2@gmail.com",
                    CreatedDate = DateTime.Now
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Sample Hotel 3",
                    Address = "789 Oak St",
                    ImageUrl = "https://i0.wp.com/insideflyer.com/wp-content/uploads/2022/09/Review-M-Social-Times-Square-New-York-City-USA-VS-46-scaled.jpeg",
                    Description = "Experience luxury at its finest.",
                    Rating = 4.8m,
                    PhoneNumber = "555-123-4567",
                    Email = "hotel3@gmail.com",
                    CreatedDate = DateTime.Now
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Sample Hotel 4",
                    Address = "101 Pine St",
                    ImageUrl = "https://hips.hearstapps.com/clv.h-cdn.co/assets/16/33/3200x2198/gallery-1471831519-iablackhawk.jpg",
                    Description = "Affordable and comfortable accommodations.",
                    Rating = 3.7m,
                    PhoneNumber = "777-999-8888",
                    Email = "hotel4@gmail.com",
                    CreatedDate = DateTime.Now
                },
                new Hotel
                {
                    Id = 5,
                    Name = "Sample Hotel 5",
                    Address = "321 Cedar St",
                    ImageUrl = "https://www.wyndhamgrandclearwater.com/assets/images/escape-clearwater.png",
                    Description = "A cozy retreat for nature lovers.",
                    Rating = 4.2m,
                    PhoneNumber = "222-555-1111",
                    Email = "hotel5@gmail.com",
                    CreatedDate = DateTime.Now
                },
            };
            modelBuilder.Entity<Hotel>().HasData(hotels);

            var rooms = new List<Room>
            {
                new Room
                {
                    Id = 1,
                    HotelId = 1,
                    RoomNumber = "101",
                    PricePerNight = 150.00m,
                    IsAvailable = true,
                },
                new Room
                {
                    Id = 2,
                    HotelId = 1,
                    RoomNumber = "102",
                    PricePerNight = 175.00m,
                    IsAvailable = false,
                },
                new Room
                {
                    Id = 3,
                    HotelId = 1,
                    RoomNumber = "103",
                    PricePerNight = 130.00m,
                    IsAvailable = false,
                },
                new Room
                {
                    Id = 4,
                    HotelId = 2,
                    RoomNumber = "Standard double room",
                    PricePerNight = 150.00m,
                    IsAvailable = false,
                },
                new Room
                {
                    Id = 5,
                    HotelId = 2,
                    RoomNumber = "Deluxe sea view suite",
                    PricePerNight = 500.00m,
                    IsAvailable = true,
                },
                new Room
                {
                    Id = 6,
                    HotelId = 3,
                    RoomNumber = "202A",
                    PricePerNight = 100.00m,
                    IsAvailable = true,
                },
                new Room
                {
                    Id = 7,
                    HotelId = 4,
                    RoomNumber = "200",
                    PricePerNight = 450.00m,
                    IsAvailable = false,
                },
                new Room
                {
                    Id = 8,
                    HotelId = 4,
                    RoomNumber = "201",
                    PricePerNight = 500.00m,
                    IsAvailable = true,
                },
                new Room
                {
                    Id = 9,
                    HotelId = 5,
                    RoomNumber = "Standard single room",
                    PricePerNight = 115.00m,
                    IsAvailable = false,
                },
                new Room
                {
                    Id = 10,
                    HotelId = 5,
                    RoomNumber = "Family room with two beds",
                    PricePerNight = 200.00m,
                    IsAvailable = true,
                },
            };
            modelBuilder.Entity<Room>().HasData(rooms);
        }
    }
}
