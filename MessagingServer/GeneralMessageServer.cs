using System;
using System.Text;
using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// This class is a messaging server to broadcast messages between clients. 
    /// It holds list of client's message handlers to keep number of send messages by each client.
    /// It uses observer design pattern to receive clients' messages.
    /// </summary>
    
    class GeneralMessageServer : IMessageServer
    {
        // maximum number of messages each client can send 
        public static int MESSAGE_SENDING_LIMIT = 10;

        // This dictionary hold number of sent messages of each client 
        private Dictionary<string, int> clientSentMessages = new Dictionary<string, int>();

        // This list holds all connected client's message handlers used for broadcasting
        private List<IMessagingHandler> messageHandlers = new List<IMessagingHandler>();

        /// <summary>
        /// Return number of connected clients
        /// </summary>
        /// <returns></returns>
        public int GetClientsCount()
        {
            return messageHandlers.Capacity;
        }

        /// <summary>
        /// Stop the server by closing the socket
        /// </summary>
        /// <param name="handler">Messaging handler</param>
        public void StopServer(IMessagingHandler handler)
        {
            Console.WriteLine(handler.GetSenderClient() + " left");
            messageHandlers.Remove(handler);
        }

        /// <summary>
        /// Checks the number of message has sent by the each client, if the number of messages meets specific limit, 
	    /// program will be stopped.
        /// </summary>
        /// <param name="clientName">Client name</param>
        /// <returns>boolean</returns>
        public bool CanStopProcess(string clientName)
        {
            if (GetUserNames().ContainsKey(clientName) && GetUserNames()[clientName] > MESSAGE_SENDING_LIMIT)
            {
                Console.WriteLine("Program finished");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sends message to all clients except the sender client and also send the message back to the sender client.
        /// </summary>
        /// <param name="message">Client's message</param>
        /// <param name="handler">Message handler</param>
        public void BroadcastMessage(string message, IMessagingHandler handler)
        {
           
		    // Check number of sent messages by sender client		 
            string senderClient = handler.GetSenderClient();
            Boolean canStop = CanStopProcess(senderClient);

            if (!canStop)
            {
                foreach (IMessagingHandler client in GetMessageHandlers())
                {
                    if (!client.GetSenderClient().Equals(senderClient))
                    {

                        // Send message to receiver client
                        string msg = new StringBuilder(client.GetSenderClient()).Append("/")
                                .Append(senderClient).Append(": ").Append(message).ToString();
                        client.SendMessage(msg);

                        Console.WriteLine(senderClient + ": " + message);

                        // Create message back text and send from receiver client to sender client
                        int count = GetClientMessageCount(senderClient);

                        string messageBack = message + count;
                        Console.WriteLine(client.GetSenderClient() + ": " + messageBack);
                        messageBack = new StringBuilder(senderClient).Append("/")
                                .Append(client.GetSenderClient()).Append(": ")
                                .Append(message).Append(count).ToString();
                        handler.SendMessage(messageBack);
                        
                        // Update number of sent messages by each client                         
                        UpdateClientMessageCount(client.GetSenderClient());
                        UpdateClientMessageCount(senderClient);
                    }
                }
            }
        }

        /// <summary>
        /// Update number of messages sent by the client
        /// </summary>
        /// <param name="clientName">Client name</param>
        public void UpdateClientMessageCount(string clientName)
        {
            if (clientSentMessages.ContainsKey(clientName))
            {
                clientSentMessages[clientName] = clientSentMessages[clientName] + 1;                
            }
            else
            {
                clientSentMessages.Add(clientName, 1);
            }
        }

        /// <summary>
        /// Get the number of sent messages by the client
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        public int GetClientMessageCount(string clientName)
        {
            if (clientSentMessages.ContainsKey(clientName))
            {
                return clientSentMessages[clientName];
            }
            return 0;
        }

        /// <summary>
        /// Return List of clients by number of their sent messages
        /// </summary>
        /// <returns></returns>        
        public Dictionary<string, int> GetUserNames()
        {
            return clientSentMessages;
        }

        /// <summary>
        /// Return list of message handlers of connected clients 
        /// </summary>
        /// <returns></returns>        
        public List<IMessagingHandler> GetMessageHandlers()
        {
            return messageHandlers;
        }

        /// <summary>
        /// When a client connected to server, its message handler is added to server list
        /// </summary>
        /// <param name="messageHandler"></param>
        public void AddMessageHandler(IMessagingHandler messageHandler)
        {
            messageHandlers.Add(messageHandler);
        }

        /// <summary>
        /// When the client send the message, server is notified by the message handler to broadcast the message.
        /// </summary>
        /// <param name="handler"></param>
        public void ClientChange(IMessagingHandler handler)
        {
            BroadcastMessage(handler.GetMessage(), handler);
        }
    }
}