using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace Services.Utils
{
    public static class Util
    {
        public static string GenerateUniqueTitle(string title, HashSet<string> existingTitles)
        {
            var baseTitle = title;
            int counter = 1;

            while (existingTitles.Contains(title))
            {
                title = $"{baseTitle} ({counter})";
                counter++;
            }

            return title;
        }
    }
}
