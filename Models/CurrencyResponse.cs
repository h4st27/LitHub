namespace Libra.Models
{
    public class ExchangeRateResponse
    {
        public MetaData Meta { get; set; }
        public Dictionary<string, CurrencyData> Data { get; set; }
    }

    public class MetaData
    {
        public DateTime LastUpdatedAt { get; set; }
    }

    public class Currency
    {
        public string Name { get; set; }
        public CurrencyData Data { get; set; }
    }
    public class CurrencyData
    {
        public string Code { get; set; }
        public decimal Value { get; set; }
    }
}
