
namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string Username)
    : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

internal class DeleteBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        // Delete Basket entity from command object
        // save to database
        // return result
        
        var basket = await dbContext.ShoppingCarts
            .SingleOrDefaultAsync(x => x.UserName == command.Username, cancellationToken);

        if (basket == null)
        {
            throw new BasketNotFoundException(command.Username); 
        }

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteBasketResult(true);
    }
}
