using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    /// <summary>
    /// This class implements multi-process messaging scenario for send messages between clients 
    /// run on own processes.It uses socket programming for connecting each client client to the server.
    /// </summary>
    public class MultiProcessMessagingStrategy : IMessagingStrategy
    {
        public static int MAX_CLIENT_NUMBERS = 2;
        private int port;
        private IMessageServer server;
        public Socket socket { get; private set; }

        /// <summary>
        /// Creates an instance by a server
        /// </summary>
        /// <param name="server">Server</param>
        /// <param name="port">Socket port</param>
        public MultiProcessMessagingStrategy(IMessageServer server, int port)
        {
            this.server = server;
            this.port = port;
        }

        /// <summary>
        /// Create socket server and message handler (for each connected client)
        /// </summary>
        public void Run()
        {
            // Establish the local endpoint for the socket
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            socket = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                socket.Bind(localEndPoint);
                socket.Listen(MAX_CLIENT_NUMBERS);
                
                int clients = MAX_CLIENT_NUMBERS;
                // Accept clients until limit number is met
                while (clients > 0)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Creates a handler for each client
                    var multiProcessMessageHandler = new MultiProcessMessageHandler(this);
                    Thread thread = new Thread(new ThreadStart(multiProcessMessageHandler.HandleMessage));
                    thread.Start();
                    clients--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public IMessageServer GetServer() => server;
    }
}