namespace Server
{
	/// <summary>
	/// This interface has common methods for different types of messaging strategies.
	/// </summary>
	interface IMessagingStrategy
    {
		/// <summary>
		/// Return connected server
		/// </summary>
		/// <returns></returns>
		public IMessageServer GetServer();

		/// <summary>
		/// Run the strategy 
		/// </summary>
		public void Run();
	}
}
