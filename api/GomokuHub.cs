using GameHub.GomokuEngine;
using GameHub.GomokuEngine.Enums;
using GameHub.GomokuEngine.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameHub.API;

public class GomokuHub(GomokuManager gomokuManager) : Hub
{
  private readonly GomokuManager _gomokuManager = gomokuManager;

  public async Task JoinLobby()
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, "Lobby");
  }

  public async Task GetPublicGames()
  {
    var games = _gomokuManager.GetGames()
                              .Where(g => g.IsPublic && g.Players.Count == 1)
                              .Select(g => new { id = g.Id, player = g.Players[0].Name})
                              .ToList();
    await Clients.Caller.SendAsync("PublicGames", games);
  }

  public async Task GetGameById(string gameId)
  {
    var game = _gomokuManager.GetGame(gameId);
    if (game == null)
    {
      await Clients.Caller.SendAsync("GameNotFound");
      return;
    }

    await Clients.Caller.SendAsync("Game", game);
  }

  public async Task CreateGame(int size, bool isPublic)
  {
    var game = _gomokuManager.CreateGame(size, isPublic);
    if (game != null)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());
      if (isPublic)
      {
        await Clients.Group("Lobby").SendAsync("GameCreated", game);
      }
    }
  }

  public async Task JoinGame(string gameId, string name)
  {
    var game = _gomokuManager.GetGame(gameId);
    if (game == null)
    {
      await Clients.Caller.SendAsync("GameNotFound");
      return;
    }

    if (game.Players.Count == 2)
    {
      await Clients.Caller.SendAsync("GameFull");
      return;
    }

    var player = new Player(Context.ConnectionId, name, false, game.Players.Count + 1);
    game.AddPlayer(player);

    await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    await Clients.Group(gameId).SendAsync("PlayerJoined", player);
  }

public async Task UpdatePlayerState(string gameId, PlayerState state)
{
  var game = _gomokuManager.GetGame(gameId);
  if (game == null)
  {
    await Clients.Caller.SendAsync("GameNotFound");
    return;
  }

  var player = game.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
  if (player == null)
  {
    await Clients.Caller.SendAsync("PlayerNotFound");
    return;
  }

  player.State = state;

  if (state == PlayerState.Ready && game.Players.All(p => p.State == PlayerState.Ready))
  {
    await StartGame(gameId);
  }

  await Clients.Group(gameId).SendAsync("PlayerStateUpdated", player);
}

  public async Task LeaveGame(string gameId)
  {
    var game = _gomokuManager.GetGame(gameId);
    if (game == null)
    {
      await Clients.Caller.SendAsync("GameNotFound");
      return;
    }

    game.RemovePlayer(Context.ConnectionId);

    await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
    await Clients.Group(gameId).SendAsync("PlayerLeft", Context.ConnectionId);
  }

  public async Task StartGame(string gameId)
  {
    var game = _gomokuManager.GetGame(gameId);
    if (game == null)
    {
      await Clients.Caller.SendAsync("GameNotFound");
      return;
    }

    game.Start();

    await Clients.Group(gameId).SendAsync("GameStarted", game);
  }

  public async Task MakeMove(string gameId, int x, int y)
  {
    var game = _gomokuManager.GetGame(gameId);
    if (game == null)
    {
      await Clients.Caller.SendAsync("GameNotFound");
      return;
    }

    game.MakeMove(x, y, game.CurrentPlayer.Id);

    await Clients.Group(gameId).SendAsync("MoveMade", game);
  }
}