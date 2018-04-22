using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcanoid
{
    static class UiIdentyfier
    {
        public static string CreateUIElementId(Guid guid)
        {
            string GuidString = Convert.ToBase64String(guid.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Replace("/", "");
            while (Char.IsDigit(GuidString[0]) && GuidString.Length > 0)
                GuidString = GuidString.Remove(0, 1);

            return GuidString;
        }
    }
}
