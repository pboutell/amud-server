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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace amud_server
{
    class Server
    {
        private ConcurrentBag<Thread> connections = new ConcurrentBag<Thread>();
        private ConcurrentBag<Client> clients = new ConcurrentBag<Client>();
        private FileInfo worldSave = new FileInfo("World.sav");
        private TcpListener tcpListener;
        private Thread listenThread;
        private World world;
        private System.Timers.Timer updateTimer;
        
        private bool isRunning = false;

        private Logger logger;

        public Server(MainWindow window)
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), 4000);
            this.logger = new Logger();
        }

        public void startServer()
        {
            isRunning = true;
            listenThread = new Thread(new ThreadStart(listenForClients));
            listenThread.Start();

            initWorld();
            startWorldTimer();
            logger.log("Server started on port 4000");
        }

        private void initWorld()
        {
            if (worldSave.Exists)
            {
                logger.log("loading saved world");
                deserialize();
            }
            else
            {
                logger.log("save file not found, loading new world");
                world = new World();
            }
        }

        private void startWorldTimer()
        {
            updateTimer = new System.Timers.Timer();

            updateTimer.Interval = 2000;
            updateTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            updateTimer.Enabled = true;
        }

        public void shutdown()
        {
            logger.log("Server shutting down!");

            kickAll();
            updateTimer.Enabled = false;
            isRunning = false;

            logger.log("Saving world..");
            serialize();

            Thread.Sleep(1000);
            logger.log("Server shutdown complete.");
        }

        private void kickAll()
        {
            foreach (Client client in clients)
            {
                if (client.isPlaying)
                {
                    client.send("bye " + client.player.name);
                }

                client.disconnect();
            }
        }

        private void listenForClients()
        {
            tcpListener.Start();

            while (isRunning)
            {
                createNewConnection();
            }
        }

        private void createNewConnection()
        {
            TcpClient connection = tcpListener.AcceptTcpClient();
            Client client = new Client(connection, clients, world);
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

            while (!connections.TryTake(out outThread)) ;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            world.worldTime = world.worldTime.AddMinutes(1);
            updatePlayers();
            updateMobs();
        }

        private void updatePlayers()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (Client c in clients)
            {
                if (c != null && c.isPlaying)
                {
                    c.player.update();
                    buffer.Append(world.getWeather(world.worldTime));

                    if (buffer.Length > 10)
                        c.player.client.send(buffer.ToString());
                }
            }
        }

        private void updateMobs()
        {
            Monitor.TryEnter(world.mobs);
            try
            {
                foreach (NPC m in world.mobs)
                {
                    if (m != null && m.room != null)
                    {
                        m.update(world.worldTime);
                    }
                }
            }
            finally
            {
                Monitor.Exit(world.mobs);
            }
        }

        public void serialize()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(worldSave.Name, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, world);
            stream.Close();
        }

        public void deserialize()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(worldSave.Name, FileMode.Open, FileAccess.Read, FileShare.Read);
            world = (World)formatter.Deserialize(stream);
            stream.Close();
        }
    }
}
