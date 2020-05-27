using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Chess
{
    public class Player
    {
        private string username;
        private bool isWhite = false;
        private bool isBlack = false;

        public string Username { get => username; set => username = value; }
        public bool IsBlack { get => isBlack; set => isBlack = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }

        public Player(string usr) {
            username = usr;
        }
    }
}
