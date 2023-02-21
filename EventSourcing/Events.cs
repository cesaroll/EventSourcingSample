using System.Linq.Expressions;
using System;

namespace EventSourcing
{
    public interface IEvent {
        Func<int, int> Apply { get; }
    }

    public record ProductShipped(string Sku, int Quantity, DateTime DateTime, Func<int, int> Apply) : IEvent;

    public record ProductReceived(string Sku, int Quantity, DateTime DateTime, Func<int, int> Apply) : IEvent;

    public record InventoryAdjusted(string Sku, int Quantity, string Reason, DateTime DateTime, Func<int, int> Apply) : IEvent;
}
