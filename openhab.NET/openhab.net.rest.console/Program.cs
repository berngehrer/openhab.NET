using openhab.net.rest.Items;
using System;

namespace openhab.net.rest.console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var a = new TestClass())
            {
                a.SwitchTest();
                Console.ReadLine();
            }
        }
    }

    class TestClass : IDisposable
    {
        ItemContext _context;

        public TestClass()
        {
            var strategy = new UpdateStrategy(TimeSpan.FromSeconds(3));

            _context = new ItemContext("192.168.178.69", 8080, strategy);
            _context.Refreshed += Context_Refreshed;
        }

        public async void SwitchTest()
        {
            var tvled = await _context.GetByName<SwitchItem>("MQTT_TVLED_POW");
            //tvled.Toggle();
        }
                
        void Context_Refreshed(object sender, ContextRefreshedEventArgs<OpenhabItem> args)
        {
            Console.WriteLine(args.Element.Name);
        }

        public void Dispose()
        {
            _context.Refreshed -= Context_Refreshed;
            _context.Dispose();
        }
    }
}
