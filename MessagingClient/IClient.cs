namespace MessagingClient
{
	/// <summary>
	/// Each client has a username and can connect to the message server and send message to another client.
	/// </summary>

	public interface IClient
	{
        
		/// <summary>
		/// Return username of client	 
		/// </summary>
		/// <returns>Client's username</returns>
		public string GetUsername();

		/// <summary>
		/// Set username of client
		/// </summary>
		/// <param name="username">Client's username</param>
		public void SetUsername(string username);

		/// <summary>
		/// Get message from messaging server
		/// </summary>
		/// <param name="message">Sent message from server</param>
		public void GetMessage(string message);
    }
}
