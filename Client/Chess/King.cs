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
    class King : Piece
    {
        public static bool isKingWhiteInChess = false;
        public static bool isKingBrawnInChess = false;
        public King(piece p, color c)
            : base(p, c)
        {
            base.Uri = c == color.eWhite ?
                new Uri(@"../Chess/Resources/King.png", UriKind.Relative) :
                new Uri(@"../Chess/Resources/KingB.png", UriKind.Relative);

            Img = new Image();
            Img.Source = new BitmapImage(Uri);
        }

        private bool check(int i, int j, color c)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j) == false)
                return false;

            if (tempTable[i, j].Piece != null)
                if (tempTable[i, j].Piece.Color != c && tempTable[i, j].Piece.Name == piece.eKing)
                    return true;
            return false;
        }

        private bool checkTheKing(int i, int j, color c)
        {
            if (check(i - 1, j - 1, c) ||
                check(i - 1, j, c) ||
                check(i - 1, j + 1, c) ||
                check(i, j - 1, c) ||
                check(i, j + 1, c) ||
                check(i + 1, j - 1, c) ||
                check(i + 1, j, c) ||
                check(i + 1, j + 1, c))
                return false;
            return true;
        }


        private void setMoves(int i, int j, color c, ref List<Tuple<int, int>> arrMoves)
        {
            Square[,] tempTable = Table.table;

            if (checkIsOnTable(i, j) && checkTheKing(i, j, c))
            {
                if (tempTable[i, j].Piece != null && tempTable[i, j].Piece.Color != c)
                    arrMoves.Add(new Tuple<int, int>(i, j));
                else if (tempTable[i, j].Piece == null)
                    arrMoves.Add(new Tuple<int, int>(i, j));
            }
        }

        private List<Tuple<int, int>> getMovesKing(int i, int j, color c)
        {
            List<Tuple<int, int>> arrPossibleMoves = new List<Tuple<int, int>>();
            //top-left
            setMoves(i - 1, j - 1, c, ref arrPossibleMoves);
            //top-middle
            setMoves(i - 1, j, c, ref arrPossibleMoves);
            //top-right
            setMoves(i - 1, j + 1, c, ref arrPossibleMoves);
            //middle-left
            setMoves(i, j - 1, c, ref arrPossibleMoves);
            //middle-right
            setMoves(i, j + 1, c, ref arrPossibleMoves);
            //bottom-left
            setMoves(i + 1, j - 1, c, ref arrPossibleMoves);
            //bottom-middle
            setMoves(i + 1, j, c, ref arrPossibleMoves);
            //bottom-right
            setMoves(i + 1, j + 1, c, ref arrPossibleMoves);

            return arrPossibleMoves;
        }

        public override List<Tuple<int, int>> getPossibleMovesWhite(int i, int j)
        {
            return getMovesKing(i, j, color.eWhite);
        }

        public override List<Tuple<int, int>> getPossibleMovesBlack(int i, int j)
        {
            return getMovesKing(i, j, color.eBrown);
        }

    }
}
