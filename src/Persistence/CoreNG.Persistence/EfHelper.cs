using Microsoft.EntityFrameworkCore;

namespace CoreNG.Persistence
{
    public static class EfHelper
    {
        public static bool Like(string column, string value)
        {
            return EF.Functions.Like(column, string.Format("%{0}%", value));
        }
    }
}