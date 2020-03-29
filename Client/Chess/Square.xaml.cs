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
    public partial class Square : UserControl
    {
        public color Color { get; set; }
        public Piece Piece { get; set; }
        public int Row { get; set; }
        public int Column { get; set;}
        public Square(color color, Piece piece, int i, int j) 
        {
            InitializeComponent();
            Row = i; Column = j;
            Color = color;
            Box.Background = color == color.eWhite ? Brushes.White : Brushes.SaddleBrown;
            initPiece(piece);
        }
        public void initPiece(Piece piece)
        {
            if (piece == null)
                return;
            Piece = piece;
            Box.Content = piece.Img;
        }


        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
        "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Square));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        void RaiseClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Square.ClickEvent);
            RaiseEvent(newEventArgs);
        }

        void OnClick()
        {
             RaiseClickEvent();
        }

        public void addEventClick()
        {
            PreviewMouseLeftButtonUp += (sender, args) => OnClick();
            this.Click += Square_Click;
        }

        Brush getColorSquare(Square sq)
        {
            return sq.Color == color.eBrown ? Brushes.SaddleBrown : Brushes.White;
        }

        public void make_VisibleColorSquare(Square sq)
        {
            //make visible all possible moves
            List<Tuple<int, int>> possibleMoves = sq.Piece.getPossibleMoves(sq.Row, sq.Column);
            for (int i = 0; i < possibleMoves.Count(); i++)
            {
                int r = possibleMoves.ElementAt(i).Item1;
                int c = possibleMoves.ElementAt(i).Item2;
                Table.table[r, c].Box.Background = Brushes.LightGreen;
            }
        }

        public void make_reinitColorSquare(Square sq)
        {
            //reinitialize color of the square
            List<Tuple<int, int>> possibleMoves = sq.Piece.getPossibleMoves(sq.Row, sq.Column);
            for (int i = 0; i < possibleMoves.Count(); i++)
            {
                int r = possibleMoves.ElementAt(i).Item1;
                int c = possibleMoves.ElementAt(i).Item2;
                Table.table[r, c].Box.Background = getColorSquare(Table.table[r,c]);
            }
        }

        void copyContentSquare(Square source, Square dest)
        {
            Table.table[dest.Row, dest.Column].Piece = source.Piece;
            Table.table[dest.Row, dest.Column].Box.Content = source.Box.Content;
        }

        void clearContentSquare(ref Square sq)
        {
            sq.Piece = null;
            sq.Box.Content = null;
        }

        public void isKingInChess(color color)
        {
            Square[,] tempTable = Table.table;
            if(color == color.eWhite)
                King.isKingWhiteInChess = false;
            else
                King.isKingBrawnInChess = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(tempTable[i,j].Piece != null)
                    {
                        if (color == color.eBrown)
                            tempTable[i, j].Piece.getPossibleMovesWhite(i, j);
                        else
                            tempTable[i, j].Piece.getPossibleMovesBlack(i, j);
                    }
                }
            }
        }

        Square getLastPieceCaptured()
        {
            return Table.arrCaputeredPiece[Table.arrCaputeredPiece.Count - 1];
        }

        void Square_Click(object sender, RoutedEventArgs e)
        {
            Square[,] table = Table.table;
            Square tempSq = sender as Square;
            
            if ((tempSq.Piece != null) && 
                (Table.Turn % 2 == 0 && tempSq.Piece.Color == color.eWhite ||
                 Table.Turn % 2 != 0 && tempSq.Piece.Color == color.eBrown))  //turn validation
            {
                if (Table.selectedPiece != null)
                {
                    Table.selectedPiece.Box.Background = getColorSquare(Table.selectedPiece);
                    make_reinitColorSquare(Table.selectedPiece);
                }
                Table.selectedPiece = tempSq;
                Table.selectedPiece.Box.Background = Brushes.LightGreen;

                make_VisibleColorSquare(tempSq);
            }
            else if (Table.selectedPiece != null && tempSq.Box.Background == Brushes.LightGreen)
            {
                //move piece
                Table.selectedPiece.Box.Background = getColorSquare(Table.selectedPiece);
                make_reinitColorSquare(Table.selectedPiece);
                copyContentSquare(Table.selectedPiece, table[tempSq.Row, tempSq.Column]);
                Table.arrCaputeredPiece.Add(Table.selectedPiece);   //save captured piece
                clearContentSquare(ref Table.selectedPiece);    //clear selected item
                
                //call this method to know to init static bool from king
                if(Table.Turn % 2 == 0 && tempSq.Piece.Color == color.eWhite)
                {
                    isKingInChess(color.eWhite);
                    if(King.isKingWhiteInChess == true)
                    {
                        //the move is not good
                        MessageBox.Show("Ai grija fraiere!", "", MessageBoxButton.OK);
                        copyContentSquare(table[tempSq.Row, tempSq.Column], getLastPieceCaptured());
                        clearContentSquare(ref table[tempSq.Row, tempSq.Column]);
                        return;
                    }

                }
                else if (Table.Turn % 2 != 0 && tempSq.Piece.Color == color.eBrown)
                {
                    isKingInChess(color.eBrown);
                    if (King.isKingBrawnInChess == true)
                    {
                        MessageBox.Show("Ai grija fraiere!", "", MessageBoxButton.OK);
                        copyContentSquare(table[tempSq.Row, tempSq.Column], getLastPieceCaptured());
                        clearContentSquare(ref table[tempSq.Row, tempSq.Column]);
                        return;
                    }
                }
                Table.selectedPiece = null;
                Table.Turn++;
            }

        }

    }
}
