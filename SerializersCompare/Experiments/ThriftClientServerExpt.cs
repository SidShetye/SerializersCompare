using System;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace SerializersCompare.Experiments
{
    public class ThriftClientServerExpt
    {
        public static string Host = "localhost";
        public static int Port = 9090;

        public void RunExpt1()
        {
            var server = new ThriftExptServer();
            // Spawn the server in the background as a Task 
            // (abstraction over threads, see http://msdn.microsoft.com/EN-US/library/dd235678%28v=VS.110,d=hv.2%29.aspx)
            // note that this experiment is multi-threaded, but the server itself is NOT (future consideration)
            var serverTask = new Task(server.Begin);
            serverTask.Start();

            // Run a client(s) 
            var client = new ThriftExptClient();
            // We run till any result is 0 (an arbitrary choice for this experiment)
            while (client.Run() !=0)
            {
            }

            // Close the server to free up the port
            server.End();
        }

        /// <summary>
        /// Thrift is an RPC system, so builds beyond just serialization.
        /// The layer above serialization is the RPC, so this is the 
        /// concrete implementation of the RPC interface (remember, thrift 
        /// only defines the RPC interface and data IO across it)
        /// </summary>
        public class MultiplicationHandler : MultiplicationService.Iface
        {
            public int multiply(int n1, int n2)
            {
                Console.WriteLine("Multiply(" + n1 + "," + n2 + ")");
                return n1 * n2;
            }
        }

        public class ThriftExptServer
        {
            private TServer _server;

            public void Begin()
            {
                try
                {
                    var handler = new MultiplicationHandler();
                    var processor = new MultiplicationService.Processor(handler);
                    TServerTransport serverTransport = new TServerSocket(Port);
                    _server = new TSimpleServer(processor, serverTransport);
                    Console.WriteLine("Starting the server...");
                    
                    // The next call will ...
                    // ... create a new thrift server on a new thread (Thrift design)
                    // ... that thread runs till server.Stop() is called
                    // ... this thread is blocked till then                    
                    _server.Serve();                     
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("Server, done.");
            }

            public void End()
            {
                if (_server != null)
                {
                    Console.WriteLine("Server is shutting down normally.");
                    _server.Stop();
                }
            }
        }

        public class ThriftExptClient
        {
            public int Run()
            {
                int c = 0;

                try
                {
                    TTransport transport = new TSocket(Host, Port);
                    TProtocol protocol = new TBinaryProtocol(transport);
                    var client = new MultiplicationService.Client(protocol);

                    Console.WriteLine("Thrift client opening transport to {0} on port {1} ...", Host, Port);
                    transport.Open();


                    int a, b;
                    Console.Write("Enter 1st integer : ");
                    int.TryParse(Console.ReadLine(), out a);
                    Console.Write("Enter 2nd integer : ");
                    int.TryParse(Console.ReadLine(), out b);

                    c = client.multiply(a, b);

                    Console.WriteLine("{0} x {1} = {2}", a, b, c);
                    Console.WriteLine("Thrift client closing transport ...");
                    transport.Close();

                }
                catch (TApplicationException x)
                {
                    Console.WriteLine(x.StackTrace);

                }

                return c;
            }
        }
    }
}
