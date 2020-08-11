namespace FastPOS.Core
{
    public readonly struct Money
    {
        private readonly decimal amount;

        public Money(decimal amount)
        {
            this.amount = amount;
        }

        public readonly override string ToString() => $"{amount:0.00}";

        public static implicit operator Money(decimal amount) => new Money(amount);

        public static Money operator *(Money a, decimal b) => new Money(a.amount * b);

        public static Money operator +(Money a, Money b) => new Money(a.amount + b.amount);

        public static Money operator -(Money a, Money b) => new Money(a.amount - b.amount);

        public static bool operator <(Money a, Money b) => a.amount < b.amount;

        public static bool operator >(Money a, Money b) => a.amount > b.amount;
    }
}