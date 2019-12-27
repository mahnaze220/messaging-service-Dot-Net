namespace Server
{
    /// <summary>
    /// This interface defines methods for observable objects in the Observer design pattern
    /// </summary>
    interface ISubject
    {
        /// <summary>
        /// Add an observer to observer list of the observable
        /// </summary>
        /// <param name="observer">Observer</param>
        void Register(IObserver observer);

        /// <summary>
        /// Remove an observer from observer list of the observable
        /// </summary>
        /// <param name="observer">Observer</param>
        void Unregister(IObserver observer);

        /// <summary>
        /// Notify related observers by getting message from clients
        /// </summary>
        void Notify();
    }
}
