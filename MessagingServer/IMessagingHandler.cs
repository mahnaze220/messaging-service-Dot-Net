namespace Server
{
	/// <summary>
	/// This interface has common methods for different types of messaging handlers
	/// </summary>
	public interface IMessagingHandler
    {
		/// <summary>
		/// Return sender client
		/// </summary>
		/// <returns></returns>
		public string GetSenderClient();

		/// <summary>
		/// Send message between client and server
		/// </summary>
		/// <param name="message"></param>
		public void SendMessage(string message);

		/// <summary>
		/// Return client's message
		/// </summary>
		/// <returns></returns>
		public string GetMessage();
	}
}
