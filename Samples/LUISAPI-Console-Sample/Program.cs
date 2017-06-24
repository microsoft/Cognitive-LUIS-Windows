using Microsoft.Cognitive.LUIS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace LUISAPI_Console_Sample
{
    public class Program
    {
        private const string subscriptionKey = "<INSERT YOUR SUBSCRIPTION KEY HERE>";
        private const string appKey = "<INSERT YOUR APP KEY HERE>";

        public static void Main(string[] args)
        {
            //TODO: Documentation internal
            //TODO: Documentation .md
            Task.Run(async () =>
            {
                var manager = new LuisManager(subscriptionKey); //Creates Luis Manager handler.

                var application = await manager.Apps.CreateApplicationAsync("MyApp"); //Creates an application called "MyApp".
                //var application = await manager.Apps.GetApplicationAsync("MyApp");
                //var application = await manager.Apps.GetApplicationAsync(new Guid("3e3d485f-1179-4c1c-998b-3e0bd4fd4cc0"));

                var intent = await application.Intent.AddIntentionAsync("Intention.Example");
                //var intent = await application.Intent.GetIntentionAsync("Intention.Example");
                //var intent = await application.Intent.GetIntentionAsync(new Guid("60eb6f9b-11e7-4c2d-a19a-630897a1fb29"));

                await intent.Examples.AddLabelAsync("This is an example");
                await intent.Examples.AddLabelsAsync(new List<string>()
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