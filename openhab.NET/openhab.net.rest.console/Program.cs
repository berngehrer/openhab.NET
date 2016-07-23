using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openhab.net.rest.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ClientSettings("");
            using (var context = new ItemContext(settings))
            {
                context.Refreshed += Context_Refreshed;
            }
        }

        private static void Context_Refreshed(object sender, ContextRefreshedEventArgs<Items.OpenhabItem> args)
        {
            throw new NotImplementedException();
        }
    }
}
