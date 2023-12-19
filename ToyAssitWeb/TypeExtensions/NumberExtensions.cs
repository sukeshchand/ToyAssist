namespace ToyAssist.Web.TypeExtensions
{
    public static class NumberExtensions
    {
        public static string? ToStringCustom(this decimal? self)
        {
            if(self == null)
            {
                return null;
            }
            return ((decimal)self).ToString("N0");
        }

        public static string? ToStringCustom(this decimal self)
        {
            return ToStringCustom((decimal?)self);
        }
    }
}
