using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;

namespace LUISAPI_Console_Sample
{
    public class Program
    {
        private const string programmaticAPIKey = "4c5ddb9a802e43f1b5c9d6a20a9b3ffe";

        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var manager = new LuisManager(programmaticAPIKey); //Creates Luis Manager handler.
                var id = await manager.Apps.AddApplicationAsync("My App"); //Creates an application called "My App".
                await manager.Apps.DeleteApplicationAsync(id); //Deletes the "My App" application.
            });

            Task.WaitAll();
            Console.ReadLine();
        }
    }
}
