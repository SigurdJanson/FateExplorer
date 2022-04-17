using FateExplorer.GameData;

namespace FateExplorer.Shop
{
    public class CurrencyM
    {
        public CurrencyM(CurrencyDbEntry currencyDbEntry)
        {
            this.Id = currencyDbEntry.Id;
            this.Origin = currencyDbEntry.Origin;
            this.Name = currencyDbEntry.Name;
            this.Rate = currencyDbEntry.Rate;
        }


        public string Id { get; }

        public string Origin { get; }

        public string Name { get; }

        public double Rate { get; }
    }
}