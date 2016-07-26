using openhab.net.rest.Items;

namespace openhab.net.rest.console
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var context = new ItemContext(new ClientSettings("192.168.178.69")))
            //{
            //    var task = context.GetByName("MQTT_TVLED_POW");
            //    task.Wait();
            //    var tvled = task.Result as Items.SwitchItem;
            //    tvled.Value = !tvled.Value;
            //}
            SwitchTest();
            System.Console.ReadLine();
        }

        static async void SwitchTest()
        {
            using (var context = new ItemContext("192.168.178.69", 8080))
            {
                //var tvled = await context.GetByName<SwitchItem>("MQTT_TVLED_POW");
                //tvled.Toggle();

                var item = await context.GetByName("MQTT_TVLED_POW");
                var tvled = item as SwitchItem;
                tvled.Value = !tvled.Value; 
            }
        }
    }
}
