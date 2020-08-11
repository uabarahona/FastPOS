namespace FastPOS.Core
{
    public interface ISalesTaxCalculator
    {
        public Money GetTax(SalesOrder order);
    }
}