using FastPOS.Core;
using System.Collections.Generic;
using System.Linq;

namespace FastPOS.WasmClient
{
    public class AppState
    {
        public AppState()
        {
            SalesOrders = new List<SalesOrder>();


            List<ProductInventory> products = new List<ProductInventory>
            {
                new ProductInventory(new ProductInfo("Credit Card Entel 10", 10m, new StockKeepingUnit("ENTL10")), 10),
                new ProductInventory(new ProductInfo("Credit Card Entel 20", 20m, new StockKeepingUnit("ENTL20")), 220),
                new ProductInventory(new ProductInfo("Credit Card Tigo 30", 30m, new StockKeepingUnit("TIG30")), 20)
            };

            LocalStorage = new LocalStorage(products);
            CurrentSalesOrder = new SalesOrder(new Location(Country.BOLIVIA, City.COCHABAMBA));
        }

        public List<SalesOrder> SalesOrders { get; set; }
        public LocalStorage LocalStorage { get; set; }
        public SalesOrder CurrentSalesOrder { get; set; }

        public Money GetTotalSales() => SalesOrders.Aggregate(new Money(), (next, item) => next += item.GetTotal());
        public void AddProductToSalesOrder(ProductInfo productInfo) => CurrentSalesOrder.AddProduct(productInfo, 1);
        public void ReduceProductFromSalesOrder(ProductInfo productInfo) => CurrentSalesOrder.DecreaseProduct(productInfo, 1);

        public void CompleteCurrentOrder()
        {
            CurrentSalesOrder.CompleteSalesOrder();
            SalesOrders.Add(CurrentSalesOrder);

            CurrentSalesOrder = new SalesOrder(new Location(Country.BOLIVIA, City.COCHABAMBA));
        }
    }
}
