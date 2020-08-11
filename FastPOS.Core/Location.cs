namespace FastPOS.Core
{
    public class Location
    {
        private readonly Country country;
        private readonly City city;

        public Location(Country country, City city)
        {
            this.country = country;
            this.city = city;
        }

        public ISalesTaxCalculator GetSalesTaxCalculator() =>
            country switch
            {
                Country.BOLIVIA => new BoliviaSalesTaxCalculator(this),
                _ => default
            };
    }
}