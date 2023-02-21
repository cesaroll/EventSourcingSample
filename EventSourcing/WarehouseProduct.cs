using System;

namespace EventSourcing
{
    public class CurrentState
    {
        public int QuantityOnHand { get; set; }
    }

    public class WarehouseProduct
    {
        private readonly IList<IEvent> _events = new List<IEvent>();

        // Projection (Current State)
        private readonly CurrentState _currentState = new ();

        public string Sku {get;}

        public WarehouseProduct (string sku)
        {
            Sku = sku;
        }

        public void ShipProduct(int quantity)
        {
            if (quantity > _currentState.QuantityOnHand)
                throw new InvalidDomainException("Not enough product in stock to ship");

            AddEvent(new ProductShipped(Sku, quantity, DateTime.UtcNow, x => x - quantity));
        }

        public void ReceiveProduct(int quantity)
        {
            AddEvent(new ProductReceived(Sku, quantity, DateTime.UtcNow, x => x + quantity));
        }

        public void AdjustInventory(int quantity, string reason)
        {
            if (_currentState.QuantityOnHand + quantity < 0)
                throw new InvalidDomainException("Not enough product in stock to adjust");

            AddEvent(new InventoryAdjusted(Sku, quantity, reason, DateTime.UtcNow, x => x + quantity));
        }

        public void AddEvent(IEvent @event)
        {
            _currentState.QuantityOnHand = @event.Apply(_currentState.QuantityOnHand);
            _events.Add(@event);
        }

        public IList<IEvent> GetEvents()
        {
            return _events;
        }

        public int GetQuantityOnHand()
        {
            return _currentState.QuantityOnHand;
        }

    }

    public class InvalidDomainException : Exception
    {
        public InvalidDomainException(string message) : base(message)
        {
        }
    }
}
