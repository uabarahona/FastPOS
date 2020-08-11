using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FastPOS.Core
{
    public abstract class ProductStockContainer<T> where T : ProductStock
    {
        private readonly ProductAddRules _productAddRules;
        private readonly ProductDecreaseRules _productDecreaseRules;
        private readonly IDictionary<StockKeepingUnit, T> _productStockItems;

        public ProductStockContainer(ProductAddRules addRules = default,
                                     ProductDecreaseRules decreaseRules = default,
                                     ICollection<T> initialProducts = default)
        {
            _productAddRules = addRules;
            _productDecreaseRules = decreaseRules;
            _productStockItems = initialProducts is not null ? FromInitialProducts(initialProducts) : new Dictionary<StockKeepingUnit, T>();
        }

        private IDictionary<StockKeepingUnit, T> FromInitialProducts(ICollection<T> initialProducts)
        {
            IDictionary<StockKeepingUnit, T> productStockItems = new Dictionary<StockKeepingUnit, T>();
            foreach (var item in initialProducts)
            {
                productStockItems.Add(item.Product.ProductSKU, item);
            }
            return productStockItems;
        }

        public IReadOnlyCollection<T> ProductStockItems
        {
            get => new ReadOnlyCollection<T>(new List<T>(_productStockItems.Values));
        }

        public void AddProduct(ProductInfo product, int quantity)
        {
            bool equalToZeroExeption = quantity == 0 && !_productAddRules.HasFlag(ProductAddRules.DisableExceptionOnZero);
            bool lessToZeroExpcetion = quantity < 0 && !_productAddRules.HasFlag(ProductAddRules.DisableExceptionOnNegative);

            if (equalToZeroExeption || lessToZeroExpcetion)
            {
                throw new InvalidOperationException($"The {nameof(quantity)} must be greather than 0");
            }

            if (_productStockItems.TryGetValue(product.ProductSKU, out T newItem))
            {
                newItem.IncreaseQuantity(quantity);
            }
            else
            {
                _productStockItems.Add(product.ProductSKU, GetNewProductStock(product, quantity));
            }
        }

        public void DecreaseProduct(ProductInfo product, int quantity)
        {
            bool equalToZeroExeption = quantity == 0 && !_productDecreaseRules.HasFlag(ProductDecreaseRules.DisableDeleteOnZero);
            bool greatherToZeroExpcetion = quantity > 0 && !_productDecreaseRules.HasFlag(ProductDecreaseRules.DisableExceptionOnPositive);

            if (equalToZeroExeption || greatherToZeroExpcetion)
            {
                throw new InvalidOperationException($"The {nameof(quantity)} must be greather less than 0");
            }

            if (_productStockItems.TryGetValue(product.ProductSKU, out T item))
            {
                if (item.Quantity < quantity)
                {
                    throw new InvalidOperationException($"There is no enough amount to decrease {quantity}, current quantity is {item.Quantity}");
                }

                if (item.Quantity == quantity && !_productDecreaseRules.HasFlag(ProductDecreaseRules.DisableDeleteOnZero))
                {
                    _productStockItems.Remove(product.ProductSKU);
                    return;
                }

                item.DecreaseQuantity(quantity);
            }

        }

        protected abstract T GetNewProductStock(ProductInfo product, int quantity);
    }
}