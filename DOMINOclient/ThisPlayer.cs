using System.Collections.Generic;

namespace DOMINOclient
{
    class ThisPlayer
    {
        public static string name { get; set; }
        public static int turn { get; set; }
        public static int numOfCards { get; set; }
        public static List<string> cards = new List<string>();
    }

    class OtherPlayers
    {
        public string name { get; set; }
        public string turn { get; set; }
        public string numOfCards { get; set; }
    }
}
