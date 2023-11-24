using System.Globalization;
using System.Linq;
namespace ToyAssist.Web.Helpers
{
    public class CultureHelper
    {
        public static IEnumerable<CultureInfo> GetAllCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .OrderBy(c => c.DisplayName);
        }
    }
}
