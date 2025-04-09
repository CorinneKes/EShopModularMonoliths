
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            if (!asNoTracking) 
            {         
                return await repository.GetBasket(userName, asNoTracking, cancellationToken);
            }

            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken); 
            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket = await repository.GetBasket(userName, asNoTracking, cancellationToken);

            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.CreateBasket(basket, cancellationToken);

            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);   
            
            return await repository.CreateBasket(basket, cancellationToken); 
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);

            await cache.RemoveAsync(userName, cancellationToken);

            return true; 

        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await repository.SaveChangesAsync(cancellationToken);

            // TODO: Clear cache
        }
    }
}
