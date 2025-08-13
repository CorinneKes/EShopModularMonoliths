using StackExchange.Redis;
using Order = Ordering.Orders.Models.Order;
namespace Ordering.Orders.Events;

public record OrderCreatedEvent(Order Order)
    : IDomainEvent;
