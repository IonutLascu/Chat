using Client.Chess;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chess
{
    public enum color
    {
        eBrown,
        eWhite,
    }

    public struct Moves
    {
        public Moves(Square from, Square to)
        {
            fromSquare = from;
            toSquare = to;
        }

        public Moves(int FromRow, int FromColumn, int ToRow, int ToColumn)
        {
            fromSquare = Table.table[FromRow, FromColumn];
            toSquare = Table.table[ToRow, ToColumn];
        }

        private Square fromSquare;
        private Square toSquare;

        public Square FromSquare { get => fromSquare; set => fromSquare = value; }
        public Square ToSquare { get => toSquare; set => toSquare = value; }

    }

    public partial class Table : UserControl
    {
        static public Square[,] table = new Square[8, 8];
        private Square selectedPiece = null;

        private bool isWhiteTurn = false;
        private bool isBrownTurn = false;

        private static ObservableCollection<Square> arrCaputeredPiece = new ObservableCollection<Square>();
        public static ObservableCollection<Square> ArrCaputeredPiece { get => arrCaputeredPiece; set => arrCaputeredPiece = value; }

        private static ObservableCollection<Moves> arrMoves = new ObservableCollection<Moves>();
        public static ObservableCollection<Moves> ArrMoves { get => arrMoves; set => arrMoves = value; }

        private static ObservableCollection<Moves> arrOponentMoves = new ObservableCollection<Moves>();
        public static ObservableCollection<Moves> ArrOponentMoves { get => arrOponentMoves; set => arrOponentMoves = value; }

        private static InstanceGame instanceGame = null;
        public static InstanceGame InstanceGame { get => instanceGame; set => instanceGame = value; }

        private Piece getPieceInstance(piece p, color c)
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

            arrMoves = new ObservableCollection<Moves>();
            arrOponentMoves = new ObservableCollection<Moves>();
            arrCaputeredPiece = new ObservableCollection<Square>();
            
            if(instanceGame.Player.IsWhite == true)
                isWhiteTurn = true;

            ArrOponentMoves.CollectionChanged -= UpdateTable;
            ArrOponentMoves.CollectionChanged += UpdateTable;

            lblNamePlayer.Content = instanceGame.Player.Username;
            lblNameOpponent.Content = instanceGame.Opponent.Username;

            initTableSqare();
            initTablePiece();
            instanceGame.Player.StpWatch = swPlayerTimer;
            instanceGame.Opponent.StpWatch = swOpponentTimer;

            swPlayerTimer.timeElapsed -= TimeElapsed;
            swPlayerTimer.timeElapsed += TimeElapsed;

            if (isWhiteTurn == true)
                swPlayerTimer.StartTimer();
            else
                swOpponentTimer.StartTimer();
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

        private void Add(int i, int j, Brush b, Piece p)
        {
            Square sq = new Square(b, p, i, j);
            sq.PreviewMouseDown -= Square_Click;
            sq.PreviewMouseDown += Square_Click;
            mainTable.Children.Add(sq);
            Grid.SetRow(sq, i);
            Grid.SetColumn(sq, j);
            table[i, j] = sq;
        }

        private void initMainPiece(int index, color c)
        {
            table[index, 0].initPiece(getPieceInstance(piece.eRook, c));
            table[index, 1].initPiece(getPieceInstance(piece.eKnight, c));
            table[index, 2].initPiece(getPieceInstance(piece.eBishop, c));
            table[index, 3].initPiece(getPieceInstance(piece.eQueen, c));
            table[index, 4].initPiece(getPieceInstance(piece.eKing, c));
            table[index, 5].initPiece(getPieceInstance(piece.eBishop, c));
            table[index, 6].initPiece(getPieceInstance(piece.eKnight, c));
            table[index, 7].initPiece(getPieceInstance(piece.eRook, c));
        }

        private void initTablePiece()
        {
            for (int i = 0; i < 8; i++)
            {
                table[1, i].initPiece(getPieceInstance(piece.ePawn, color.eBrown));
                table[6, i].initPiece(getPieceInstance(piece.ePawn, color.eWhite));
            }
            initMainPiece(0, color.eBrown);
            initMainPiece(7, color.eWhite);


        }

        private void initTableSqare()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                            Add(i, j, Brushes.White, null);
                        else
                            Add(i, j, Brushes.SaddleBrown, null);
                    }
                    else
                    {
                        if (j % 2 == 0)
                            Add(i, j, Brushes.SaddleBrown, null);
                        else
                            Add(i, j, Brushes.White, null);
                    }

                }

            }
        }

        private void make_VisibleColorSquare(Square sq)
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

        private void make_reinitColorSquare(Square sq)
        {
            //reinitialize square
            List<Tuple<int, int>> possibleMoves = sq.Piece.getPossibleMoves(sq.Row, sq.Column);
            for (int i = 0; i < possibleMoves.Count(); i++)
            {
                int r = possibleMoves.ElementAt(i).Item1;
                int c = possibleMoves.ElementAt(i).Item2;
                table[r, c].Box.Background = table[r, c].ColorSquare;
            }
        }

        void copyContentSquare(Square source, Square dest)
        {
            table[dest.Row, dest.Column].Piece = source.Piece;
            table[dest.Row, dest.Column].Box.Content = source.Box.Content;
        }

        void clearContentSquare(ref Square sq)
        {
            sq.Piece = null;
            sq.Box.Content = null;
        }

        private void isKingInChess(color color)
        {
            if (color == color.eWhite)
                King.isKingWhiteInChess = false;
            else
                King.isKingBrawnInChess = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (table[i, j].Piece != null)
                    {
                        if (color == color.eBrown)
                            table[i, j].Piece.getPossibleMovesWhite(i, j);
                        else
                            table[i, j].Piece.getPossibleMovesBlack(i, j);
                    }
                }
            }
        }

        private bool isCheckMate(color colorKing)
        {
            bool checkMate = true;
            //check each piece moves with same color as the king
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square currentSquare = table[i, j];
                    if (currentSquare.Piece != null && currentSquare.Piece.Color == colorKing)
                    {
                        List<Tuple<int, int>> possibleMoves = currentSquare.Piece.getPossibleMoves(currentSquare.Row, currentSquare.Column);
                        for (int k = 0; k < possibleMoves.Count; k++)
                        {
                            Square nextMove = table[possibleMoves[k].Item1, possibleMoves[k].Item2];
                            Square keepInMindPiece = nextMove.Clone() as Square;
                            nextMove.Piece = currentSquare.Piece;
                            currentSquare.Piece = null;
                            isKingInChess(colorKing);
                            if (King.IsKingInChess(colorKing) == false)
                                checkMate = false;

                            currentSquare.Piece = nextMove.Piece;
                            nextMove.Piece = keepInMindPiece.Piece;

                            //break the loop if possible move was found
                            if (checkMate == false)
                                return false;
                        }
                    }
                }

            }
            return checkMate;
        }

        public bool MovePiece(Moves move)
        {
            Square fromSquare = table[move.FromSquare.Row, move.FromSquare.Column];
            Square toSquare = table[move.ToSquare.Row, move.ToSquare.Column];

            bool isPieceCaptured = false;

            //move piece
            Square keepInMindPiece = toSquare.Clone() as Square;
            fromSquare.Box.Background = fromSquare.ColorSquare;
            make_reinitColorSquare(fromSquare);
            copyContentSquare(fromSquare, toSquare);
            if (keepInMindPiece.Piece != null)
            {
                ArrCaputeredPiece.Add(keepInMindPiece);   //save captured piece
                isPieceCaptured = true;
            }
            clearContentSquare(ref fromSquare);    //clear selected item

            //call this method to know to init static bool from king
            if (isWhiteTurn && toSquare.Piece != null && toSquare.Piece.Color == color.eWhite)
            {
                isKingInChess(color.eWhite);
                if (King.isKingWhiteInChess == true)
                {
                    //the move is not good
                    MessageBox.Show("King in chess!", "", MessageBoxButton.OK);
                    copyContentSquare(toSquare, fromSquare);
                    copyContentSquare(keepInMindPiece, toSquare);
                    if (isPieceCaptured == true)
                        ArrCaputeredPiece.Remove(keepInMindPiece);
                    //clearContentSquare(ref table[toSquare.Row, toSquare.Column]);
                    return false;
                }
            }
            else if (isBrownTurn && toSquare.Piece != null && toSquare.Piece.Color == color.eBrown)
            {
                isKingInChess(color.eBrown);
                if (King.isKingBrawnInChess == true)
                {
                    MessageBox.Show("King in chess!", "", MessageBoxButton.OK);
                    copyContentSquare(toSquare, fromSquare);
                    copyContentSquare(keepInMindPiece, toSquare);
                    if (isPieceCaptured == true)
                        ArrCaputeredPiece.Remove(keepInMindPiece);
                    //clearContentSquare(ref table[toSquare.Row, toSquare.Column]);
                    return false;
                }
            }
            return true;
        }

        private void UpdateTable(object sender, NotifyCollectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                 ObservableCollection<Moves> collection = sender as ObservableCollection<Moves>;
                 MovePiece(collection.Last());
            });
            if (InstanceGame.Player.IsWhite)
            {
                isWhiteTurn = true;
                isBrownTurn = false;
                TitleTurn.Content= "It's white turn...";
            }
            else if (InstanceGame.Player.IsBlack)
            {
                isBrownTurn = true;
                isWhiteTurn = false;
                TitleTurn.Content = "It's black turn...";
            }
        }

        Square getLastPieceCaptured()
        {
            return ArrCaputeredPiece[ArrCaputeredPiece.Count - 1];
        }

        void Square_Click(object sender, RoutedEventArgs e)
        {
            if (!isWhiteTurn && !isBrownTurn)
                return;
            Square selectedSquare = sender as Square;
            if (selectedSquare.Piece != null &&
                ((isWhiteTurn && selectedSquare.Piece.Color == color.eWhite) ||
                (isBrownTurn && selectedSquare.Piece.Color == color.eBrown)))
            {
                if (selectedSquare.Piece.Color == color.eWhite && isBrownTurn)
                    return;

                if (selectedSquare.Piece.Color == color.eBrown && isWhiteTurn)
                    return;

                if (selectedPiece != null)
                {
                    selectedPiece.Box.Background = selectedPiece.ColorSquare;
                    make_reinitColorSquare(selectedPiece);
                }
                selectedPiece = selectedSquare;
                selectedPiece.Box.Background = Brushes.LightGreen;

                make_VisibleColorSquare(selectedSquare);
            }
            else if (selectedPiece != null && selectedSquare.Box.Background == Brushes.LightGreen)
            {
                Moves mv = new Moves(selectedPiece, selectedSquare);
                if (MovePiece(mv) == true)
                {
                    if (isWhiteTurn)
                    {
                        isKingInChess(color.eBrown);
                        if (King.isKingBrawnInChess == true)
                            if (isCheckMate(color.eBrown) == true)
                                InstanceGame.IsFinishGame = true;

                    }
                    else if (isBrownTurn)
                    {
                        isKingInChess(color.eWhite);
                        if (King.isKingWhiteInChess == true)
                            if (isCheckMate(color.eWhite) == true)
                                InstanceGame.IsFinishGame = true;
                    }
                    if (InstanceGame.Player.IsWhite)
                    {
                        isWhiteTurn= false;
                        TitleTurn.Content = "It's black turn...";
                    }
                    else if (Table.InstanceGame.Player.IsBlack)
                    {
                        isBrownTurn = false;
                        TitleTurn.Content = "It's white turn...";
                    }
                    ArrMoves.Add(mv);
                    selectedPiece = null;
                }
            }
        }

        private void TimeElapsed(object sender, EventArgs e)
        {
            isWhiteTurn = false;
            isBrownTurn = false;

            instanceGame.IsFinishGame = false;
            arrMoves.Add(new Moves(0, 0, 0, 0));
        }

    }
}
