
namespace GameHub.GomokuEngine.Models;

public class Board
{
    public int Size { get; set; }
    public int[,] Cells { get; set; } = default!;

    public Board(int size)
    {
        Size = size;
        Cells = new int[size, size];
        InitializeCells();
    }

    public bool IsCellEmpty(int x, int y)
    {
        if (!IsWithInBounds(x, y))
        {
            throw new ArgumentOutOfRangeException("x, y", "Coordinates are out of bounds");
        }
        return Cells[x, y] == 0;
    }

    public bool SetCell(int x, int y, int value)
    {
        if (!IsWithInBounds(x, y))
        {
            throw new ArgumentOutOfRangeException("x, y", "Coordinates are out of bounds");
        }

        if (!IsCellEmpty(x, y))
        {
            return false;
        }

        Cells[x, y] = value;

        return true;
    }

    public bool IsWithInBounds(int x, int y)
    {
        return x >= 0 && x < Size && y >= 0 && y < Size;
    }

    private void InitializeCells()
    {
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                Cells[i, j] = 0;
            }
        }
    }
}
    