using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DOMINOclient
{
    public partial class Lobby : Form
    {
        public Lobby lobby;
        public List<Label> PlayerName = new List<Label>();
        public int connectedPlayer = 0;
        public Lobby()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            lobby = this;
            btnBegin.Visible = false;
            PlayerName.Add(labelP1);
            PlayerName.Add(labelP2);
            PlayerName.Add(labelP3);
            PlayerName.Add(labelP4);    
        }
        public void ShowStartButton()
        {
            btnBegin.Visible = true;
        }
        public void Tempdisplay(string msg)
        {
            richTextBox1.Text += msg + '\n';
        }
        public void DisplayConnectedPlayer(string name)
        {
            connectedPlayer++;
            switch (connectedPlayer)
            {
                case 1:
                    labelP1.Text = name;
                    break;
                case 2:
                    labelP2.Text = name;
                    break;
                case 3:
                    labelP3.Text = name;
                    break;
                case 4:
                    labelP4.Text = name;
                    break;
                default:
                    break;
            }
        }                
        private void btnBegin_Click(object sender, EventArgs e)
        {
            ClientSocket.datatype = "BEGIN";
            ClientSocket.SendMessage("");
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
 }
