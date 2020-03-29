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
    class Bishop : CommonPiece
    {
        public Bishop(piece p, color c)
            : base(p, c)
        {
            base.Uri = c == color.eWhite ?
                new Uri(@"../Resources/Bishop.png", UriKind.Relative) :
                new Uri(@"../Resources/BishopB.png", UriKind.Relative);

            Img = new Image();
            Img.Source = new BitmapImage(Uri);
        }


        public override List<Tuple<int, int>> getPossibleMovesWhite(int i, int j)
        {
            if (Table.table[i, j].Piece.Color != color.eWhite)
                return null;

            List<Tuple<int, int>> arrPosibleMoves = new List<Tuple<int, int>>();
            populateDiagonalBottom_Left(i, j, color.eBrown, ref arrPosibleMoves);
            populateDiagonalBottom_Right(i, j, color.eBrown, ref arrPosibleMoves);
            populateDiagonalTop_Left(i, j, color.eBrown, ref arrPosibleMoves);
            populateDiagonalTop_Right(i, j, color.eBrown, ref arrPosibleMoves);
            return arrPosibleMoves;
        }

        public override List<Tuple<int, int>> getPossibleMovesBlack(int i, int j)
        {
            if (Table.table[i, j].Piece.Color != color.eBrown)
                return null;

            List<Tuple<int, int>> arrPosibleMoves = new List<Tuple<int, int>>();
            populateDiagonalBottom_Left(i, j, color.eWhite, ref arrPosibleMoves);
            populateDiagonalBottom_Right(i, j, color.eWhite, ref arrPosibleMoves);
            populateDiagonalTop_Left(i, j, color.eWhite, ref arrPosibleMoves);
            populateDiagonalTop_Right(i, j, color.eWhite, ref arrPosibleMoves);
            return arrPosibleMoves;
        }

    }
}
