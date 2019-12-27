using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    /// <summary>
    /// This message handler is created for each client in multi-process scenario. 
    /// This class plays role of mediator between the server and client and handle messages.
    /// It uses observer design pattern to notify server to broadcast messages.
    /// This handler uses single-process strategy for sending messages.
    /// </summary>
    class MultiProcessMessageHandler : IMessagingHandler, ISubject
    {
        private string clientMessage;
        private string senderClient;        
        private MultiProcessMessagingStrategy strategy;        
        private Socket socket;
        private HashSet<IObserver> observers = new HashSet<IObserver>();

        /// <summary>
        /// Create message handler by related messaging strategy
        /// </summary>
        /// <param name="strategy"></param>
        public MultiProcessMessageHandler(MultiProcessMessagingStrategy strategy)
        {
            this.strategy = strategy;

            // Add his hander to the server list
            strategy.GetServer().AddMessageHandler(this);

            // Add the server object as an observer entity 
            Register(strategy.GetServer());

            // Start listening for connections
            socket = strategy.socket.Accept();

            // Get sender client form socket
            byte[] msgReceived = new byte[1024];
            int bRecv = socket.Receive(msgReceived);
            this.senderClient = Encoding.ASCII.GetString(msgReceived, 0, bRecv);
        }

        /// <summary>
        /// Handle sending messages between client and server
        /// </summary>
        public void HandleMessage()
        {            
            Boolean shouldStop = false;            

            // Read client message from the socket and send it until the number of initiator's sent messages
            // meet limit                          
            do
            {                
                shouldStop = strategy.GetServer().CanStopProcess(senderClient);                
                byte[] messageReceived = new byte[1024];
                int byteRecv = socket.Receive(messageReceived);
                clientMessage = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

                // Notify the server to broadcast messages to the client                
                Notify();
            }
            while (!shouldStop);

            //Stop sever 
            socket.Close();
        }

        /// <summary>
        /// Get client' message
        /// </summary>
        /// <returns>Client's message</returns>
        public string GetMessage()
        {
            return clientMessage;
        }

        /// <summary>
        /// Get sender client
        /// </summary>
        /// <returns>Sender client name</returns>
        public string GetSenderClient()
        {
            return senderClient;
        }

        /// <summary>
        /// Send message to client
        /// </summary>
        /// <param name="message">Client's message</param>
        public void SendMessage(string message)
        {
            byte[] msg = Encoding.ASCII.GetBytes(message);
            this.socket.Send(msg);            
        }

        /// <summary>
        /// Add server to observers list
        /// </summary>
        /// <param name="observer"></param>
        public void Register(IObserver observer)
        {
            observers.Add(observer);
        }

        /// <summary>
        /// Remove the server from observers
        /// </summary>
        /// <param name="observer"></param>
        public void Unregister(IObserver observer)
        {
            observers.Remove(observer);
        }

        /// <summary>
        /// Notify the server to broadcast messages to the client
        /// </summary>
        public void Notify()
        {
            observers.ToList().ForEach(o => o.ClientChange(this));
        }
    }
}