using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;

public class ProductCreatedEventHandler(IBus bus, ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        
        // Publish product price changed integration event to update basket prices
        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Category = notification.Product.Category,
            Description = notification.Product.Description,
            ImageFile = notification.Product.ImageFile,
            Price = notification.Product.Price // set updated product price
        };

        await bus.Publish(integrationEvent, cancellationToken);
    
    }
}
