using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class CurrencyConversionRateModelMapper
    {
        public static CurrencyConversionRateModel? Map(CurrencyConversionRate item)
        {
            if (item == null)
            {
                return null;
            }

            return new CurrencyConversionRateModel()
            {
                BaseCurrencyId = item.BaseCurrencyId,
                ToCurrencyId = item.ToCurrencyId,
                ConversionRate = item.ConversionRate,
                BaseCurrency = CurrencyModelMapper.Map(item.BaseCurrency),
                ToCurrency = CurrencyModelMapper.Map(item.ToCurrency),
            };
        }
    }
}
