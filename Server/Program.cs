using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Common;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "net.pipe://localhost/Service";
            ServiceHost host = new ServiceHost(typeof(DynamicService), new Uri(baseAddress));
            ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IDynamicContract), new NetNamedPipeBinding { OpenTimeout = TimeSpan.FromSeconds(10), MaxBufferSize = 2147483647, MaxReceivedMessageSize = 2147483647 }, "");
            foreach (OperationDescription operation in endpoint.Contract.Operations)
            {
                operation.Behaviors.Find<DataContractSerializerOperationBehavior>().DataContractResolver = new ReflectionDataContractResolver();
            }

            host.Open();
            Console.WriteLine("Host opened, press ENTER to close");
            Console.ReadLine();
            host.Close();
        }
    }
}
