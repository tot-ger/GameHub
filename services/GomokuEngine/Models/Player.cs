using GameHub.GomokuEngine.Enums;

namespace GameHub.GomokuEngine.Models;

public class Player(string connectionId, string name, bool isAI, int symbol)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ConnectionId { get; set; } = connectionId;
    public string Name { get; set; } = name;
    public bool IsHost { get; set; }
    public bool IsAI { get; set; } = isAI;
    public int Score { get; set; } = 0;
    public int Symbol { get; set; } = symbol;
    public PlayerState State { get; set; } = isAI ? PlayerState.Ready : PlayerState.Waiting;
}