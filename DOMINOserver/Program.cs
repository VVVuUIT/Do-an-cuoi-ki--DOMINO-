using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace DOMINOserver
{
    class Program
    {
        private static Socket serverSocket;
        private static Socket clientSocket;
        private static Thread clientThread;
        private static List<Player> connectedClient = new List<Player>();
        private static int thisTurn = 1;
        private static bool clockwise = true;
        public static void encodingClientSocket(Socket client)
        {
            Player player = new Player();
            player.playerSocket = client;
            connectedClient.Add(player);
            byte[] buffer = new byte[1024];
            while (player.playerSocket.Connected)
            {
                if (player.playerSocket.Available > 0)
                {
                    string msg = "";
                    while (player.playerSocket.Available > 0)
                    {
                        msg += Encoding.UTF8.GetString(buffer, 0, player.playerSocket.Receive(buffer));
                    }
                    Console.WriteLine(player.playerSocket.RemoteEndPoint + ": " + msg);
                    AnalyzingMessage(msg, player);
                }
            }
        }
        public static void AnalyzingMessage(string msg, Player p)
        {
            string[] arrPayload = msg.Split('-');
            switch (arrPayload[0])
            {
                case "CONNECT":
                    {
                        p.name = arrPayload[1];
                        foreach (var player in connectedClient)
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes("LOBBY-" + player.name);
                            p.playerSocket.Send(buffer);
                            Thread.Sleep(100);
                        }
                        foreach (var player in connectedClient)
                        {
                            if (player.playerSocket != p.playerSocket)
                            {
                                byte[] buffer = Encoding.UTF8.GetBytes("LOBBY-" + p.name);
                                player.playerSocket.Send(buffer);
                                Thread.Sleep(100);
                            }
                        }
                    }
                    break;
                case "DISCONNECT":
                    {
                        foreach (var player in connectedClient.ToList())
                        {
                            if (player.name == arrPayload[1])
                            {
                                player.playerSocket.Shutdown(SocketShutdown.Both);
                                player.playerSocket.Close();
                                connectedClient.Remove(player);
                            }
                        }
                    }
                    break;
                case "BEGIN":
                    {
                        RandomizePlayerTurn();
                        connectedClient.Sort((x, y) => x.turn.CompareTo(y.turn));
                        PileSuffle();
                        DrawPile.faceupcard = FaceUpCard();
                        foreach (var player in connectedClient)
                        {
                            string makemsg = "INIT-" + player.name + "-" + player.turn + "-" + player.numOfCards + "-" + InitialCardsDeal() + DrawPile.faceupcard;
                            byte[] buffer = Encoding.UTF8.GetBytes(makemsg);
                            player.playerSocket.Send(buffer);
                            Console.WriteLine("Sendback: " + makemsg);
                            Thread.Sleep(100);
                        }
                        foreach (var player in connectedClient)
                        {
                            foreach (var player_ in connectedClient)
                            {
                                if (player.name != player_.name)
                                {
                                    string makemsg = "OTHER-" + player_.name + "-" + player_.turn + "-" + player_.numOfCards;
                                    byte[] buffer = Encoding.UTF8.GetBytes(makemsg);
                                    player.playerSocket.Send(buffer);
                                    Console.WriteLine("Sendback: " + makemsg);
                                    Thread.Sleep(100);
                                }
                            }
                        }
                        foreach (var player in connectedClient)
                        {
                            string makemsg = "SETUP-" + player.name;
                            byte[] buffer = Encoding.UTF8.GetBytes(makemsg);
                            player.playerSocket.Send(buffer);
                            Console.WriteLine("Sendback: " + makemsg);
                            Thread.Sleep(100);
                        }
                        foreach (var player in connectedClient)
                        {
                            string makemsg_ = "TURN-" + connectedClient[thisTurn - 1].name;
                            byte[] buffer_ = Encoding.UTF8.GetBytes(makemsg_);
                            player.playerSocket.Send(buffer_);
                            Console.WriteLine("Sendback: " + makemsg_);
                            Thread.Sleep(100);
                        }
                    }
                    break;
                case "PLAY":
                    {
                        DrawPile.faceupcard = arrPayload[3];
                        DiscardPile.disPile.Add(arrPayload[3]);
                        connectedClient[thisTurn - 1].numOfCards = int.Parse(arrPayload[2]);
                        if (connectedClient[thisTurn - 1].numOfCards == 0)
                        {
                            foreach (var player in connectedClient)
                            {
                                string makemsg = "END-" + arrPayload[1] + "-" + arrPayload[2] + "-" + arrPayload[3];
                                byte[] buffer = Encoding.UTF8.GetBytes(makemsg);
                                player.playerSocket.Send(buffer);
                                Console.WriteLine("Sendback: " + makemsg);
                                Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            foreach (var player in connectedClient)
                            {
                                if (player.turn != thisTurn)
                                {
                                    string makemsg = "UPDATE-" + arrPayload[1] + "-" + arrPayload[2] + "-" + arrPayload[3];                                    
                                    byte[] buffer = Encoding.UTF8.GetBytes(makemsg);
                                    player.playerSocket.Send(buffer);
                                    Console.WriteLine("Sendback: " + makemsg);
                                    Thread.Sleep(100);
                                }
                            }

                            if (clockwise == true)
                            {
                                thisTurn++;
                            }
                            else
                            {
                                thisTurn--;
                            }

                            if (thisTurn > connectedClient.Count)
                                thisTurn = 1;

                            if (thisTurn < 1)
                                thisTurn = connectedClient.Count;

                            foreach (var player in connectedClient)
                            {
                                string makemsg_ = "TURN-" + connectedClient[thisTurn - 1].name;
                                byte[] buffer_ = Encoding.UTF8.GetBytes(makemsg_);
                                player.playerSocket.Send(buffer_);
                                Console.WriteLine("Sendback: " + makemsg_);
                                Thread.Sleep(100);
                            }
                        }
                    }
                    break;
                case "PICK":
                    {
                        connectedClient[thisTurn - 1].numOfCards = int.Parse(arrPayload[2]);
                        string mkmsg = "PICKCARD-" + arrPayload[1] + "-" + DrawPile.card_id[0];
                        DrawPile.card_id = DrawPile.card_id.Where(val => val != DrawPile.card_id[0]).ToArray();
                        byte[] bf = Encoding.UTF8.GetBytes(mkmsg);
                        connectedClient[thisTurn - 1].playerSocket.Send(bf);

                        foreach (var player in connectedClient)
                        {
                            if (player.turn != thisTurn)
                            {
                                string makemsg = "UPDATE-" + arrPayload[1] + "-" + arrPayload[2];
                                byte[] buffer = Encoding.UTF8.GetBytes(makemsg);
                                player.playerSocket.Send(buffer);
                                Console.WriteLine("Sendback: " + makemsg);
                                Thread.Sleep(100);
                            }
                        }

                        if (clockwise == true)
                        {
                            thisTurn++;
                        }
                        else
                        {
                            thisTurn--;
                        }

                        if (thisTurn > connectedClient.Count)
                            thisTurn = 1;

                        if (thisTurn < 1)
                            thisTurn = connectedClient.Count;

                        foreach (var player in connectedClient)
                        {
                            string makemsg_ = "TURN-" + connectedClient[thisTurn - 1].name;
                            byte[] buffer_ = Encoding.UTF8.GetBytes(makemsg_);
                            player.playerSocket.Send(buffer_);
                            Console.WriteLine("Sendback: " + makemsg_);
                            Thread.Sleep(100);
                        }
                    }
                    break;
                case "SKIP":
                    {
                        if (clockwise == true)
                        {
                            thisTurn++;
                        }
                        else
                        {
                            thisTurn--;
                        }

                        if (thisTurn > connectedClient.Count)
                            thisTurn = 1;

                        if (thisTurn < 1)
                            thisTurn = connectedClient.Count;

                        foreach (var player in connectedClient)
                        {
                            string makemsg_ = "TURN-" + connectedClient[thisTurn - 1].name;
                            byte[] buffer_ = Encoding.UTF8.GetBytes(makemsg_);
                            player.playerSocket.Send(buffer_);
                            Console.WriteLine("Sendback: " + makemsg_);
                            Thread.Sleep(100);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            serverSocket.Bind(serverEP);
            serverSocket.Listen(4);
            Console.WriteLine("[ Waiting for connection from players ... ]");

            while (true)
            {
                clientSocket = serverSocket.Accept();
                Console.WriteLine(">> Connection from " + clientSocket.RemoteEndPoint);
                clientThread = new Thread(() => encodingClientSocket(clientSocket));
                clientThread.Start();
            }
        }
        public static void RandomizePlayerTurn()
        {
            int[] turns = new int[connectedClient.Count];

            for (int i = 1; i <= connectedClient.Count; i++)
            {
                turns[i - 1] = i;
            }

            Random rand = new Random();
            foreach (var player in connectedClient)
            {
                int pick = rand.Next(turns.Length);
                player.turn = turns[pick];
                turns = turns.Where(val => val != turns[pick]).ToArray();
                player.numOfCards = 7;
            }
        }
        public static void PileSuffle()
        {
            Random rand = new Random();
            DrawPile.card_id = DrawPile.card_id.OrderBy(x => rand.Next()).ToArray();
        }
        public static string InitialCardsDeal()
        {
            Random rand = new Random();
            string sevencards = "";
            for (int i = 0; i < 7; i++)
            {
                int pick = rand.Next(DrawPile.card_id.Length);
                sevencards += DrawPile.card_id[pick] + "-";
                DrawPile.card_id = DrawPile.card_id.Where(val => val != DrawPile.card_id[pick]).ToArray();
            }
            return sevencards;
        }
        public static string FaceUpCard()
        {
            string fCard = "v_s";
            DrawPile.card_id = DrawPile.card_id.Where(val => val != "v_s").ToArray();
            DiscardPile.disPile.Add(fCard);
            return fCard;
        }
        public static bool CheckPileEmpty()
        {
            if (DrawPile.card_id.Length == 0)
                return true;
            return false;
        }
        public static void BroadcastBack(string type, string msg)
        {
            foreach (var player in connectedClient)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(type + msg);
                player.playerSocket.Send(buffer);
            }
        }
    }
}
