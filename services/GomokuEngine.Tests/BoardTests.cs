using GameHub.GomokuEngine.Models;

namespace GameHub.GomokuEngine.Tests;

public class BoardTests
{
    [Fact]
    public void Board_Initialization_AllCellsShouldBeEmpty()
    {
      var board = new Board(15);
      
      for (var i = 0; i < 15; i++)
      {
        for (var j = 0; j < 15; j++)
        {
          Assert.Equal(0, board.Cells[i, j]);
        }
      }
    }

    [Fact]
    public void SetCell_ValidCoordinates_SetsValue()
    {
      var board = new Board(15);
      
      Assert.True(board.SetCell(0, 0, 1));
      
      Assert.Equal(1, board.Cells[0, 0]);
    }

    [Fact]
    public void SetCell_InvalidCoordinates_ThrowsArgumentOutOfRangeException()
    {
      var board = new Board(15);
      
      Assert.Throws<ArgumentOutOfRangeException>(() => board.SetCell(15, 15, 1));
    }

    [Fact]
    public void SetCell_CellIsNotEmpty_ReturnsFalse()
    {
      var board = new Board(15);
      
      board.SetCell(0, 0, 1);
      
      Assert.False(board.SetCell(0, 0, 2));
    }

    [Fact]
    public void IsCellEmpty_CellIsEmpty_ReturnsTrue()
    {
      var board = new Board(15);
      
      Assert.True(board.IsCellEmpty(0, 0));
    }

    [Fact]
    public void IsCellEmpty_CellIsNotEmpty_ReturnsFalse()
    {
      var board = new Board(15);
      
      board.SetCell(0, 0, 1);
      
      Assert.False(board.IsCellEmpty(0, 0));
    }

    [Fact]
    public void IsWithInBounds_ValidCoordinates_ReturnsTrue()
    {
      var board = new Board(15);
      
      Assert.True(board.IsWithInBounds(0, 0));
    }

    [Fact]
    public void IsWithInBounds_InvalidCoordinates_ReturnsFalse()
    {
      var board = new Board(15);
      
      Assert.False(board.IsWithInBounds(15, 15));
    }
}