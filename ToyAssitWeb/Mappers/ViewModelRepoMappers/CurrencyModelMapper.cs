using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class CurrencyModelMapper
    {
        public static CurrencyModel? Map(Currency? item)
        {
            if(item == null)
            {
                return null;
            }

            return new CurrencyModel()
            {
                CurrencyId = item.CurrencyId,
                CurrencyName = item.CurrencyName,
                CurrencyCode = item.CurrencyCode,
                CurrencySymbol = item.CurrencySymbol,
            };
        }
    }
}
