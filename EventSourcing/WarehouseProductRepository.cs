using System;

namespace EventSourcing
{
    public class WarehouseProductRepository
    {
        private readonly Dictionary<string, IList<IEvent>> _inMemoryStreams = new ();

        public WarehouseProduct Get(string sku)
        {
            var warehouseProduct = new WarehouseProduct(sku);

            if (_inMemoryStreams.ContainsKey(sku))
            {
                foreach (var @event in _inMemoryStreams[sku])
                {
                    warehouseProduct.AddEvent(@event);
                }
            }

            return warehouseProduct;
        }

        public void save (WarehouseProduct warehouseProduct)
        {
            _inMemoryStreams[warehouseProduct.Sku] = warehouseProduct.GetEvents();
        }
    }
}
