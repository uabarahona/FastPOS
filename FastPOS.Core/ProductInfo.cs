namespace FastPOS.Core
{
    public class ProductInfo
    {
        public ProductInfo(
            string name,
            Money price,
            StockKeepingUnit? productSKU = default)
        {
            Name = name;
            Price = price;
            ProductSKU = productSKU ?? StockKeepingUnit.NewSKU();
        }

        public string Name { get; }

        public Money Price { get; }

        public StockKeepingUnit ProductSKU { get; }
    }
}