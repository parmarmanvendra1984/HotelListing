using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamicia",
                    ShortName = "JM"
                },
                 new Country
                 {
                     Id = 2,
                     Name = "Bahamas",
                     ShortName = "BS"
                 },
                  new Country
                  {
                      Id = 3,
                      Name = "Cayman Island",
                      ShortName = "CI"
                  },
                   new Country
                   {
                       Id = 4,
                       Name = "India",
                       ShortName = "IN"
                   }

                );

            builder.Entity<Hotel>().HasData(
               new Hotel
               {
                   Id = 1,
                   Name = "Sandals Resort and Spa",
                   Address = "Negril",
                   CountryID = 1,
                   Rating = 2.5
               },
                new Hotel
                {
                    Id = 2,
                    Name = "Confort Suited",
                    Address = "George Town",
                    CountryID = 2,
                    Rating = 4.5
                },
                 new Hotel
                 {
                     Id = 3,
                     Name = "Grand Palldium",
                     Address = "Nassua",
                     CountryID = 3,
                     Rating = 3.8
                 },
                 new Hotel
                 {
                     Id = 4,
                     Name = "JP Hotel",
                     Address = "India",
                     CountryID = 4,
                     Rating = 4.25
                 }

               );
        }

    }
}
