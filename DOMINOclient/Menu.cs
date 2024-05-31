using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
namespace DOMINOclient
{
    public partial class Menu : Form
    {
        public static Lobby lobby;
        public Menu()
        {
            InitializeComponent();
        }       
        void lobby_FormClosed(object sender, EventArgs e)
        {
            ClientSocket.datatype = "DISCONNECT";
            ClientSocket.SendMessage(ThisPlayer.name);
            ClientSocket.clientSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            ClientSocket.clientSocket.Close();
            this.Show();
        }                   
        private void btnCreate_Click(object sender, EventArgs e)
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(textBoxIP.Text), 11000);
            ClientSocket.datatype = "CONNECT";
            ClientSocket.Connect(serverEP);
            lobby = new Lobby();
            ClientSocket.SendMessage(textBoxName.Text);
            ThisPlayer.name = textBoxName.Text;
            lobby.FormClosed += new FormClosedEventHandler(lobby_FormClosed);
            lobby.ShowStartButton();
            this.Hide();
            lobby.Show();
        }
        private void btnJoin_Click(object sender, EventArgs e)
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(textBoxIP.Text), 11000);
            ClientSocket.datatype = "CONNECT";
            ClientSocket.Connect(serverEP);
            lobby = new Lobby();
            ClientSocket.SendMessage(textBoxName.Text);
            ThisPlayer.name = textBoxName.Text;
            lobby.FormClosed += new FormClosedEventHandler(lobby_FormClosed);
            this.Hide();
            lobby.Show();
        }
        private void btnRules_Click(object sender, EventArgs e)
        {
            this.Hide();
            Rule rl = new Rule();
            rl.Show();
        }
    }
}
