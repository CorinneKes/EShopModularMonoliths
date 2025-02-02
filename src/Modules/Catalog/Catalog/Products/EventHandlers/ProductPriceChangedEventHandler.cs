namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: publish product price changed integration event for updating basket prices
        logger.LogInformation("Domain Event handlet: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
