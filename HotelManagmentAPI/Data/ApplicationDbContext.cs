using HotelManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagmentAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Id = 1,
                    Name = "Sample Hotel 1",
                    Address = "123 Main St",
                    ImageUrl = "https://example.com/hotel1.jpg",
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
                    ImageUrl = "https://example.com/hotel2.jpg",
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
                    ImageUrl = "https://example.com/hotel3.jpg",
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
                    ImageUrl = "https://example.com/hotel4.jpg",
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
                    ImageUrl = "https://example.com/hotel5.jpg",
                    Description = "A cozy retreat for nature lovers.",
                    Rating = 4.2m,
                    PhoneNumber = "222-555-1111",
                    Email = "hotel5@gmail.com",
                    CreatedDate = DateTime.Now
                },
            };
            modelBuilder.Entity<Hotel>().HasData(hotels);
        }
    }
}
