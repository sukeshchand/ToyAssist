namespace ToyAssist.Web.Models
{
    public class CurrencyConversionRateModel
    {
        public int BaseCurrencyId { get; set; }

        public int ToCurrencyId { get; set; }

        public decimal ConversionRate { get; set; }

        public CurrencyModel? BaseCurrency { get; set; } = null!;

        public CurrencyModel? ToCurrency { get; set; } = null!;
    }
}
