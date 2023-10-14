using DogApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogApp.ConfigureClasses
{
    public class DogConfiguration : IEntityTypeConfiguration<Dog>
    {
        public void Configure(EntityTypeBuilder<Dog> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name).IsRequired().HasColumnType("NVARCHAR(100)");
            builder.Property(d => d.Color).IsRequired().HasColumnType("NVARCHAR(100)");
            builder.Property(d => d.TailLength).IsRequired().HasColumnType("FLOAT");
            builder.Property(d => d.Weight).IsRequired().HasColumnType("FLOAT");

            builder.HasCheckConstraint("CK_TailLength", "TailLength >= 0");
            builder.HasCheckConstraint("CK_Weight", "Weight >= 0");

            builder.HasData(
                new Dog 
                {
                    Id = 1,
                    Name = "Jessy",
                    Color = "Black",
                    TailLength = 25,
                    Weight = 5
                },
                new Dog
                {
                    Id = 2,
                    Name = "Neo",
                    Color = "White",
                    TailLength = 15,
                    Weight = 3
                },
                new Dog
                {
                    Id = 3,
                    Name = "Doggy",
                    Color = "Brown",
                    TailLength = 50,
                    Weight = 15
                },
                new Dog
                {
                    Id = 4,
                    Name = "Chessy",
                    Color = "White&Brown",
                    TailLength = 10,
                    Weight = 2
                },
                new Dog
                {
                    Id = 5,
                    Name = "Bob",
                    Color = "Gray",
                    TailLength = 60,
                    Weight = 20
                },
                new Dog
                {
                    Id = 6,
                    Name = "Jessy",
                    Color = "Black",
                    TailLength = 25,
                    Weight = 5
                },
                new Dog
                {
                    Id = 7,
                    Name = "Marko",
                    Color = "Gold",
                    TailLength = 70,
                    Weight = 30
                },
                new Dog
                {
                    Id = 8,
                    Name = "Fido",
                    Color = "Black",
                    TailLength = 100,
                    Weight = 25
                }
            );
        }
    }
}
