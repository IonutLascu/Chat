using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class CommonPiece : Piece
    {
        public CommonPiece(piece p, color c) : base(p, c) { }

        //possible moves rook
        protected void populateRowLowerThanIndex(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                i++;
                while (i < 8)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    i++;
                }
            }
        }
        protected void populateRowGreatherThanIndex(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                i--;
                while (i > -1)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    i--;
                }
            }
        }
        protected void populateColumnLowerThanIndex(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                j++;
                while (j < 8)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    j++;
                }
            }
        }
        protected void populateColumnGreatherThanIndex(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                j--;
                while (j > -1)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    j--;
                }
            }
        }

        //possbile moves bishop
        protected void populateDiagonalBottom_Right(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                i++; j++;
                while (i < 8 && j < 8)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    i++; j++;
                }
            }
        }
        protected void populateDiagonalBottom_Left(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                i++; j--;
                while (i < 8 && j > -1)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    i++; j--;
                }
            }
        }
        protected void populateDiagonalTop_Right(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                i--; j++;
                while (i > -1 && j < 8)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    i--; j++;
                }
            }
        }
        protected void populateDiagonalTop_Left(int i, int j, color c, ref List<Tuple<int, int>> arrPosibleMoves)
        {
            Square[,] tempTable = Table.table;
            if (checkIsOnTable(i, j))
            {
                i--; j--;
                while (i > -1 && j > -1)
                {
                    if (tempTable[i, j].Piece == null)
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name != piece.eKing)
                    {
                        arrPosibleMoves.Add(new Tuple<int, int>(i, j));
                        break;
                    }
                    else if (tempTable[i, j].Piece.Color == c && tempTable[i, j].Piece.Name == piece.eKing)
                    {
                        if (c == color.eBrown)
                            King.isKingBrawnInChess = true;
                        else King.isKingWhiteInChess = true;
                        break;
                    }
                    else
                        break;

                    i--; j--;
                }
            }
        }

    }
}
