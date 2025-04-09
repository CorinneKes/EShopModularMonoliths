
namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string Username)
    : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

internal class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        // Delete Basket entity from command object
        // save to database
        // return result

        var basket = await repository.DeleteBasket(command.Username, cancellationToken); 
        
        return new DeleteBasketResult(true);
    }
}
