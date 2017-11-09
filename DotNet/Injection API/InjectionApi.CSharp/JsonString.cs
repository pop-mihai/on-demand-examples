﻿using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using InjectionApi.Sdk.Client;
using RestSharp;

namespace SocketLabs.InjectionApi.CSharp
{
    static public partial class Samples
    {
        /// <summary>
        /// This example uses the raw JSON string to create a POST
        /// request for sending one or more SMTP messages through SocketLabs On-Demand.
        /// 
        /// The JSON request generated by this sample code looks like this:
        /// 
        ///{
        ///    "ServerId": "YOUR SERVER ID HERE",
        ///    "ApiKey": "YOUR API KEY HERE",
        ///    "Messages": [{
        ///        "Subject": "Email subject line for raw JSON example.",
        ///        "TextBody": "The text portion of the message.",
        ///        "HtmlBody": "<h1>The html portion of the message</h1><br/>Test",
        ///        "To": [{
        ///            "EmailAddress": "to@example.com",
        ///            "FriendlyName": "Customer Name"
        ///        }],
        ///        "From": {
        ///            "EmailAddress": "from@example.com",
        ///            "FriendlyName": "From Address"
        ///        },
        ///    }]
        ///}
        /// </summary>
        public static void SimpleInjectionViaStringAsJson(
            int serverId, string yourApiKey, string apiUrl)
        {
            // The client object processes requests to the SocketLabs Injection API.
            var client = new RestClient(apiUrl);

            // Construct the string used to generate JSON for the POST request.
            string stringBody =
                "{" +
                    "\"ServerId\":\"" + serverId + "\"," +
                    "\"ApiKey\":\"" + yourApiKey + "\"," +
                    "\"Messages\":[{" +
                        "\"Subject\":\"Email subject line for raw JSON example.\"," +
                        "\"TextBody\":\"The text portion of the message.\"," +
                        "\"HtmlBody\":\"<h1>The html portion of the message</h1><br/>Test\"," +
                        "\"To\":[{" +
                            "\"EmailAddress\":\"to@example.com\"," +
                            "\"FriendlyName\":\"Customer Name\"" +
                        "}]," +
                        "\"From\":{" +
                            "\"EmailAddress\":\"from@example.com\"," +
                            "\"FriendlyName\":\"From Address\"" +
                        "}," +
                    "}]" +
                "}";

            try
            {
                // Generate a new POST request.
                var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };

                // Store the request data in the request object.
                request.AddParameter("application/json; charset=utf-8", stringBody, ParameterType.RequestBody);

                // Make the POST request.
                var result = client.ExecuteAsPost(request, "POST");

                // Store the response result in our custom class.
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(result.Content)))
                {
                    var serializer = new DataContractJsonSerializer(typeof (PostResponse));
                    var resp = (PostResponse) serializer.ReadObject(stream);

                    // Display the results.
                    if (resp.ErrorCode.Equals("Success"))
                    {
                        Console.WriteLine("Successful injection!");
                    }
                    else
                    {
                        Console.WriteLine("Failed injection! Returned JSON is: ");
                        Console.WriteLine(result.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, something bad happened: " + ex.Message);
            }
        }
    }
}
