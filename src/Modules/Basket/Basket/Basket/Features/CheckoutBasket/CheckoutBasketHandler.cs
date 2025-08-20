
using MassTransit;
using Shared.Messaging.Events;
using System.Text.Json;

namespace Basket.Basket.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout)
    : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("Username is required.");
    }
}

internal class CheckoutBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        await using var transaction =
            await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var basket = await dbContext.ShoppingCarts
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.UserName == command.BasketCheckout.UserName, cancellationToken);

            if (basket == null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }

            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccurredOn = DateTime.UtcNow
            };

            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new CheckoutBasketResult(true);

        }
        catch 
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false);
        
        }


        ///// CHECKOUT BASKET WITHOUT OUTBOX
        
        //var basket = await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);
        //var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
        //eventMessage.TotalPrice = basket.TotalPrice;
        //await bus.Publish(eventMessage, cancellationToken);
        //await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);
        //return new CheckoutBasketResult(true);
    }
}
