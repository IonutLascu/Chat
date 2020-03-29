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
    public enum piece
    {
        ePawn,
        eRook,
        eKnight,
        eBishop,
        eQueen,
        eKing,
    }
    
    public class Piece
    {
        public Image Img { get; set; }
        public piece Name { get; set; }
        public color Color { get; set; }
        public Uri Uri{ get; set; }
        public Piece(piece name, color color)
        {
            Name = name;
            Color = color;
        }
        public virtual List<Tuple<int, int>> getPossibleMovesWhite(int i, int j) { return null; }
        public virtual List<Tuple<int, int>> getPossibleMovesBlack(int i, int j ) { return null; }

        public List<Tuple<int, int>> getPossibleMoves(int i, int j)
        {
            if (Color == color.eBrown)
                return getPossibleMovesBlack(i, j);
            return getPossibleMovesWhite(i, j);
        }

        public bool checkIsOnTable(int i, int j)
        {
            if (i < 0 || i > 7)
                return false;
            if(j < 0 || j > 7)
                return false;
            return true;
        }
    }
}
