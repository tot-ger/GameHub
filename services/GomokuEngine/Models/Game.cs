using GameHub.GomokuEngine.Enums;

namespace GameHub.GomokuEngine.Models;

public class Game(int size, bool isPublic)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Board Board { get; set; } = new Board(size);
    public List<Player> Players { get; set; } = [];
    public Player CurrentPlayer => Players[(Turn - 1) % Players.Count];
    public int Round { get; set; } = 1;
    public int Turn { get; set; } = 1;
    public GameState State { get; set; } = GameState.WaitingForPlayers;
    public bool IsPublic { get; set; } = isPublic;

    public void AddPlayer(Player player)
    {
        if (State != GameState.WaitingForPlayers)
        {
            throw new InvalidOperationException("Game has already started");
        }

        if (Players.Count == 2)
        {
            throw new InvalidOperationException("Game is full");
        }

        if (Players.Any(p => p.ConnectionId == player.ConnectionId))
        {
            throw new InvalidOperationException("Player is already in the game");
        }

        if (player.IsAI)
        {
          player.State = PlayerState.Ready;
        }
        else
        {
          player.State = PlayerState.Waiting;
        }

        Players.Add(player);
    }

    public void RemovePlayer(string connectionId)
    {
        var player = Players.FirstOrDefault(p => p.ConnectionId == connectionId) ?? throw new InvalidOperationException("Player is not in the game");
        Players.Remove(player);
        State = GameState.WaitingForPlayers;
    }

    public void Start()
    {
        if (State != GameState.WaitingForPlayers)
        {
            throw new InvalidOperationException("Game has already started");
        }

        if (Players.Count != 2)
        {
            throw new InvalidOperationException("Game is not full");
        }
        State = GameState.InProgress;
        Players[0].State = PlayerState.Thinking;
        Players[1].State = PlayerState.Playing;
    }

    public void End()
    {
        if (State != GameState.InProgress)
        {
            throw new InvalidOperationException("Game is not in progress");
        }

        Players.ForEach(p =>
        {
            p.State = p.IsAI ? PlayerState.Ready : PlayerState.Waiting;
        });
        State = GameState.Finished;
    }

    public bool MakeMove(int x, int y, Guid playerId)
    {
        if (State != GameState.InProgress)
        {
            throw new InvalidOperationException($"Game is not in progress. State: {State}");
        }

        if (CurrentPlayer.Id != playerId)
        {
            throw new InvalidOperationException("It's not this player's turn");
        }

        if (Board.SetCell(x, y, CurrentPlayer.Symbol))
        {
            Console.WriteLine($"Player {CurrentPlayer.Name} placed a piece at ({x}, {y})");
            if (CheckForWin(x, y, CurrentPlayer.Symbol))
            {
                Console.WriteLine("Winner");
                SetWinner();
                End();
            }
            else if (CheckForDraw())
            {
                End();
            }
            else
            {
                NextTurn();
            }

            return true;
        }
        return false;
    }

    private void NextTurn()
    {
        Players.ForEach(p =>
        {
            if (p.State == PlayerState.Playing)
            {
                p.State = PlayerState.Thinking;
            }
            else if (p.State == PlayerState.Thinking)
            {
                p.State = PlayerState.Playing;
            }
        });
        Turn++;
    }

    private bool CheckForWin(int x, int y, int symbol)
    {
        return (CountConsecutive(x, y, symbol, 1, 0) + CountConsecutive(x, y, symbol, -1, 0) == 4) ||
               (CountConsecutive(x, y, symbol, 0, 1) + CountConsecutive(x, y, symbol, 0, -1) == 4) ||
               (CountConsecutive(x, y, symbol, 1, 1) + CountConsecutive(x, y, symbol, -1, -1) == 4) ||
               (CountConsecutive(x, y, symbol, 1, -1) + CountConsecutive(x, y, symbol, -1, 1) == 4);
    }

    private int CountConsecutive(int x, int y, int symbol, int dx, int dy)
    {
        int count = 0;
        int cx = x + dx;
        int cy = y + dy;

        while (Board.IsWithInBounds(cx, cy) && Board.Cells[cx, cy] == symbol)
        {
            count++;
            cx += dx;
            cy += dy;
        }

        return count;
    }

    private bool CheckForDraw()
    {
        return Board.Cells.Cast<int>().All(cell => cell != 0);
    }

    private void SetWinner()
    {
        CurrentPlayer.Score++;
    }
}