using System.Windows.Forms;

namespace DOMINOclient
{
    public partial class VictoryForm : Form
    {
        private Label winnerLabel;
        public VictoryForm(string winnerName)
        {
            label3.Text = winnerName + "IS THE WINNER!";
        }
    }
}
