using System.Collections.Concurrent;
using GameHub.GomokuEngine.Models;

namespace GameHub.GomokuEngine;

public class GomokuManager
{
    private ConcurrentDictionary<string, Game> _games = new();

    public Game? CreateGame(int size, bool isPublic)
    {
        var game = new Game(size, isPublic);
        if (_games.TryAdd(game.Id.ToString(), game))
        {
            return game;
        }
        return null;
    }

    public Game? GetGame(string gameId)
    {
        if (_games.TryGetValue(gameId, out var game))
        {
            return game;
        }
        return null;
    }

    public bool RemoveGame(string gameId)
    {
        return _games.TryRemove(gameId, out _);
    }

    public List<Game> GetGames()
    {
        return _games.Values.ToList();
    }
}