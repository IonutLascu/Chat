using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class Square : UserControl
    {
        public Brush ColorSquare { get; set; }
        public Piece Piece { get; set; }
        public int Row { get; set; }
        public int Column { get; set;}
        public Square(color color, Piece piece, int i, int j) 
        {
            InitializeComponent();
            Row = i; Column = j;
            initPiece(piece);
            Box.Background = ColorSquare =  color == color.eWhite ? Brushes.White : Brushes.SaddleBrown;
        }

        public void initPiece(Piece piece)
        {
            if (piece == null)
                return;
            Piece = piece;
            Box.Content = piece.Img;
        }
    }
}
