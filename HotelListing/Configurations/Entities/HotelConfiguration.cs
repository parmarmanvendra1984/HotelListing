using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
