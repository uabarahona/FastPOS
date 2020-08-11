using System;
using System.Collections.Generic;
using System.Linq;

namespace FastPOS.Core
{
    public class SalesOrder : ProductStockContainer<LineItem>
    {
        private readonly Location _origin;
        private readonly Customer _customer;

        private Money _amountReceived;

        public SalesOrder(Location origin, Customer customer = default)
        {
            _origin = origin;
            _customer = customer;

            CreatedDate = DateTime.Now;
            Status = SalesOrderStatus.WaitingForPayment;
        }

        public SalesOrderStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public Money GetTaxes()
        {
            return _origin.GetSalesTaxCalculator().GetTax(this);
        }

        public Money GetSubTotal()
        {
            return ProductStockItems.Aggregate(new Money(), (total, next) => total + next.GetTotalPrice());
        }

        public Money GetTotal()
        {
            return GetSubTotal() + GetTaxes();
        }

        public Money AddRecievedAmount(Money amountRecieved)
        {
            _amountReceived = amountRecieved;

            return GetTotal() < _amountReceived ? _amountReceived - GetTotal() : 0;
        }

        public IReadOnlyCollection<ProductStock> CompleteSalesOrder()
        {
            if (_amountReceived < GetTotal())
            {
                throw new InvalidOperationException("Cannot close the Sales order without a complete payment");
            }

            Status = SalesOrderStatus.Fulfilled;

            return ProductStockItems;
        }

        protected override LineItem GetNewProductStock(ProductInfo product, int quantity) => new LineItem(product, quantity);
    }
}