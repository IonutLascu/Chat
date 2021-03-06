﻿using Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Chess
{
    public class InstanceGame
    {
        private Player player;
        private Player opponent;
        private bool? isFinishedGame = null;
        
        public Player Player { get => player; set => player = value; }
        public Player Opponent { get => opponent; set => opponent = value; }
        public bool? IsFinishGame { get => isFinishedGame; set => isFinishedGame = value; }

        public InstanceGame(Player pl, Player op)
        {
            player = pl;
            opponent = op;
        }
      
    }
}
