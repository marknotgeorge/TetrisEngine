using System;

namespace TetrisEngine
{
    public class Board
    {
        /// <summary>
        /// THe underlying board that contains the pieces
        /// </summary>
        private BoardBlock[,] boardArray;
        
        // <summary>
        /// The number of Blocks in a Piece
        /// </summary>/
        private int pieceBlocks = 5;
        
        public int Width { get; set; }
        public int Height { get; set; }
        
        public Board(int width, int height)
        {
            Width = width;
            Height = height; 

            // Create the underlying board array
            boardArray = new BoardBlock[width, height];
            initBoard();
        }

        /// <summary>
        /// Fill the underlying board array with Empty blocks
        /// </summary>
        private void initBoard()
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    boardArray[i,j] = BoardBlock.Empty;
                }
            }
        }

        /// <summary>
        /// Store a piece in the array in a specified position.
        /// </summary>
        /// <param name="piece">The piece to store</param>
        /// <param name="x">Horizontal offset in blocks</param>
        /// <param name="y">Vertical offset in blocks</param>
        public void StorePiece(Piece piece)
        {
            var x = piece.PosX;
            var y = piece.PosY;

            for (var i = 0; i < pieceBlocks; i++)
            {
                for (var j = 0; j < pieceBlocks; j++)
                {
                    // Only store the blocks that are not empty, using the 
                    // piece's type to denote the colour.
                    if(piece.GetBlock(i,j) != PieceBlock.NoBlock)
                    {
                        boardArray[x + i, y + j] = (BoardBlock)piece.Type;
                    }
                }
            }
        }

        /// <summary>
        /// Check to see if the game is over (one of the blocks in row 0 is not Empty)
        /// </summary>
        /// <returns>True if one of the blocks in row 0 is not Empty </returns>
        public bool IsGameOver()
        {
            for (var i = 0; i < Width; i++)
            {
                if (boardArray[i,0] != BoardBlock.Empty)
                {
                    return true;
                }                
            }
            return false;
        }

        /// <summary>
        /// Delete a line of the board by moving all the lines down
        /// </summary>
        /// <param name="row"> The row to delete</param>
        private void deleteRow(int row)
        {
            // Move all lines above row down
            for (var j = row; j > 0; j--)
            {
                for (var i = 0; i < Width; i++)
                {
                    boardArray[i, j] = boardArray[i, j - 1];
                }
            }
        }

        /// <summary>
        /// Delete all possible rows
        /// </summary>
        public void DeleteAllPossibleRows()
        {
            for (var j = 0; j < Height; j++)
            {
                var i = 0;
                while (i < Width)
                {
                    if (boardArray[i,j] == BoardBlock.Empty)
                    {
                        break;
                    }
                    i++;
                }
                if (i == Width)
                {
                    deleteRow(j);
                }
            }
        }

        /// <summary>
        /// Is the block at (x,y) Empty?
        /// </summary>
        /// <param name="x">Horizontal position in blocks</param>
        /// <param name="y">Vertical position in blocks</param>
        /// <returns></returns>
        public bool IsFreeBlock(int x, int y)
        {
            return (boardArray[x, y] == BoardBlock.Empty);
        }
        
        public bool IsMovementPossible(Piece piece, ButtonPressed buttonPressed)
        {
            // Get prospective new position
            var newPosition = piece;
            switch (buttonPressed)
            {
                case ButtonPressed.Left:
                    newPosition.PosX--;
                    break;
                case ButtonPressed.Right:
                    newPosition.PosX++;
                    break;
                case ButtonPressed.Down:
                case ButtonPressed.Drop:
                    newPosition.PosY++;
                    break;
                case ButtonPressed.Rotate:
                    newPosition.Rotation = (byte)((newPosition.Rotation + 1) % 4);
                    break;
            }           
            
            
            // Test prospective new position...
            for (var i = 0; i < pieceBlocks; i++)
            {
                for (var j = 0; j < pieceBlocks; j++)
                {
                    if (piece.GetBlock(i, j) != PieceBlock.NoBlock)
                    {
                        var newX = i + newPosition.PosX;
                        var newY = j + newPosition.PosY;

                        // Check if block is outside limits
                        if ((newX < 0) || (newX > (Width-1)) || (newY > (Height - 1)))
                        {
                            return false;
                        }

                        // Check if position is not empty
                        if ((newY >=0) && (boardArray[newX, newY] != BoardBlock.Empty))
                        {
                            return false;
                        }
                    }
                }
            }

            // No collision
            return true;
        }

        
    }

    public enum BoardBlock
    {
        Empty = -1,
        Indigo,
        Oscar,
        Tango,
        Sierra,
        Zulu,
        Juliet, 
        Lima
    }


}