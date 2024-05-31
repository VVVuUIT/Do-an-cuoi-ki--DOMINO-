using System.Collections.Generic;

namespace DOMINOserver
{
    class DrawPile
    {
        public static string faceupcard = "";
        public static string[] card_id =
        {
            "k_k","k_m","k_h","k_b","k_t","k_n","k_s",
            "m_m","m_h","m_b","m_t","m_n","m_s",
            "h_h","h_b","h_t","h_n","h_s",
            "b_b","b_t","b_n","b_s",
            "t_t","t_n","t_s",
            "n_n","n_s",
            "s_s","v_s"
        };
    }
    class DiscardPile
    {
        public static List<string> disPile = new List<string>();
    }
}
