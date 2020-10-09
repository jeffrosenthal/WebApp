using System;
using System.Net;
using System.Xml.Linq;
using ServiceStack;
using Twilio;
using Twilio.Clients;
using Twilio.TwiML;

class Program 
{
    [Route("/hello/{Name}")]
    public class Hello
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }

    public class HelloService : Service
    {
        public object Any(Hello request)
        {
            //return new HelloResponse { Result = "Hello, " + request.Name };
            return new HelloResponse
            {
                Result = @"<?xml version=""1.0"" encoding=""UTF - 8""?>
                < Response >
                < Message >< Body > Hello World! </ Body ></ Message >
                </ Response > "
            };
        }

        public object Any(TwilioCallBack request)
        {
            var body = base.Request.FormData["Body"];
            var msgTo = base.Request.FormData["to"]; 
            var msgFrom = base.Request.FormData["from"];


            for (int i = 0; i < 19; i++)
            {
                Console.WriteLine($"{i} - {base.Request.FormData.Keys[i]}    {base.Request.FormData[i]} ");
            }

            var headerValue = base.Request.Headers;
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("The copy cat says: ");

            return TwiML(messagingResponse);
        }

    }

    [Route("/TwilioCallBack")]
    public class TwilioCallBack
    {

    }


    //Define the Web Services AppHost
    public class AppHost : AppSelfHostBase
    {
        public AppHost()
            : base("HttpListener Self-Host", typeof(HelloService).Assembly) { }

        public override void Configure(Funq.Container container) { }
    }

    //Run it!
    static void Main(string[] args)
    {
        var listeningOn = args.Length == 0 ? "http://*:1337/" : args[0];
        var appHost = new AppHost()
            .Init()
            .Start(listeningOn);

        

        Console.WriteLine("AppHost Created at {0}, listening on {1}",
            DateTime.Now, listeningOn);

        Console.ReadKey();
    }
}