namespace Server
{
	/// <summary>
	/// This class holds a reference to a messaging strategy object and delegates it executing the behavior 
	/// in Strategy design pattern.
	/// </summary>
	class MessagingContext
    {
		private IMessagingStrategy strategy;

		/// <summary>
		/// Set the messaging strategy at runtime by the application preferences
		/// </summary>
		/// <param name="strategy"></param>
		public void SetStrategy(IMessagingStrategy strategy)
		{
			this.strategy = strategy;
		}

		/// <summary>
		/// Run the selected strategy
		/// </summary>
		public void Run()
		{
			strategy.Run();
		}
	}
}
