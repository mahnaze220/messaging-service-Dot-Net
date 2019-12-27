using System;

namespace Server
{
	class MultiProcessMessagingDemo
    {
		static void Main(string[] args)
		{
			MultiProcessMessagingStrategy handler = new MultiProcessMessagingStrategy(new GeneralMessageServer(), 11000);
			MessagingContext messagingContext = new MessagingContext();
			
			// Set multi-process messaging strategy
			messagingContext.SetStrategy(handler);
			messagingContext.Run();

			Console.WriteLine("Sever is listening ...");
		}
	}
}
