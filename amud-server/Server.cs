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
        public ConcurrentBag<Player> players = new ConcurrentBag<Player>();

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

            foreach (Player player in players)
            {
                player.sendToPlayer("bye " + player.name);
                player.disconnect();
            }
        }

        private void listenForClients() 
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                Player player = new Player(client, ref players);
                Thread clientThread = new Thread(new ParameterizedThreadStart(player.init));
                
                clientThread.Start();
                player.threadRef = clientThread;
                connections.Add(clientThread);
                players.Add(player);

                player.OnPlayerDisconnected += handleDisconnected;
            }
        }

        private void handleDisconnected(object sender, EventArgs e)
        {
            Player player = sender as Player;
            Thread outThread = player.threadRef;

            player.OnPlayerDisconnected -= handleDisconnected;

            while (!players.TryTake(out player)) ;
            while (!connections.TryTake(out outThread)) ;
        }
    }
}
