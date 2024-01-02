using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class CurrencyConversionRateViewModelMapper
    {
        public static CurrencyConversionRateViewModel? Map(CurrencyConversionRate item)
        {
            if (item == null)
            {
                return null;
            }

            return new CurrencyConversionRateViewModel()
            {
                BaseCurrencyId = item.BaseCurrencyId,
                ToCurrencyId = item.ToCurrencyId,
                ConversionRate = item.ConversionRate,
                BaseCurrency = CurrencyViewModelMapper.Map(item.BaseCurrency),
                ToCurrency = CurrencyViewModelMapper.Map(item.ToCurrency),
            };
        }
    }
}
