namespace Server
{
    /// <summary>
    /// This interface defines methods for observer objects in the Observer design pattern
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// This method gets client changes 
        /// </summary>
        /// <param name="handler"></param>
        void ClientChange(IMessagingHandler handler);
    }
}
