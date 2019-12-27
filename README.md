# messaging-service-Dot-Net
A simple client-server messaging service which runs in multi-process mode (by socket) implement by C#.
Observer design pattern is used to notify sever (as an observer) by sending message from 
a client client (as an observable) to broadcast messages.
And also Strategy design pattern is used to implements different strategies (single-process and multi-process).

In this scenario each client (MultiProcessClient class instance) and also server (GeneralMessageServer class instance) 
run on own process. 
Each client has own thread and creates a socket to connect to the server. 
MultiProcessMessagingStrategy is used an a messaging strategy and MultiProcessMessageHandler as a message handler
to connect clients to server and handle messages.
For each client a MultiProcessMessageHandler instance is created and it reads messages from the socket 
and send to the server.
