namespace Server
{
	/// <summary>
	/// This interface is used for creating different types of message servers.
	/// </summary>
	public interface IMessageServer : IObserver
	{
		/// <summary>
		/// Stop server after closing a connection and remove message handler from the connected clients list
		/// </summary>
		/// <param name="handler">Message handler</param>
		public void StopServer(IMessagingHandler handler);

		/// <summary>
		/// Add message handler of connected client to server's list
		/// </summary>
		/// <param name="handler">Message handler</param>
		public void AddMessageHandler(IMessagingHandler handler);

		/// <summary>
		/// Check the criteria for stopping sending message for a client
		/// </summary>
		/// <param name="clientName">Client name</param>
		/// <returns>boolean</returns>
		public bool CanStopProcess(string clientName);

		/// <summary>
		/// Broadcast message from sender to all connected clients
		/// </summary>
		/// <param name="message">Client' message</param>
		/// <param name="handler">Message handler</param>
		public void BroadcastMessage(string message, IMessagingHandler handler);

		/// <summary>
		/// Update number of send messages by the client
		/// </summary>
		/// <param name="clientName"></param>
		public void UpdateClientMessageCount(string clientName);

		/// <summary>
		/// Retun number of sent messages by the client
		/// </summary>
		/// <param name="clientName"></param>
		/// <returns></returns>
		public int GetClientMessageCount(string clientName);

		/// <summary>
		/// Return number of connected clients
		/// </summary>
		/// <returns></returns>
		public int GetClientsCount();		
	}
}
