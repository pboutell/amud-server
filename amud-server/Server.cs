using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

namespace amud_server
{
    class Server
    {
        private ConcurrentBag<Thread> connections = new ConcurrentBag<Thread>();
        private ConcurrentBag<Client> clients = new ConcurrentBag<Client>();

        private TcpListener tcpListener;
        private Thread listenThread;
        private World world;
        private System.Timers.Timer updateTimer;
        private DateTime worldTime = new DateTime();
        
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

            updateTimer = new System.Timers.Timer();

            updateTimer.Interval = 1000;
            updateTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            updateTimer.Enabled = true;
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

            Thread.Sleep(1000);
        }

        private void listenForClients() 
        {
            tcpListener.Start();

            while (true)
            {
                createNewConnection();
            }
        }

        private void createNewConnection()
        {
            TcpClient connection = tcpListener.AcceptTcpClient();
            Client client = new Client(connection, clients);
            Thread clientThread = new Thread(new ParameterizedThreadStart(client.init));

            logIP(connection);
            clientThread.Start();
            client.threadRef = clientThread;
            connections.Add(clientThread);
            clients.Add(client);

            client.OnPlayerDisconnected += handleDisconnected;
        }

        private void logIP(TcpClient connection)
        {
            IPEndPoint ep = connection.Client.RemoteEndPoint as IPEndPoint;

            if (ep.Address != null)
                logger.log("new connection from: " + ep.Address);
            else
                logger.log("new connection from: <unknown address>");
        }

        private void handleDisconnected(object sender, EventArgs e)
        {
            Client client = sender as Client;
            Thread outThread = client.threadRef;

            client.OnPlayerDisconnected -= handleDisconnected;

            //while (!clients.TryTake(out client)) ;
            while (!connections.TryTake(out outThread)) ;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            worldTime = worldTime.AddMinutes(1);
        
            foreach (Client c in clients)
            {
                if (c != null && c.playing)
                {
                    c.player.update();
                    buffer.Append(world.getWeather(worldTime));

                    if (buffer.Length > 2)
                        c.player.client.send(buffer.ToString());
                }
            }

            foreach (NPC m in World.mobs)
            {
                if (m != null)
                {
                    m.update();
                }
            }
        }
    }
}
