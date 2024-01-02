namespace ToyAssist.Web.ViewModels
{
    public class CurrencyConversionRateViewModel
    {
        public int BaseCurrencyId { get; set; }

        public int ToCurrencyId { get; set; }

        public decimal ConversionRate { get; set; }

        public CurrencyViewModel? BaseCurrency { get; set; } = null!;

        public CurrencyViewModel? ToCurrency { get; set; } = null!;
    }
}
