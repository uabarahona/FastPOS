namespace FastPOS.Core
{
    public class BoliviaSalesTaxCalculator : ISalesTaxCalculator
    {
        private readonly Location location;

        public BoliviaSalesTaxCalculator(Location location)
        {
            this.location = location;
        }

        public Money GetTax(SalesOrder order)
        {
            return order.GetSubTotal() * 0.13m;
        }
    }
}