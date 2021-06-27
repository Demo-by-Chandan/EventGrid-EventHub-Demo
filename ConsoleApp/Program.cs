using Newtonsoft.Json;
using System;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Azure;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();

            string topicEndpoint = configuration["EventGridTopicEndpoint"];
            string apiKey = configuration["EventGridAccessKey"];

            var res = "y";
            do
            {
                EventGridPublisherClient client = new EventGridPublisherClient(
                    new Uri(topicEndpoint),
                    new AzureKeyCredential(apiKey));
                Console.WriteLine("Enter the message you want to send (Blank : \"This is an automated message\"):");
                var message = Console.ReadLine();

                Console.WriteLine("Event Type (Blank : \"MyCompany.Items.NewItemCreated\"):");
                var eventType = Console.ReadLine();

                message = string.IsNullOrWhiteSpace(message) ? "This is an automated message" : message;
                eventType = string.IsNullOrWhiteSpace(eventType) ? "MyCompany.Items.NewItemCreated" : message;
                var eventOne = new EventGridEvent(
                    "ExampleEventSubject",
                    eventType,
                    "1.0",
                    message);

                client.SendEventAsync(eventOne).GetAwaiter().GetResult();
                Console.WriteLine("Events published to the Event Grid Topic");
                Console.Write("Want to send more event?(y/n)");
                res = Console.ReadLine();
            } while (res.ToUpper() == "Y");
            Console.ReadLine();
        }
    }

    public class NewItemCreatedEvent
    {
        [JsonProperty(PropertyName = "name")]
        public string itemName;
    }
}