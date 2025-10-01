using System.ComponentModel;
using ModelContextProtocol.Server;
using ChessMCP.Services;
using ChessMCP.Models;

namespace ChessMCP.Tools;

public class ChessTools(ChessStatsService chessStatsService)
{
    private readonly ChessStatsService _chessStatsService = chessStatsService;

    [McpServerTool(Name = "get_chess_player_stats")]
    [Description("Gets comprehensive chess statistics for a player from Chess.com including ratings, records, and performance metrics across different time controls")]
    public async Task<ChessPlayerStatsResult> GetChessPlayerStatsAsync(
        [Description("The Chess.com username of the player to get stats for")] string username,
        CancellationToken cancellationToken = default)
    {
        var stats = await _chessStatsService.GetPlayerStatsAsync(username, cancellationToken);
        
        if (stats == null)
        {
            return new ChessPlayerStatsResult
            {
                Username = username,
                Success = false,
                Error = "No statistics found for this player"
            };
        }

        return new ChessPlayerStatsResult
        {
            Username = username,
            Success = true,
            Stats = stats,
            Summary = CreateStatsSummary(stats)
        };
    }

    //Add a tool that gets the clubs of a player from chess.com
    //https://api.chess.com/pub/player/{username}/clubs

    //Add a tool that gets the live daily games of a player from chess.com
    //https://api.chess.com/pub/player/{username}/games

    private static string CreateStatsSummary(ChessPlayerStats stats)
    {
        var summary = new List<string>
        {
            $"Chess Player Statistics Summary:"
        };

        if (stats.ChessRapid?.Last != null)
        {
            summary.Add($"• Rapid: {stats.ChessRapid.Last.RatingValue} (W:{stats.ChessRapid.Record?.Win} L:{stats.ChessRapid.Record?.Loss} D:{stats.ChessRapid.Record?.Draw})");
        }

        if (stats.ChessBlitz?.Last != null)
        {
            summary.Add($"• Blitz: {stats.ChessBlitz.Last.RatingValue} (W:{stats.ChessBlitz.Record?.Win} L:{stats.ChessBlitz.Record?.Loss} D:{stats.ChessBlitz.Record?.Draw})");
        }

        if (stats.ChessBullet?.Last != null)
        {
            summary.Add($"• Bullet: {stats.ChessBullet.Last.RatingValue} (W:{stats.ChessBullet.Record?.Win} L:{stats.ChessBullet.Record?.Loss} D:{stats.ChessBullet.Record?.Draw})");
        }

        if (stats.ChessDaily?.Last != null)
        {
            summary.Add($"• Daily: {stats.ChessDaily.Last.RatingValue} (W:{stats.ChessDaily.Record?.Win} L:{stats.ChessDaily.Record?.Loss} D:{stats.ChessDaily.Record?.Draw})");
        }

        if (stats.Tactics?.Highest != null)
        {
            summary.Add($"• Tactics: {stats.Tactics.Highest.Rating} (highest)");
        }

        if (stats.PuzzleRush?.Best != null)
        {
            summary.Add($"• Puzzle Rush: {stats.PuzzleRush.Best.Score} (best score)");
        }

        return string.Join("\n", summary);
    }
}

public class ChessPlayerStatsResult
{
    public string Username { get; set; } = "";
    public bool Success { get; set; }
    public string? Error { get; set; }
    public ChessPlayerStats? Stats { get; set; }
    public string? Summary { get; set; }
}




