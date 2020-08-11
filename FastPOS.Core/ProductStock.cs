using System;

namespace FastPOS.Core
{
    public abstract class ProductStock
    {
        public ProductStock(ProductInfo product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public int Quantity { get; private set; }
        public ProductInfo Product { get; }

        public void IncreaseQuantity(int quantity = 1)
        {
            Quantity += quantity;
        }

        public void DecreaseQuantity(int quantity = 1)
        {
            Quantity -= quantity;
        }
    }
}
