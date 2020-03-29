using System;
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
    class Knight : Piece
    {
        public Knight(piece p, color c)
            : base(p, c)
        {
            base.Uri = c == color.eWhite ?
                new Uri(@"../Resources/Knight.png", UriKind.Relative) :
                new Uri(@"../Resources/KnightB.png", UriKind.Relative);

            Img = new Image();
            Img.Source = new BitmapImage(Uri);
        }

        private void addMoves(int i, int j, color c, ref List<Tuple<int, int>> arrPosMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                if ((tempTable[i, j].Piece != null &&
                    tempTable[i, j].Piece.Color != c &&
                    tempTable[i, j].Piece.Name != piece.eKing) || tempTable[i, j].Piece == null)
                    arrPosMoves.Add(new Tuple<int, int>(i, j));
                if (tempTable[i,j].Piece != null && 
                    tempTable[i, j].Piece.Name == piece.eKing && 
                    tempTable[i,j].Piece.Color != c)
                {
                    if (c == color.eBrown)
                        King.isKingWhiteInChess = true;
                    else
                        King.isKingBrawnInChess = true;
                }

            }
        }


        public List<Tuple<int, int>>
            getMoves(int i, int j, color c)
        {
            Square[,] tempTable = Table.table;
            List<Tuple<int, int>> arrPossibleMoves = new List<Tuple<int, int>>();

            //top possibilities
            addMoves(i + 2, j - 1, c, ref arrPossibleMoves);
            addMoves(i + 2, j + 1, c, ref arrPossibleMoves);

            //bottom possibilities
            addMoves(i - 2, j - 1, c, ref arrPossibleMoves);
            addMoves(i - 2, j + 1, c, ref arrPossibleMoves);

            //left possibilities
            addMoves(i + 1, j - 2, c, ref arrPossibleMoves);
            addMoves(i - 1, j - 2, c, ref arrPossibleMoves);

            //right possibilities
            addMoves(i + 1, j + 2, c, ref arrPossibleMoves);
            addMoves(i - 1, j + 2, c, ref arrPossibleMoves);

            return arrPossibleMoves;
        }

        public override List<Tuple<int, int>> getPossibleMovesWhite(int i, int j)
        {
            if (Table.table[i, j].Piece.Color != color.eWhite)
                return null;

            return getMoves(i, j, color.eWhite);
        }

        public override List<Tuple<int, int>> getPossibleMovesBlack(int i, int j)
        {
            if (Table.table[i, j].Piece.Color != color.eBrown)
                return null;

            return getMoves(i, j, color.eBrown);
        }
    }
}
