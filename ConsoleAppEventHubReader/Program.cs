using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppEventHubReader
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var connectionString = "<connectionstring>";
            var eventHubName = "<eventhub name>";
            var res = "y";
            do
            {
                var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

                var consumer = new EventHubConsumerClient(
                    consumerGroup,
                    connectionString,
                    eventHubName);

                try
                {
                    using CancellationTokenSource cancellationSource = new CancellationTokenSource();
                    cancellationSource.CancelAfter(TimeSpan.FromSeconds(45));

                    int eventsRead = 0;
                    int maximumEvents = 100;

                    await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(cancellationSource.Token))
                    {
                        string readFromPartition = partitionEvent.Partition.PartitionId;
                        byte[] eventBodyBytes = partitionEvent.Data.EventBody.ToArray();

                        Console.WriteLine($"Read event of length { eventBodyBytes.Length } from { readFromPartition }, Data {System.Text.Encoding.ASCII.GetString(eventBodyBytes)}");
                        eventsRead++;

                        if (eventsRead >= maximumEvents)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // This is expected if the cancellation token is
                    // signaled.
                }
                finally
                {
                    await consumer.CloseAsync();
                }
                Console.Write("Want to send more event?(y/n)");
                res = Console.ReadLine();
            } while (res.ToUpper() == "Y");
        }
    }
}