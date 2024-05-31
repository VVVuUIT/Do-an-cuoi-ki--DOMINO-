using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DOMINOclient
{
    public partial class Table : Form
    {
        public string faceUpCard = "";
        public List<List<CardButton>> CardBtns;
        public List<Label> lbnames;
        public List<TextBox> tbnums;
        public int row = 0;

        public class CardButton
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string id { get; set; }
            public Button btn = new Button();
        }
        public Table()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            btnPlay.Enabled = false;
            btnPick.Enabled = false;
            btnDRAWPILE.Enabled = false;
            btnDRAWPILE.FlatStyle = FlatStyle.Flat;
            btnDRAWPILE.FlatAppearance.BorderSize = 2;
            btnDRAWPILE.FlatAppearance.BorderColor = Color.Black;
            btnDRAWPILE.BackgroundImageLayout = ImageLayout.Stretch;
            CardBtns = new List<List<CardButton>>();
            lbnames = new List<Label>();
            tbnums = new List<TextBox>();
        }
        public void EnableDiscardBtn()
        {
            btnPlay.Enabled = true;
        }
        public void EnableDrawBtn()
        {
            btnPick.Enabled = true;    
        }
        public void EnableCancelBtn()
        {
            btnSkip.Enabled = true;
        }
        public void InitCardFetch()
        {
            CardBtns.Add(new List<CardButton>());
            int X = 15;
            int Y = 450;
            int i = 0;
            foreach (var cd in ThisPlayer.cards)
            {
                CardButton cardbtn = new CardButton();
                cardbtn.id = cd;
                cardbtn.btn.Tag = cd;
                cardbtn.btn.FlatStyle = FlatStyle.Flat;
                cardbtn.btn.FlatAppearance.BorderSize = 2;
                cardbtn.btn.BackgroundImageLayout = ImageLayout.Stretch;
                cardbtn.btn.Size = new Size(125, 45);
                cardbtn.btn.Location = new Point(X + i * 131, Y);
                cardbtn.X = X + i * 131;
                cardbtn.Y = Y;
                cardbtn.btn.Click += new EventHandler(cardBtn_Click);
                FetchImg(cardbtn.btn, cd);
                CardBtns[row].Add(cardbtn);
                Controls.Add(cardbtn.btn);
                i++;
            }
            CardsIdle();
        }
        public void FetchImg(Button btn, string cardid)
        {
            switch (cardid)
            {
                case "k_k":
                    btn.BackgroundImage = Properties.Resources.kk;
                    break;
                case "k_m":
                    btn.BackgroundImage = Properties.Resources.km;
                    break;
                case "k_h":
                    btn.BackgroundImage = Properties.Resources.kh;
                    break;
                case "k_b":
                    btn.BackgroundImage = Properties.Resources.kb;
                    break;
                case "k_t":
                    btn.BackgroundImage = Properties.Resources.kt;
                    break;
                case "k_n":
                    btn.BackgroundImage = Properties.Resources.kn;
                    break;
                case "k_s":
                    btn.BackgroundImage = Properties.Resources.ks;
                    break;
                case "m_m":
                    btn.BackgroundImage = Properties.Resources.mm;
                    break;
                case "m_h":
                    btn.BackgroundImage = Properties.Resources.mh;
                    break;
                case "m_b":
                    btn.BackgroundImage = Properties.Resources.mb;
                    break;
                case "m_t":
                    btn.BackgroundImage = Properties.Resources.mt;
                    break;
                case "m_n":
                    btn.BackgroundImage = Properties.Resources.mn;
                    break;
                case "m_s":
                    btn.BackgroundImage = Properties.Resources.ms;
                    break;
                case "h_h":
                    btn.BackgroundImage = Properties.Resources.hh;
                    break;
                case "h_b":
                    btn.BackgroundImage = Properties.Resources.hb;
                    break;
                case "h_t":
                    btn.BackgroundImage = Properties.Resources.ht;
                    break;
                case "h_n":
                    btn.BackgroundImage = Properties.Resources.hn;
                    break;
                case "h_s":
                    btn.BackgroundImage = Properties.Resources.hs;
                    break;
                case "b_b":
                    btn.BackgroundImage = Properties.Resources.bb;
                    break;
                case "b_t":
                    btn.BackgroundImage = Properties.Resources.bt;
                    break;
                case "b_n":
                    btn.BackgroundImage = Properties.Resources.bn;
                    break;
                case "b_s":
                    btn.BackgroundImage = Properties.Resources.bs;
                    break;
                case "t_t":
                    btn.BackgroundImage = Properties.Resources.tt;
                    break;
                case "t_n":
                    btn.BackgroundImage = Properties.Resources.tn;
                    break;
                case "t_s":
                    btn.BackgroundImage = Properties.Resources.ts;
                    break;
                case "n_n":
                    btn.BackgroundImage = Properties.Resources.nn;
                    break;
                case "n_s":
                    btn.BackgroundImage = Properties.Resources.ns;
                    break;
                case "s_s":
                    btn.BackgroundImage = Properties.Resources.ss;
                    break;
            }
        }
        public void InitDisplay()
        {
            ClientSocket.otherPlayers.Sort((x, y) => x.turn.CompareTo(y.turn));
            labelName.Text = ThisPlayer.name;
            textBoxNum.Text = ThisPlayer.numOfCards.ToString();
            textBoxNum.Tag = ThisPlayer.name;
            lbnames.Add(labelName);
            tbnums.Add(textBoxNum);
            switch (ClientSocket.otherPlayers.Count)
            {
                case 1:
                    {
                        panelPlayerL.Visible = false;
                        panelPlayerR.Visible = false;
                        labelNameU.Text = ClientSocket.otherPlayers[0].name;
                        textBoxNumU.Tag = ClientSocket.otherPlayers[0].name;
                        textBoxNumU.Text = "7";
                        lbnames.Add(labelNameU);
                        tbnums.Add(textBoxNumU);
                    }
                    break;
                case 2:
                    {
                        panelPlayerU.Visible = false;
                        if (ThisPlayer.turn == 2)
                        {
                            labelNameL.Text = ClientSocket.otherPlayers[1].name;
                            textBoxNumL.Tag = ClientSocket.otherPlayers[1].name;
                            textBoxNumL.Text = "7";
                            labelNameR.Text = ClientSocket.otherPlayers[0].name;
                            textBoxNumR.Tag = ClientSocket.otherPlayers[0].name;
                            textBoxNumR.Text = "7";
                        }
                        else
                        {
                            labelNameL.Text = ClientSocket.otherPlayers[0].name;
                            textBoxNumL.Tag = ClientSocket.otherPlayers[0].name;
                            textBoxNumL.Text = "7";
                            labelNameR.Text = ClientSocket.otherPlayers[1].name;
                            textBoxNumR.Tag = ClientSocket.otherPlayers[1].name;
                            textBoxNumR.Text = "7";
                        }
                        lbnames.Add(labelNameL);
                        lbnames.Add(labelNameR);
                        tbnums.Add(textBoxNumL);
                        tbnums.Add(textBoxNumR);
                    }
                    break;
                case 3:
                    {
                        if (ThisPlayer.turn == 1 && ThisPlayer.turn == 4)
                        {
                            labelNameL.Text = ClientSocket.otherPlayers[0].name;
                            textBoxNumL.Tag = ClientSocket.otherPlayers[0].name;
                            textBoxNumL.Text = "7";
                            labelNameU.Text = ClientSocket.otherPlayers[1].name;
                            textBoxNumU.Tag = ClientSocket.otherPlayers[1].name;
                            textBoxNumU.Text = "7";
                            labelNameR.Text = ClientSocket.otherPlayers[2].name;
                            textBoxNumR.Tag = ClientSocket.otherPlayers[2].name;
                            textBoxNumR.Text = "7";
                        }
                        else if (ThisPlayer.turn == 2)
                        {
                            labelNameL.Text = ClientSocket.otherPlayers[1].name;
                            textBoxNumL.Tag = ClientSocket.otherPlayers[1].name;
                            textBoxNumL.Text = "7";
                            labelNameU.Text = ClientSocket.otherPlayers[2].name;
                            textBoxNumU.Tag = ClientSocket.otherPlayers[2].name;
                            textBoxNumU.Text = "7";
                            labelNameR.Text = ClientSocket.otherPlayers[0].name;
                            textBoxNumR.Tag = ClientSocket.otherPlayers[0].name;
                            textBoxNumR.Text = "7";
                        }
                        else
                        {
                            labelNameL.Text = ClientSocket.otherPlayers[2].name;
                            textBoxNumL.Tag = ClientSocket.otherPlayers[2].name;
                            textBoxNumL.Text = "7";
                            labelNameU.Text = ClientSocket.otherPlayers[0].name;
                            textBoxNumU.Tag = ClientSocket.otherPlayers[0].name;
                            textBoxNumU.Text = "7";
                            labelNameR.Text = ClientSocket.otherPlayers[1].name;
                            textBoxNumR.Tag = ClientSocket.otherPlayers[1].name;
                            textBoxNumR.Text = "7";
                        }
                        lbnames.Add(labelNameL);
                        lbnames.Add(labelNameU);
                        lbnames.Add(labelNameR);
                        tbnums.Add(textBoxNumL);
                        tbnums.Add(textBoxNumU);
                        tbnums.Add(textBoxNumR);
                    }
                    break;
            }
        }
        public void DisplayFaceUp()
        {
          FetchImg(btnDRAWPILE, faceUpCard);
        }

        public string tempTurnName = "";
        public void HighlightTurn(string name)
        {
            tempTurnName = name;
            foreach (var lb in lbnames)
            {
                if (lb.Text == name)
                {
                    lb.Font = new Font(lb.Font, FontStyle.Bold);
                    lb.ForeColor = Color.Red;
                    break;
                }
            }
        }
        public void UndoHighlightTurn()
        {
            foreach (var lb in lbnames)
            {
                if (lb.Text == tempTurnName)
                {
                    lb.Font = new Font(lb.Font, FontStyle.Regular);
                    lb.ForeColor = Color.Black;
                    break;
                }
            }
        }

        private int i;
        private int X = 35;
        private int Y = 653;
        public void FetchDrawCard(string cd)
        {
            CardButton cardbtn = new CardButton();
            cardbtn.id = cd;
            cardbtn.btn.Tag = cd;
            cardbtn.btn.FlatStyle = FlatStyle.Flat;
            cardbtn.btn.FlatAppearance.BorderSize = 2;
            cardbtn.btn.BackgroundImageLayout = ImageLayout.Stretch;
            cardbtn.btn.Size = new Size(168, 76);
            FetchImg(cardbtn.btn, cd);
            if (CardBtns[row].Count == 7)
            {
                i = 0;
                row++;
                CardBtns.Add(new List<CardButton>());
                cardbtn.X = X;
                cardbtn.Y = Y;
                cardbtn.btn.Location = new Point(X, Y);
            }
            else
            {
                cardbtn.X = X + i * 174;
                cardbtn.Y = Y;
                cardbtn.btn.Location = new Point(cardbtn.X, cardbtn.Y);
            }
            i++;
            CardBtns[row].Add(cardbtn);
            cardbtn.btn.Visible = false;
            Controls.Add(cardbtn.btn);
        }
        public void UpdateNumOfCards(string name, string n)
        {
            foreach (var tb in tbnums)
            {
                if (tb.Tag.ToString() == name)
                {
                    tb.Text = n;
                }
            }
        }
        public void CardsIdle()
        {
            foreach (var row in CardBtns)
            {
                foreach (var cdbtn in row)
                {
                    cdbtn.btn.FlatAppearance.BorderColor = Color.Black;
                    cdbtn.btn.Enabled = false;
                }
            }
        }

        public string selectedCardId = "";
        void cardBtn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            selectedCardId = btn.Tag.ToString();
        }

        private int currentdisplayrow = 0;
        private void btnPlay_Click(object sender, EventArgs e)
        {
            ThisPlayer.numOfCards--;
            ClientSocket.datatype = "PLAY";
            string msgPlay = ThisPlayer.name + "-" + ThisPlayer.numOfCards + "-" + selectedCardId;
            ClientSocket.SendMessage(msgPlay);
            btnPlay.Enabled = false;
            btnPick.Enabled = false;
            faceUpCard = selectedCardId;
            DisplayFaceUp();
            foreach (var cd in CardBtns[currentdisplayrow])
            {
                if (cd.btn.Tag.ToString() == selectedCardId)
                {
                    cd.btn.Visible = false;
                }
            }
            foreach (var tb in tbnums)
            {
                if (tb.Tag.ToString() == ThisPlayer.name)
                {
                    tb.Text = ThisPlayer.numOfCards.ToString();
                    break;
                }
            }
            CardsIdle();
        }
        private void btnPick_Click(object sender, EventArgs e)
        {
            ThisPlayer.numOfCards++;
            string msgPick = ThisPlayer.name + "-" + ThisPlayer.numOfCards;
            ClientSocket.datatype = "PICK";
            ClientSocket.SendMessage(msgPick);
            foreach (var tb in tbnums)
            {
                if (tb.Tag.ToString() == ThisPlayer.name)
                {

                    break;
                }
            }
            btnPick.Enabled = false;
            CardsIdle();
        }
        private void btnSkip_Click(object sender, EventArgs e)
        {
            string msgSkip = ThisPlayer.name + "-" + ThisPlayer.numOfCards;
            ClientSocket.datatype = "SKIP";
            ClientSocket.SendMessage(msgSkip);
            CardsIdle();
        }
    } 
}