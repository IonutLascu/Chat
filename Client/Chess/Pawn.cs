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
    class Pawn : Piece
    {
        private bool possitionEnpassant = false;
        public bool PossitionEnpassant { get => possitionEnpassant; set => possitionEnpassant = value; }

        private bool canBeEnpassant = true;
        public bool CanBeEnpassant { get => canBeEnpassant; set => canBeEnpassant = value; }

        public Pawn(piece p, color c)
            : base(p, c)
        {
            base.Uri = c == color.eWhite ?
                new Uri(@"../Chess/Resources/Pawn.png", UriKind.Relative) :
                new Uri(@"../Chess/Resources/PawnB.png", UriKind.Relative);

            Img = new Image();
            Img.Source = new BitmapImage(Uri);
        }
        public override List<Tuple<int, int>> getPossibleMovesWhite(int i, int j)
        {
            List<Tuple<int, int>> arrMoves = new List<Tuple<int, int>>();
            Square[,] tempTable = Table.table;

            if (tempTable[i, j].Piece.Color == color.eBrown)
                return null;
            //initial position the pawn can jump two squares
            if (i == 6 && tempTable[i - 1, j].Piece == null)  
            {
                if (checkIsOnTable(i - 2, j) && tempTable[i - 2, j].Piece == null)
                    arrMoves.Add(new Tuple<int, int>(i - 2, j));
            }

            //jump only one square
            if (checkIsOnTable(i - 1, j) && tempTable[i - 1, j].Piece == null)
                arrMoves.Add(new Tuple<int, int>(i - 1, j));

            //check enpassant
            if (checkIsOnTable(i, j + 1) && tempTable[i, j + 1].Piece != null && 
                tempTable[i, j + 1].Piece.Name == piece.ePawn &&
                (tempTable[i, j + 1].Piece as Pawn).possitionEnpassant == true)
            {
                arrMoves.Add(new Tuple<int, int>(i - 1, j + 1));
            }
            if (checkIsOnTable(i, j - 1) && tempTable[i, j - 1].Piece != null && 
                tempTable[i, j - 1].Piece.Name == piece.ePawn &&
                (tempTable[i, j - 1].Piece as Pawn).possitionEnpassant == true)
            {
                arrMoves.Add(new Tuple<int, int>(i - 1, j - 1));
            }
            //check possible diagonal moves
            if (checkIsOnTable(i - 1, j + 1) && tempTable[i - 1, j + 1].Piece != null)
                if (tempTable[i - 1, j + 1].Piece.Color == color.eBrown && tempTable[i - 1, j + 1].Piece.Name != piece.eKing)
                    arrMoves.Add(new Tuple<int, int>(i - 1, j + 1));
                else if (tempTable[i - 1, j + 1].Piece.Color == color.eBrown && tempTable[i - 1, j + 1].Piece.Name == piece.eKing)
                    King.isKingBrawnInChess = true;
            if (checkIsOnTable(i - 1, j - 1) && tempTable[i - 1, j - 1].Piece != null)
                if (tempTable[i - 1, j - 1].Piece.Color == color.eBrown && tempTable[i - 1, j - 1].Piece.Name != piece.eKing)
                    arrMoves.Add(new Tuple<int, int>(i - 1, j - 1));
                else if (tempTable[i - 1, j - 1].Piece.Color == color.eBrown && tempTable[i - 1, j - 1].Piece.Name == piece.eKing)
                    King.isKingBrawnInChess = true;
            return arrMoves;
        }

        public override List<Tuple<int, int>> getPossibleMovesBlack(int i, int j)
        {
            List<Tuple<int, int>> arrMoves = new List<Tuple<int, int>>();
            Square[,] tempTable = Table.table;
            if (tempTable[i, j].Piece.Color == color.eWhite)
                return null;
            
            if (i == 1 && tempTable[i + 1, j].Piece == null)  //initial position the pawn can jump two squares
            {
                if (checkIsOnTable(i + 2, j) && tempTable[i + 2, j].Piece == null)
                    arrMoves.Add(new Tuple<int, int>(i + 2, j));
            }

            //jump only one square
            if (checkIsOnTable(i + 1, j) && tempTable[i + 1, j].Piece == null)
                arrMoves.Add(new Tuple<int, int>(i + 1, j));

            //check enpassant
            if (checkIsOnTable(i, j + 1) && tempTable[i, j + 1].Piece != null &&
                tempTable[i, j + 1].Piece.Name == piece.ePawn &&
                (tempTable[i, j + 1].Piece as Pawn).possitionEnpassant == true)
            {
                arrMoves.Add(new Tuple<int, int>(i + 1, j + 1));
            }
            if (checkIsOnTable(i, j - 1) && tempTable[i, j - 1].Piece != null &&
                tempTable[i, j - 1].Piece.Name == piece.ePawn &&
                (tempTable[i, j - 1].Piece as Pawn).possitionEnpassant == true)
            {
                arrMoves.Add(new Tuple<int, int>(i + 1, j - 1));
            }

            //check possible diagonal moves
            if (checkIsOnTable(i + 1, j + 1) && tempTable[i + 1, j + 1].Piece != null)
                if (tempTable[i + 1, j + 1].Piece.Color == color.eWhite && tempTable[i + 1, j + 1].Piece.Name != piece.eKing)
                    arrMoves.Add(new Tuple<int, int>(i + 1, j + 1));
                else if (tempTable[i + 1, j + 1].Piece.Color == color.eWhite && tempTable[i + 1, j + 1].Piece.Name == piece.eKing)
                    King.isKingWhiteInChess = true;
            if (checkIsOnTable(i + 1, j - 1) && tempTable[i + 1, j - 1].Piece != null)
                if (tempTable[i + 1, j - 1].Piece.Color == color.eWhite && tempTable[i + 1, j - 1].Piece.Name != piece.eKing)
                    arrMoves.Add(new Tuple<int, int>(i + 1, j - 1));
                else if (tempTable[i + 1, j - 1].Piece.Color == color.eWhite && tempTable[i + 1, j - 1].Piece.Name == piece.eKing)
                    King.isKingWhiteInChess = true;
            return arrMoves;
        }

        public void UpdateEnpassant(Square from, Square to)
        {
            Square[,] tempTable = Table.table;
            if (from.Row == 1 && to.Row == 3 && canBeEnpassant)
            {
                int i = to.Row;
                int j = to.Column;
                if ((checkIsOnTable(i, j + 1) && tempTable[i, j + 1].Piece != null &&
                    tempTable[i, j + 1].Piece.Name == piece.ePawn && tempTable[i, j + 1].Piece.Color == color.eWhite) ||
                    (checkIsOnTable(i, j - 1) && tempTable[i, j - 1].Piece != null &&
                    tempTable[i, j - 1].Piece.Name == piece.ePawn && tempTable[i, j - 1].Piece.Color == color.eWhite))
                {
                    possitionEnpassant = true;
                }
            }

            if (from.Row == 6 && to.Row == 4 && canBeEnpassant)
            {
                int i = to.Row;
                int j = to.Column;
                if ((checkIsOnTable(i, j + 1) && tempTable[i, j + 1].Piece != null &&
                    tempTable[i, j + 1].Piece.Name == piece.ePawn && tempTable[i, j + 1].Piece.Color == color.eBrown) ||
                    (checkIsOnTable(i, j - 1) && tempTable[i, j - 1].Piece != null &&
                    tempTable[i, j - 1].Piece.Name == piece.ePawn && tempTable[i, j - 1].Piece.Color == color.eBrown))
                {
                    possitionEnpassant = true;
                }
            }

        }
    }
}
