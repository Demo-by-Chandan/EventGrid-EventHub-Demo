using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;

namespace ConsoleAppEventHub
{
    internal class Program
    {
        private static EventHubProducerClient eventHubClient;
        private const string EventHubConnectionString = "<connectionstring>";
        private const string EventHubName = "<eventhubname>";
        private const int numMessagesToSend = 100;

        private static async Task Main(string[] args)
        {
            var res = "y";
            do
            {
                eventHubClient = new EventHubProducerClient(EventHubConnectionString, EventHubName);
                List<EventData> list = new List<EventData>();
                for (var i = 0; i < numMessagesToSend; i++)
                {
                    try
                    {
                        var message = $"Message {i}";
                        Console.WriteLine($"Sending message: {message}");
                        list.Add(new EventData(new BinaryData(message)));
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                    }
                }
                await eventHubClient.SendAsync(list);

                Console.WriteLine($"{numMessagesToSend} messages sent.");

                await eventHubClient.CloseAsync();

                Console.WriteLine("Want to try more (y/n)?");
                res = Console.ReadLine();
            } while (res.ToUpper() == "Y");

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}