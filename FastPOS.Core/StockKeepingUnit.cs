using System;

namespace FastPOS.Core
{
    public readonly struct StockKeepingUnit
    {
        private readonly string skuCode;

        public StockKeepingUnit(string skuCode)
        {
            this.skuCode = skuCode;
        }

        public override string ToString() => skuCode;

        public static StockKeepingUnit NewSKU() => new StockKeepingUnit(Guid.NewGuid().ToString().Substring(0, 10));
    }
}
