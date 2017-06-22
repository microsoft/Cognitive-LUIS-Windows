using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;
using System.Threading;

namespace LUISAPI_Console_Sample
{
    public class Program
    {
        private const string subscriptionKey = "78e2fbd8b5c74a12b8706822937ca2b3";
        private const string appKey = "0a8265610dfb4f25a526bc814247fe18";

        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var manager = new LuisManager(subscriptionKey); //Creates Luis Manager handler.
                var application = await manager.Apps.AddApplicationAsync("MyApp"); //Creates an application called "MyApp".

                var intention = await application.Intentions.AddIntentionAsync("Intention.Example");

                await intention.Examples.AddLabelAsync("This is an example");
                await intention.Examples.AddLabelsAsync(new List<string>()
                {
                    "This is other example",
                    "Here is another example",
                    "What about this example?"
                });

                await application.Training.TrainAsync();
                while (true)
                {
                    var status = await application.Training.GetTrainingStatusAsync();
                    if (status == "Success") break;
                    Thread.Sleep(500);
                }

                await application.Settings.AssignAppKey(appKey);
                await application.Settings.PublishAsync();

                var client = new LuisClient(application.Id, appKey);
                var result = await client.Predict("An example");

                await manager.Apps.DeleteApplicationAsync(application.Id); //Deletes "MyApp" application.
            });

            Task.WaitAll();
            Console.ReadLine();
        }
    }
}
