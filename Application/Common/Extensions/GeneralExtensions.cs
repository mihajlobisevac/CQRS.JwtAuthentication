using System;
using System.Linq;

namespace Application.Common.Extensions
{
    public static class GeneralExtensions
    {
        public static string GenerateRandomString(int length)
        {
            var rnd = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string(
                Enumerable.Repeat(chars, length)
                    .Select(x => x[rnd.Next(x.Length)])
                    .ToArray());
        }

        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var datetimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return datetimeValue.AddSeconds(timestamp).ToUniversalTime();
        }
    }
}
