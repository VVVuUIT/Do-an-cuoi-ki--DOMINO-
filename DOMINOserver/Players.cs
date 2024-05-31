using System.Net.Sockets;

namespace DOMINOserver
{
     class Player
    {
        public string name { get; set; }
        public int turn { get; set; }
        public int numOfCards { get; set; }
        public bool isHost { get; set; }
        public Socket playerSocket { get; set; }
    }
}
