using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace DOMINOclient
{
    class ClientSocket
    {
        public static Socket clientSocket;
        public static Thread recvThread;
        public static string datatype = "";
        public static void Connect(IPEndPoint serverEP)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverEP);
            recvThread = new Thread(() => readingReturnData());
            recvThread.Start();
        }
        public static void SendMessage(string data)
        {
            string msgstr = datatype + "-" + data;
            byte[] msg = Encoding.UTF8.GetBytes(msgstr);
            clientSocket.Send(msg);
        }
        public static void readingReturnData()
        {
            byte[] buffer = new byte[1024];
            while (clientSocket.Connected)
            {
                if (clientSocket.Available > 0)
                {
                    string msg = "";
                    while (clientSocket.Available > 0)
                    {
                        int bRead = clientSocket.Receive(buffer);
                        msg += Encoding.UTF8.GetString(buffer, 0, bRead);
                    }
                    AnalyzingReturnMessage(msg);
                    Menu.lobby.Tempdisplay(msg);
                }
            }
            recvThread.Abort();
        }
        public static Table table;
        public static List<OtherPlayers> otherPlayers;
        public static void AnalyzingReturnMessage(string msg)
        {
            string[] arrPayload = msg.Split('-');
            switch (arrPayload[0])
            {
                case "LOBBY":
                    {
                        Menu.lobby.DisplayConnectedPlayer(arrPayload[1]);
                    }
                    break;
                case "INIT":
                    {
                        ThisPlayer.turn = int.Parse(arrPayload[2]);
                        ThisPlayer.numOfCards = int.Parse(arrPayload[3]);
                        for (int i = 4; i <= 10; i++)
                        {
                            ThisPlayer.cards.Add(arrPayload[i]);
                        }
                        table = new Table();
                        otherPlayers = new List<OtherPlayers>();
                        Menu.lobby.Invoke((MethodInvoker)delegate ()
                        {
                            table.faceUpCard = arrPayload[11];
                            table.InitCardFetch();
                            table.DisplayFaceUp();
                            table.Show();
                        }
                        );
                    }
                    break;
                case "OTHER":
                    {
                        OtherPlayers otherplayer = new OtherPlayers();
                        otherplayer.name = arrPayload[1];
                        otherplayer.turn = arrPayload[2];
                        otherplayer.numOfCards = arrPayload[3];
                        otherPlayers.Add(otherplayer);
                    }
                    break;
                case "SETUP":
                    {
                        table.InitDisplay();
                    }
                    break;
                case "UPDATE":
                    {
                        table.UpdateNumOfCards(arrPayload[1], arrPayload[2]);
                        if (arrPayload.Length > 3)
                        {
                            table.faceUpCard = arrPayload[3];
                            table.DisplayFaceUp();
                        }
                    }
                    break;
                case "TURN":
                    {
                        if (arrPayload[1] == ThisPlayer.name)
                            CheckForPotentialCards();

                        table.UndoHighlightTurn();
                        table.HighlightTurn(arrPayload[1]);
                    }
                    break;
                case "PICKCARD":
                    {
                        table.Invoke((MethodInvoker)delegate ()
                        {
                            table.FetchDrawCard(arrPayload[2]);
                        }
                        );
                        CheckForPotentialCards();
                    }
                    break;
                case "END":
                    {
                        string winnerName = arrPayload[1];
                        Form VictoryForm = new VictoryForm(winnerName);
                        VictoryForm.ShowDialog();
                    }
                    break;
                default:
                    break;
            }
        }
        private static void Gameclosed(object sender, EventArgs e)
        {
            Menu.lobby.Show();
        }
        public static void CheckForPotentialCards()
        {
            table.EnableDrawBtn();
            table.EnableCancelBtn();
            foreach (var row in table.CardBtns)
            {
                foreach (var bt in row)
                {
                    string[] faceUpSecond = table.faceUpCard.Split('_');
                    string[] cardSecond = bt.id.Split('_');
                    if (faceUpSecond[0] == cardSecond[0] || faceUpSecond[0] == cardSecond[1] || faceUpSecond[1] == cardSecond[0] || faceUpSecond[1] == cardSecond[1])
                    {
                        bt.btn.FlatAppearance.BorderColor = Color.Chartreuse;
                        bt.btn.Enabled = true;
                        table.EnableDiscardBtn();
                        continue;
                    }
                    bt.btn.FlatAppearance.BorderColor = Color.Red;
                }    
            }    
        }
    }
}
