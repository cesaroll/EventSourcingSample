using EventSourcing;

var warehouseProductRepository = new WarehouseProductRepository();

var key = string.Empty;

while (key != "X")
{
  Console.WriteLine("R: Receive Product");
  Console.WriteLine("S: Ship Product");
  Console.WriteLine("A: Adjust Inventory");
  Console.WriteLine("Q: Quantity On Hand");
  Console.WriteLine("E: Events");
  Console.WriteLine("X: Exit");
  Console.Write("> ");
  key = Console.ReadLine()?.ToUpperInvariant();
  Console.WriteLine();

  if (key == "X")
    break;

  var sku = GetSkuFromConsole();
  var warehouseProduct = warehouseProductRepository.Get(sku);

  switch (key)
  {
    case "R":
      var receiveInput = GetQuantity();
      if (receiveInput.IsValid) {
        warehouseProduct.ReceiveProduct(receiveInput.Quantity);
        Console.WriteLine($"{sku} received {receiveInput.Quantity}");
      }
      break;
    case "S":
      var shipInput = GetQuantity();
      if (shipInput.IsValid) {
        warehouseProduct.ShipProduct(shipInput.Quantity);
        Console.WriteLine($"{sku} Shipped: {shipInput.Quantity}");
      }
      break;
    case "A":
      var adjustInput = GetQuantity();
      if (adjustInput.IsValid) {
        var reason = GetAdjustmentReason();
        warehouseProduct.AdjustInventory(adjustInput.Quantity, reason);
        Console.WriteLine($"{sku} Adjusted: {adjustInput.Quantity}");
      }
      break;
    case "Q":
      Console.WriteLine($"{sku} Quantity On Hand: {warehouseProduct.GetQuantityOnHand()} on hand");
      break;
    case "E":
      foreach (var @event in warehouseProduct.GetEvents())
      {
        Console.WriteLine(@event);
      }
      break;
  }

  warehouseProductRepository.save(warehouseProduct);

  Console.ReadLine();
  Console.WriteLine();
}

string GetSkuFromConsole() {
  Console.Write("SKU: ");
  var sku = Console.ReadLine();
  Console.WriteLine();
  return sku?? "";
}

ReceiveInput GetQuantity() {
  Console.Write("Quantity: ");
  var line = Console.ReadLine();
  Console.WriteLine();

  if (!int.TryParse(line, out var quantity))
    return new ReceiveInput { IsValid = false };

  return new ReceiveInput { Quantity = quantity, IsValid = true };
}

string GetAdjustmentReason() {
  Console.Write("\nReason: ");
  var reason = Console.ReadLine();
  Console.WriteLine();
  return reason?? "";
}

public class ReceiveInput {
  public int Quantity { get; set; }
  public bool IsValid { get; set; }
}
