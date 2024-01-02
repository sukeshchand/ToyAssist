using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class CurrencyViewModelMapper
    {
        public static CurrencyViewModel? Map(Currency? item)
        {
            if(item == null)
            {
                return null;
            }

            return new CurrencyViewModel()
            {
                CurrencyId = item.CurrencyId,
                CurrencyName = item.CurrencyName,
                CurrencyCode = item.CurrencyCode,
                CurrencySymbol = item.CurrencySymbol,
            };
        }
    }
}
