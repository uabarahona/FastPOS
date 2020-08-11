namespace FastPOS.Core
{
    public class LineItem : ProductStock
    {
        public LineItem(ProductInfo product, int quantity) : base(product, quantity) { }

        public Money GetTotalPrice()
        {
            return Product.Price * Quantity;
        }
    }
}
