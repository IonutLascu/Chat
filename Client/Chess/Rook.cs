﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    class Rook : CommonPiece
    {
        public Rook(piece p, color c)
            : base(p, c)
        {
            base.Uri = c == color.eWhite ?
                new Uri(@"../Chess/Resources/Rook.png", UriKind.Relative) :
                new Uri(@"../Chess/Resources/RookB.png", UriKind.Relative);

            Img = new Image();
            Img.Source = new BitmapImage(Uri);
        }

        public override List<Tuple<int, int>> getPossibleMovesWhite(int i, int j)
        {
            if (Table.table[i, j].Piece.Color != color.eWhite)
                return null;

            List<Tuple<int, int>> arrPosibleMoves = new List<Tuple<int,int>>();
            populateRowGreatherThanIndex(i, j, color.eBrown, ref arrPosibleMoves);
            populateRowLowerThanIndex(i, j, color.eBrown, ref arrPosibleMoves);
            populateColumnGreatherThanIndex(i, j, color.eBrown, ref arrPosibleMoves);
            populateColumnLowerThanIndex(i, j, color.eBrown, ref arrPosibleMoves);
            return arrPosibleMoves;
        }

        public override List<Tuple<int, int>> getPossibleMovesBlack(int i, int j)
        {
            if (Table.table[i, j].Piece.Color != color.eBrown)
                return null;

            List<Tuple<int, int>> arrPosibleMoves = new List<Tuple<int, int>>();
            populateRowGreatherThanIndex(i, j, color.eWhite, ref arrPosibleMoves);
            populateRowLowerThanIndex(i, j, color.eWhite, ref arrPosibleMoves);
            populateColumnGreatherThanIndex(i, j, color.eWhite, ref arrPosibleMoves);
            populateColumnLowerThanIndex(i, j, color.eWhite, ref arrPosibleMoves);
            return arrPosibleMoves;
        }


    }
}
