﻿namespace Catalog.Products.Models
{
    public class Product : Aggregate<Guid> // Id in Entity.cs is of the Type Guid
    {
        public string Name { get; private set; } = default!;
        public List<string> Category { get; private set; } = new();
        public string Description { get; private set; } = default!;
        public string ImageFile { get; private set; } = default!;
        public decimal Price  { get; private set; }

        public static Product Create(Guid id, string name, List<string> category, string description, string iamgeFile, decimal price) 
        { 
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var product = new Product
            {
                Id = id,
                Name = name,
                Category = category,
                Description = description,
                ImageFile = iamgeFile,
                Price = price
            };

            product.AddDomainEvent(new ProductCreatedEvent(product));

            return product;
        }

        public void Update(string name, List<string> category, string description, string imageFile, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            // Update Product entity fields
            Name = name;
            Category = category;
            Description = description;
            ImageFile = imageFile;

            // If the price has changed, raise a ProductPriceChanged domain event
            if (Price != price)
            {
                Price = price;
                AddDomainEvent(new ProductPriceChangedEvent(this));
            }
        }
    }
}
