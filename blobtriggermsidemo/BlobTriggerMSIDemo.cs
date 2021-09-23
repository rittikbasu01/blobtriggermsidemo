using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace blobtriggermsidemo
{
    public static class BlobTriggerMSIDemo
    {
        [FunctionName("BlobTriggerMSIDemo")]
        public static async Task Run([BlobTrigger("samplecontainer/{name}", Connection = "blobconnectionstring")] Stream myBlob, string name, ILogger log)
        {
            //Name of the endpoint of Event grid topic
            string topicEndpoint = " https://demomsiegtopic.eastus-1.eventgrid.azure.net/api/events";
           
            //Creating client to publish events to eventgrid topic
            EventGridPublisherClient client = new EventGridPublisherClient(new Uri(topicEndpoint), new DefaultAzureCredential());

            log.LogInformation($"received client");

            //Creating a sample event with Subject, Eventtype, dataVersion and data
            EventGridEvent egEvent = new EventGridEvent("Subject",
                                                       "demoegmsifunc.receive",
                                                        "1.0",
                                                         "Sample event data");

            // Send the event
            await client.SendEventAsync(egEvent);
            
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }

}
