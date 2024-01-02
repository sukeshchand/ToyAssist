namespace ToyAssist.Web.ViewModels
{
    public class CurrencyViewModel
    {
        public int CurrencyId { get; set; }

        public string CurrencyCode { get; set; } = null!;

        public string CurrencyName { get; set; } = null!;

        public string? CurrencySymbol { get; set; }
    }

}
