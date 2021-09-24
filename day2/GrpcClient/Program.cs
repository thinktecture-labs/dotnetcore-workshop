using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press enter when ready.");
            Console.ReadLine();

            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                /*, new GrpcChannelOptions()
            {
                Credentials = ChannelCredentials.Create(
                    ChannelCredentials.SecureSsl,
                    CallCredentials.FromInterceptor((context, metadata) =>
                    {
                        metadata.Add("Authorization", "Basic R2VyaGFyZDpHZXJoYXJk");
                        return Task.CompletedTask;
                    }))
            });*/

            var client =  new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                new HelloRequest { Name = "GreeterClient" });

            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}