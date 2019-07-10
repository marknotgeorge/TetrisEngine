namespace TetrisEngine
{
    public class Piece
    {
        // [Rotation][Horizontal][Vertical]
        internal int[,,] blocks;
        // [Horizontal][Vertical]
        internal int[,] initialTransform;

        public PieceType Type { get; set; }
        public byte Rotation { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public PieceColor Color { get; set; }

        public static Piece GetPiece(PieceType type, byte rotation, int boardWidth, int x = -1, int y = -1)
        {
            Piece returnPiece = null;
            switch (type)        
            {
                case PieceType.Indigo:
                    returnPiece = new IndigoPiece();
                    break;
                case PieceType.Oscar:
                    returnPiece =  new OscarPiece();
                    break;
                case PieceType.Tango:
                    returnPiece =  new TangoPiece();
                    break;
                case PieceType.Sierra:
                    returnPiece = new SierraPiece();
                    break;
                case PieceType.Zulu:
                    returnPiece = new ZuluPiece();
                    break;
                case PieceType.Juliet:
                    returnPiece = new JulietPiece();
                    break;
                case PieceType.Lima:
                    returnPiece = new LimaPiece();
                    break;
            }

            returnPiece.Rotation = rotation;

            // If x or y are -1, this is the initial position.
            if (x == -1)
            {
                var initialPos = returnPiece.GetInitialPosition();
                returnPiece.PosX = (boardWidth / 2) + initialPos.X;                
            }
            else
            {
                 returnPiece.PosX = x;
            }

            if (y == -1)
            {
                var initialPos = returnPiece.GetInitialPosition();
                returnPiece.PosY = initialPos.Y;
            }
            else
            {
                returnPiece.PosY = y;
            }

            return returnPiece;
        }

        public PieceBlock GetBlock(int x, int y)
        {
            return (PieceBlock)blocks[(int)Rotation, x, y];
        }

        public PiecePosition GetInitialPosition()
        {
            return new PiecePosition(initialTransform[(int)Rotation, 0],
                initialTransform[(int)Rotation, 1]);
        }
    }

    public struct PiecePosition
    {
        public int X;
        public int Y;

        public PiecePosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public enum PieceType
    {
        Indigo = 0,
        Oscar,
        Tango,
        Sierra,
        Zulu,
        Juliet, 
        Lima
    }

    public enum PieceColor: ushort
    {
        Indigo = 0x4810,
        Oscar =  0x0520,
        Tango = 0xFD20,
        Sierra = 0xF800,
        Zulu = 0x435C,
        Juliet = 0xFFE0, 
        Lima = 0xCD07
    }       

    public enum PieceBlock
    {
        NoBlock = 0,
        Normal=1,
        Pivot=2
    }
}
