using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ActivePlayers
{
    private static Dictionary<int, int> _playerScores = new Dictionary<int, int>();
    private static List<PlayerInfo> _activePlayers = new List<PlayerInfo>();

    public static List<PlayerInfo> Players
    {
        get
        {
            return _activePlayers;
        }
    }

    public static void Reset()
    {
        _activePlayers = new List<PlayerInfo>();
        _playerScores = new Dictionary<int, int>();
    }

    public static void AddPlayer(Player player)
    {
        _activePlayers.Add(new PlayerInfo(player));
        if (_playerScores.FirstOrDefault(p => p.Key == player.PlayerNumber).Equals(default(KeyValuePair<int, int>)))
        {
            _playerScores.Add(player.PlayerNumber, 0);
        }
    }

    public static void RemovePlayer(Player player)
    {
        _activePlayers.RemoveAll(p => p.ControllerNumber == player.PlayerNumber);
        _playerScores.Remove(player.PlayerNumber);
    }

    public static void InitializeAllPlayers()
    {
        if( _playerScores.Count > 0)
        {
            return;
        }

        for(int i=1; i<=4; i++)
        {
            _playerScores.Add(i, 0);
            _activePlayers.Add(new PlayerInfo(i));
        }
    }

    public static void IncrementScore(int playerIndex)
    {
        _playerScores[playerIndex]++;
    }

    public static int GetScore(int playerIndex)
    {
        return _playerScores[playerIndex];
    }

    public static void ResetScores()
    {
        foreach (var key in _playerScores.Keys.ToList())
        {
            _playerScores[key] = 0;
        }
    }

    public static void UpdateMaterial(int playerNumber, Material material)
    {
        if (_activePlayers.FirstOrDefault(p => p.ControllerNumber == playerNumber) != null)
        {
            _activePlayers.First(p => p.ControllerNumber == playerNumber).Material = material;
        }
    }
}

public class PlayerInfo
{
    public int ControllerNumber;
    public Material Material;

    public PlayerInfo(Player player)
    {
        ControllerNumber = player.PlayerNumber;
        Material = player.Material;
    }

    public PlayerInfo(int controllerNumber)
    {
        ControllerNumber = controllerNumber;
    }
}
