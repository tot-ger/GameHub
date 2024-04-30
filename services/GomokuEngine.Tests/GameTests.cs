using GameHub.GomokuEngine.Enums;
using GameHub.GomokuEngine.Models;

namespace GameHub.GomokuEngine.Tests;

public class GameTests
{
    [Fact]
    public void AddPlayer_WhenGameIsFull_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.AddPlayer(new Player("3", "Player 3", false, 1)));
    }

    [Fact]
    public void AddPlayer_WhenPlayerIsAlreadyInGame_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.AddPlayer(new Player("1", "Player 1", false, 1)));
    }

    [Fact]
    public void RemovePlayer_WhenPlayerIsNotInGame_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.RemovePlayer("2"));
    }

    [Fact]
    public void Start_WhenGameIsNotFull_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.Start());
    }

    [Fact]
    public void Start_WhenGameIsFull_StartsGame()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        //Assert
        game.Start();
        Assert.Equal(GameState.InProgress, game.State);
    }

    [Fact]
    public void Start_WhenGameIsFull_SetsFirstPlayerStateToThinking()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        //Assert
        game.Start();
        Assert.Equal(PlayerState.Thinking, game.Players[0].State);
    }

    [Fact]
    public void Start_WhenGameIsFull_SetsSecondPlayerStateToPlaying()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        //Assert
        game.Start();
        Assert.Equal(PlayerState.Playing, game.Players[1].State);
    }

    [Fact]
    public void Start_WhenGameHasAlreadyStarted_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.Start());
    }

    [Fact]
    public void MakeMove_WhenGameHasNotStarted_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.MakeMove(1, 0, new Guid()));
    }

    [Fact]
    public void MakeMove_WhenGameHasEnded_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.End();
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.MakeMove(1, 0, game.Players[0].Id));
    }

    [Fact]
    public void MakeMove_WhenPlayerIsNotPlaying_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.MakeMove(1, 0, game.Players[1].Id));
    }

    [Fact]
    public void MakeMove_WhenPlayerIsPlaying_MakesMove()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        //Assert
        game.MakeMove(1, 0, game.Players[0].Id);
        Assert.Equal(1, game.Board.Cells[1, 0]);
    }

    [Fact]
    public void MakeMove_WhenPlayerIsPlaying_ReturnsTrue()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        //Assert
        Assert.True(game.MakeMove(1, 0, game.Players[0].Id));
    }

    [Fact]
    public void MakeMove_WhenPlayerIsPlayingAndCellIsNotEmpty_ReturnsFalse()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.MakeMove(1, 0, game.Players[0].Id);
        //Assert
        Assert.False(game.MakeMove(1, 0, game.Players[1].Id));
    }

    [Fact]
    public void MakeMove_WhenPlayerIsPlayingAndCoordinatesAreOutOfBounds_ThrowsArgumentOutOfRangeException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        //Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => game.MakeMove(15, 15, game.Players[0].Id));
    }

    [Fact]
    public void MakeMove_WhenMakesMove_ChecksForWinningMove_Vertically()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.MakeMove(1, 0, game.Players[0].Id);
        game.MakeMove(1, 1, game.Players[1].Id);
        game.MakeMove(2, 0, game.Players[0].Id);
        game.MakeMove(1, 2, game.Players[1].Id);
        game.MakeMove(3, 0, game.Players[0].Id);
        game.MakeMove(1, 3, game.Players[1].Id);
        game.MakeMove(4, 0, game.Players[0].Id);
        game.MakeMove(1, 4, game.Players[1].Id);
        game.MakeMove(5, 0, game.Players[0].Id);
        //Assert
        Assert.Equal(GameState.Finished, game.State);
    }

    [Fact]
    public void MakeMove_WhenMakesMove_ChecksForWinningMove_Horizontally()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.MakeMove(0, 0, game.Players[0].Id);
        game.MakeMove(1, 1, game.Players[1].Id);
        game.MakeMove(1, 0, game.Players[0].Id);
        game.MakeMove(2, 1, game.Players[1].Id);
        game.MakeMove(2, 0, game.Players[0].Id);
        game.MakeMove(3, 1, game.Players[1].Id);
        game.MakeMove(3, 0, game.Players[0].Id);
        game.MakeMove(4, 1, game.Players[1].Id);
        game.MakeMove(4, 0, game.Players[0].Id);
        //Assert
        Assert.Equal(GameState.Finished, game.State);
    }

    [Fact]
    public void MakeMove_WhenMakesMove_ChecksForWinningMove_DiagonallyFromTopLeftToBottomRight()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.MakeMove(0, 0, game.Players[0].Id);
        game.MakeMove(1, 1, game.Players[1].Id);
        game.MakeMove(1, 0, game.Players[0].Id);
        game.MakeMove(2, 1, game.Players[1].Id);
        game.MakeMove(2, 0, game.Players[0].Id);
        game.MakeMove(3, 1, game.Players[1].Id);
        game.MakeMove(3, 0, game.Players[0].Id);
        game.MakeMove(4, 1, game.Players[1].Id);
        game.MakeMove(4, 2, game.Players[0].Id);
        game.MakeMove(5, 1, game.Players[1].Id);
        //Assert
        Assert.Equal(GameState.Finished, game.State);
    }

    [Fact]
    public void MakeMove_WhenMakesMove_ChecksForWinningMove_DiagonallyFromTopRightToBottomLeft()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.MakeMove(0, 4, game.Players[0].Id);
        game.MakeMove(1, 1, game.Players[1].Id);
        game.MakeMove(1, 3, game.Players[0].Id);
        game.MakeMove(2, 1, game.Players[1].Id);
        game.MakeMove(2, 2, game.Players[0].Id);
        game.MakeMove(6, 1, game.Players[1].Id);
        game.MakeMove(3, 1, game.Players[0].Id);
        game.MakeMove(4, 1, game.Players[1].Id);
        game.MakeMove(4, 0, game.Players[0].Id);
        //Assert
        Assert.Equal(GameState.Finished, game.State);
    }

    [Fact]
    public void MakeMove_WhenWinner_UpdatesScore()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.MakeMove(0, 0, game.Players[0].Id);
        game.MakeMove(1, 1, game.Players[1].Id);
        game.MakeMove(1, 0, game.Players[0].Id);
        game.MakeMove(2, 1, game.Players[1].Id);
        game.MakeMove(2, 0, game.Players[0].Id);
        game.MakeMove(3, 1, game.Players[1].Id);
        game.MakeMove(3, 0, game.Players[0].Id);
        game.MakeMove(4, 1, game.Players[1].Id);
        game.MakeMove(4, 0, game.Players[0].Id);
        //Assert
        Assert.Equal(1, game.Players[0].Score);
    }

    [Fact]
    public void MakeMove_WhenMakesMove_ChecksForDraw()
    {
        //Arrange
        var game = new Game(3, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.MakeMove(0, 0, game.Players[0].Id);
        game.MakeMove(0, 1, game.Players[1].Id);
        game.MakeMove(0, 2, game.Players[0].Id);
        game.MakeMove(1, 0, game.Players[1].Id);
        game.MakeMove(1, 1, game.Players[0].Id);
        game.MakeMove(1, 2, game.Players[1].Id);
        game.MakeMove(2, 0, game.Players[0].Id);
        game.MakeMove(2, 1, game.Players[1].Id);
        game.MakeMove(2, 2, game.Players[0].Id);
        //Assert
        Assert.Equal(GameState.Finished, game.State);
    }

    [Fact]
    public void End_WhenGameHasNotStarted_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.End());
    }

    [Fact]
    public void End_WhenGameHasAlreadyEnded_ThrowsInvalidOperationException()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        game.End();
        //Assert
        Assert.Throws<InvalidOperationException>(() => game.End());
    }

    [Fact]
    public void End_WhenGameHasStarted_EndsGame()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", false, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        //Assert
        game.End();
        Assert.Equal(GameState.Finished, game.State);
    }

    [Fact]
    public void End_WhenGameHasStarted_ResetsPlayersState()
    {
        //Arrange
        var game = new Game(15, true);
        //Act
        game.AddPlayer(new Player("1", "Player 1", true, 1));
        game.AddPlayer(new Player("2", "Player 2", false, 2));
        game.Start();
        //Assert
        game.End();
        Assert.All(game.Players, p => Assert.Equal(p.IsAI ? PlayerState.Ready : PlayerState.Waiting, p.State));
    }
}