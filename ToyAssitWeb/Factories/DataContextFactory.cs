using Microsoft.EntityFrameworkCore;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Helpers;

namespace ToyAssist.Web.Factories
{
    public class DataContextFactory
    {
        public static _DataContext Create()
        {
            var connectionString = (new ConfigurationReader()).GetSetting("ConnectionStrings:MainConnection");
            var options = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<_DataContext>(), connectionString).Options;
            return new _DataContext(options);
        }
    }
}
