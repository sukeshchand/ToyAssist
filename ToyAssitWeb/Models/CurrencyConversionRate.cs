namespace ToyAssist.Web.Models
{
    public class CurrencyConversionRate
    {
        public string BaseCurrency { get; set; }
        public string ToCurrency { get; set; }
        public int BaseCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
