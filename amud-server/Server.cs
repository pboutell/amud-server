using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace amud_server
{
    class Server
    {
        public ConcurrentBag<Thread> connections = new ConcurrentBag<Thread>();
        public ConcurrentBag<Client> clients = new ConcurrentBag<Client>();

        private TcpListener tcpListener;
        private Thread listenThread;
        private World world;
        
        private Logger logger;

        public Server(MainWindow window)
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), 4000);
            this.logger = new Logger();
        }

        public void startServer()
        {
            listenThread = new Thread(new ThreadStart(listenForClients));
            listenThread.Start();
            logger.log("Server started on port 4000");
            
            world = new World();
        }

        public void shutdown()
        {
            logger.log("Server shutting down!");

            foreach (Client client in clients)
            {
                client.send("bye " + client.player.name);
                client.disconnect();
            }
        }

        private void listenForClients() 
        {
            tcpListener.Start();

            while (true)
            {
                TcpClient connection = tcpListener.AcceptTcpClient();
                Client client = new Client(connection, ref clients);
                Thread clientThread = new Thread(new ParameterizedThreadStart(client.init));
                IPEndPoint ep = connection.Client.RemoteEndPoint as IPEndPoint;

                if (ep.Address != null)
                    logger.log("new connection from: " + ep.Address);
                else
                    logger.log("new connection from: <unknown address>");

                clientThread.Start();
                client.threadRef = clientThread;
                connections.Add(clientThread);
                clients.Add(client);

                client.OnPlayerDisconnected += handleDisconnected;
            }
        }

        private void handleDisconnected(object sender, EventArgs e)
        {
            Client client = sender as Client;
            Thread outThread = client.threadRef;

            client.OnPlayerDisconnected -= handleDisconnected;

            while (!clients.TryTake(out client)) ;
            while (!connections.TryTake(out outThread)) ;

            outThread.Abort();
        }
    }
}
