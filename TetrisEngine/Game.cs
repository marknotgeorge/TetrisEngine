using System;

namespace TetrisEngine
{
    public class Game
    {
        private IHardware _hardware;
        private Board board;
        public int Width { get; set; }
        public int Height { get; set; }
        private Piece currentPiece;
        private Piece nextPiece;

        private long waitTime = 700;

        public Game(IHardware hardware, int width = 10, int height = 20)
        {
            _hardware = hardware;
            Width = width;
            Height = height;
            board = new Board(Width, Height);

            CreateNewPiece(true);
            StartGameLoop();
        }

        public void CreateNewPiece(bool createBoth = false)
        {
            var randGen = new Random();
            var typeCount = Enum.GetNames(typeof(PieceType)).Length;
            var rotationCount = 4;

            if (createBoth)
            {
                // Create new current piece
                currentPiece = Piece.GetPiece(
                    (PieceType)randGen.Next(0, typeCount),
                    (byte)randGen.Next(0, rotationCount),
                    Width
                );
            }
            else
            {
                // Move nextPiece to currentPiece.
                var initPos = nextPiece.GetInitialPosition();
                currentPiece = nextPiece;
                currentPiece.PosX = (Width / 2) + initPos.X;
                currentPiece.PosY = initPos.Y;
            }

            nextPiece = Piece.GetPiece(
                (PieceType)randGen.Next(0, typeCount),
                (byte)randGen.Next(0, rotationCount),
                Width,
                Width + 5,
                5
            );
        }

        public void StartGameLoop()
        {
            var time1 = DateTime.Now.Ticks;

            while (!_hardware.IsButtonPressed(ButtonPressed.Select))
            {
                // Draw
                _hardware.ClearScreen();
                drawScene();
                _hardware.UpdateScreen();

                // Input
                var buttonPressed = _hardware.GetButtonPressed();

                switch (buttonPressed)
                {
                    case ButtonPressed.Right:
                        if (board.IsMovementPossible(currentPiece, buttonPressed))
                        {
                            currentPiece.PosX++;
                        }
                        break;
                    case ButtonPressed.Left:
                        if (board.IsMovementPossible(currentPiece, buttonPressed))
                        {
                            currentPiece.PosX--;
                        }
                        break;
                    case ButtonPressed.Down:
                        if (board.IsMovementPossible(currentPiece, buttonPressed))
                        {
                            currentPiece.PosY++;
                        }
                        break;
                    case ButtonPressed.Drop:
                        while (board.IsMovementPossible(currentPiece, buttonPressed))
                        {
                            currentPiece.PosY++;
                        }
                        currentPiece.PosY--;
                        board.StorePiece(currentPiece);
                        board.DeleteAllPossibleRows();

                        if (board.IsGameOver())
                        {
                            return;
                        }
                        CreateNewPiece();
                        break;
                    case ButtonPressed.Rotate:
                        if (board.IsMovementPossible(currentPiece, buttonPressed))
                        {
                            currentPiece.Rotation = (byte)((currentPiece.Rotation + 1) % 4);
                        };
                        break;
                }

                // Vertical movement
                var time2 = DateTime.Now.Ticks;

                if ((time2 - time1) > waitTime)
                {
                    if (board.IsMovementPossible(currentPiece, ButtonPressed.Down))
                    {
                        currentPiece.PosY++;
                    }
                    else
                    {
                        board.StorePiece(currentPiece);
                        board.DeleteAllPossibleRows();

                        if (board.IsGameOver())
                        {
                            return;
                        }

                        CreateNewPiece();
                    }
                    time1 = DateTime.Now.Ticks;
                }
            }
        }

        private void drawScene()
        {
            _hardware.DrawBoard(board);
            _hardware.DrawPiece(currentPiece);
            _hardware.DrawPiece(nextPiece);
        }
    }
}