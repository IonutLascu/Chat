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
    public enum color
    {
        eBrown,
        eWhite,
    }
    
    public partial class Table : UserControl
    {
        static public Square[,] table = new Square[8, 8];
        static public Square selectedPiece = null;
        static public int Turn = 0;
        static public List<Square> arrCaputeredPiece = new List<Square>();

        static private Piece getPieceInstance(piece p, color c)
        {
            switch (p)
            {
                case piece.ePawn:
                    return new Pawn(p, c);

                case piece.eRook:
                    return new Rook(p, c);

                case piece.eKnight:
                    return new Knight(p, c);

                case piece.eBishop:
                    return new Bishop(p, c);

                case piece.eKing:
                    return new King(p, c);

                case piece.eQueen:
                    return new Queen(p, c);
            }
            return null;

        }
        public Table()
        {
            InitializeComponent();
            initTableSqare();
            initTablePiece();
        }

        /*
         Func<int, string> letter = (int number) => {

                if (number == 0)
                    return "A";
                else if (number == 1)
                    return "B";
                else if (number == 2)
                    return "C";
                else if (number == 3)
                    return "D";
                else if (number == 4)
                    return "E";
                else if (number == 5)
                    return "F";
                else if (number == 6)
                    return "G";
                else if (number == 7)
                    return "H";
                return "";
            };
         */

        private void Add(int i, int j, color c, Piece p)
        {
            Square sq = new Square(c, p, i, j);
            sq.addEventClick();
            mainTable.Children.Add(sq);
            Grid.SetRow(sq, i);
            Grid.SetColumn(sq, j);
            table[i, j] = sq;
        }

        private void initMainPiece(int index, color c)
        {
            table[index, 0].initPiece(Table.getPieceInstance(piece.eRook, c));
            table[index, 1].initPiece(Table.getPieceInstance(piece.eKnight, c));
            table[index, 2].initPiece(Table.getPieceInstance(piece.eBishop, c));
            table[index, 3].initPiece(Table.getPieceInstance(piece.eQueen, c));
            table[index, 4].initPiece(Table.getPieceInstance(piece.eKing, c));
            table[index, 5].initPiece(Table.getPieceInstance(piece.eBishop, c));
            table[index, 6].initPiece(Table.getPieceInstance(piece.eKnight, c));
            table[index, 7].initPiece(Table.getPieceInstance(piece.eRook, c));
        }


        public void initTablePiece()
        {
            for (int i = 0; i < 8; i++)
            {
                table[1, i].initPiece(Table.getPieceInstance(piece.ePawn, color.eBrown));
                table[6, i].initPiece(Table.getPieceInstance(piece.ePawn, color.eWhite));
            }
            initMainPiece(0, color.eBrown);
            initMainPiece(7, color.eWhite);


        }
        public void initTableSqare()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                            Add(i, j, color.eWhite, null);
                        else
                            Add(i, j, color.eBrown, null);
                    }
                    else
                    {
                        if (j % 2 == 0)
                            Add(i, j, color.eBrown, null);
                        else
                            Add(i, j, color.eWhite, null);
                    }

                }
                
            }
        }
    }
}
