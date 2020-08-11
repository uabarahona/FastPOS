namespace FastPOS.Core
{
    public class ProductInventory : ProductStock
    {
        public ProductInventory(ProductInfo product, int quantity) : base(product, quantity) { }
    }
}