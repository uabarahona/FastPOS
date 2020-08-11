using System.Collections.Generic;

namespace FastPOS.Core
{
    public class LocalStorage : ProductStockContainer<ProductInventory>
    {
        public LocalStorage(ICollection<ProductInventory> lineItems = default) : 
            base(ProductAddRules.DisableExceptionOnZero, ProductDecreaseRules.DisableDeleteOnZero, lineItems) { }

        protected override ProductInventory GetNewProductStock(ProductInfo product, int quantity) => new ProductInventory(product, quantity);
    }
}