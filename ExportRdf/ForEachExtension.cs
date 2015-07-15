using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportRdf
{
    public static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> pPerfomAction)
        {
            foreach (T item in items)
            {
                pPerfomAction(item);
            }
        }
    }

}
