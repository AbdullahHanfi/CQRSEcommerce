using Domain.Entities;

namespace Infrastructure.Persistence.Seed;

public static class Seed
{
    public static List<Product> Products()
    {
        return new()
        {
            new ()
            {
                Id=Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p1",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
            new ()
            {
                Id=Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p2",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
            new ()
            {
                Id=Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p3",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p4",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p5",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p6",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p7",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ImagePath = $"products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg",
                Price= 10,
                ProductCode = "p8",
                Name = "phone",
                MinimumQuantity = 1,
                Category = "phone",
                DiscountRate = 20
            },
        };
    }
}
