using System.Drawing;

namespace TetrisEngine
{
    public interface IHardware
    {
        (int x, int y) BoardOriginInPixels();
        int BlockSizeInPixels();
        int BoardWidthInPixels();
        int BoardHeightInPixels();

        bool IsButtonPressed(ButtonPressed button);

        ButtonPressed GetButtonPressed();
        void ClearScreen();
        void UpdateScreen();

        void DrawPiece(Piece piece);
        void DrawBoard(Board board);
    }

    public enum ButtonPressed
    {
        Left,
        Right,
        Down,
        Drop,
        Rotate,
        Select,
        Start
    }
}