using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MessagingClient
{

    /// <summary>
    /// This class creates instances for clients which run on own process.
    /// Each instance sends message to the server by socket on a specific host and port.
    /// </summary>
    public class MultiProcessClient : IClient
    {
        private readonly int port;
        public string username;
        private Socket socket;

        /// <summary>
        /// Create client instance 
        /// </summary>
        /// <param name="port">Socket port</param>
        /// <param name="username">Client name</param>
        public MultiProcessClient(int port, string username)
        {
            this.port = port;
            this.username = username;
        }

        /// <summary>
        /// This method runs each client on own process and send the message by a socket to the server
        /// </summary>
        public void Run()
        {
            // Establish the remote endpoint for the socket which uses a port on the local machine 
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            // Creation TCP/IP Socket using Socket Class Costructor 
            socket = new Socket(ipAddr.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

            // Connect Socket to the remote endpoint
            socket.Connect(localEndPoint);

            Console.WriteLine("Socket connected to -> {0} ",
                          socket.RemoteEndPoint.ToString());

            byte[] messageSent = Encoding.ASCII.GetBytes(username);
            int byteSent = socket.Send(messageSent);

            // Send message to the server
            WriteMessageThread writeThread = new WriteMessageThread(socket, username);
            Thread thread = new Thread(new ThreadStart(writeThread.Run));
            thread.Start();

            // Receive message from the server
            ReadMessageThread readThread = new ReadMessageThread(socket, username);
            Thread rthread = new Thread(new ThreadStart(readThread.Run));
            rthread.Start();
        }

        /// <summary>
        /// Get the message from the messaing server and display on the client's console
        /// </summary>
        /// <param name="message"></param>
        public void GetMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Get client's username
        /// </summary>
        /// <returns>Client name</returns>
        public string GetUsername()
        {
            return this.username;
        }

        /// <summary>
        /// Set client's username
        /// </summary>
        /// <param name="name">Client name</param>
        public void SetUsername(string name)
        {
            username = name;
        }

        /// <summary>
        /// This class reads message from the socket and send it to the client.
		/// It runs until the client client disconnects from the server.
        /// </summary>
        private class ReadMessageThread
        {
            private Socket socket;
            private string clientName;

            /// <summary>
            /// Creates an instance of ReadMessageThread for each client
            /// </summary>
            /// <param name="socket">Socket</param>
            /// <param name="clientName">Client name</param>
            public ReadMessageThread(Socket socket, string clientName)
            {
                this.socket = socket;
                this.clientName = clientName;
            }

            /// <summary>
            /// Read message from socket and send it to client
            /// </summary>
            public void Run()
            {
                while (true)
                {
                    byte[] bytes = new Byte[1024];
                    string response = null;
                    int numByte = socket.Receive(bytes);
                    response += Encoding.ASCII.GetString(bytes, 0, numByte);

                    if (response != null)
                    {
                        string[] result = response.Split("/");

                        // Each message contains the receiver name, so each client gets own message
                        if (result[0].Equals(clientName))
                        {
                            Console.WriteLine("\n" + result[1]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This inner class reads user input and send it to the server to broadcast it to another clients.
        /// It uses socket to write messages and send to the server. It stops when after closing the socket.
        /// </summary>
        private class WriteMessageThread
        {
            private readonly Socket socket;
            private readonly string clientName;

            /// <summary>
            /// Creates an instance of WriteMessageThread for each client
            /// </summary>
            /// <param name="socket">Socket</param>
            /// <param name="clientName">Client name</param>
            public WriteMessageThread(Socket socket, string clientName)
            {
                this.socket = socket;
                this.clientName = clientName;
            }

            /// <summary>
            /// Get messages from client console and write on the socket
            /// </summary>
            public void Run()
            {
                while (true)
                {
                    string message = Console.ReadLine().ToString();
                    byte[] messageSent = Encoding.ASCII.GetBytes(message);
                    socket.Send(messageSent);
                }
            }
        }

        /// <summary>
        /// Main method to create client instance on a new process
        /// </summary>
        /// <param name="args">Client name</param>
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }
            string clientName = args[0];

            MultiProcessClient client = new MultiProcessClient(11000, clientName);
            Thread rthread = new Thread(new ThreadStart(client.Run));
            rthread.Start();
        }
    }
}