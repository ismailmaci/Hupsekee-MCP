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

    [McpServerTool(Name = "get_chess_player_clubs")]
    [Description("Gets the list of chess clubs that a player is a member of from Chess.com, including club names, join dates, and activity information")]
    public async Task<ChessPlayerClubsResult> GetChessPlayerClubsAsync(
        [Description("The Chess.com username of the player to get clubs for")] string username,
        CancellationToken cancellationToken = default)
    {
        var clubs = await _chessStatsService.GetPlayerClubsAsync(username, cancellationToken);
        
        if (clubs == null || clubs.Clubs.Length == 0)
        {
            return new ChessPlayerClubsResult
            {
                Username = username,
                Success = true,
                Clubs = clubs,
                Summary = "This player is not a member of any chess clubs."
            };
        }

        return new ChessPlayerClubsResult
        {
            Username = username,
            Success = true,
            Clubs = clubs,
            Summary = CreateClubsSummary(clubs)
        };
    }

    [McpServerTool(Name = "get_chess_player_daily_games")]
    [Description("Gets the current daily chess games that a player is actively playing on Chess.com, including game positions, time controls, and opponent information")]
    public async Task<ChessPlayerDailyGamesResult> GetChessPlayerDailyGamesAsync(
        [Description("The Chess.com username of the player to get daily games for")] string username,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var games = await _chessStatsService.GetPlayerDailyGamesAsync(username, cancellationToken);
            
            if (games == null || games.Games.Length == 0)
            {
                return new ChessPlayerDailyGamesResult
                {
                    Username = username,
                    Success = true,
                    Games = games,
                    Summary = "This player is not currently playing any daily chess games."
                };
            }

            return new ChessPlayerDailyGamesResult
            {
                Username = username,
                Success = true,
                Games = games,
                Summary = CreateDailyGamesSummary(games)
            };
        }
        catch (Exception ex)
        {
            return new ChessPlayerDailyGamesResult
            {
                Username = username,
                Success = false,
                Error = ex.Message
            };
        }
    }

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

    private static string CreateClubsSummary(ChessPlayerClubs clubs)
    {
        var summary = new List<string>
        {
            $"Chess Player Club Memberships Summary:",
            $"• Total clubs: {clubs.Clubs.Length}"
        };

        foreach (var club in clubs.Clubs.Take(10)) // Limit to first 10 clubs to avoid overly long summaries
        {
            var joinedDate = DateTimeOffset.FromUnixTimeSeconds(club.Joined).ToString("MMM yyyy");
            var lastActivityDate = DateTimeOffset.FromUnixTimeSeconds(club.LastActivity).ToString("MMM yyyy");
            summary.Add($"• {club.Name} (joined: {joinedDate}, last activity: {lastActivityDate})");
        }

        if (clubs.Clubs.Length > 10)
        {
            summary.Add($"• ... and {clubs.Clubs.Length - 10} more clubs");
        }

        return string.Join("\n", summary);
    }

    private static string CreateDailyGamesSummary(ChessPlayerDailyGames games)
    {
        var summary = new List<string>
        {
            $"Current Daily Chess Games Summary:",
            $"• Total active games: {games.Games.Length}"
        };

        foreach (var game in games.Games.Take(10)) // Limit to first 10 games to avoid overly long summaries
        {
            var whitePlayer = ExtractUsernameFromUrl(game.White);
            var blackPlayer = ExtractUsernameFromUrl(game.Black);
            var timeControl = game.TimeControl;
            var turn = game.Turn == "white" ? "White" : "Black";
            var moveByDate = game.MoveBy > 0 ? DateTimeOffset.FromUnixTimeSeconds(game.MoveBy).ToString("MMM dd, HH:mm") : "No deadline";
            
            summary.Add($"• {whitePlayer} vs {blackPlayer} ({timeControl}) - {turn} to move by {moveByDate}");
        }

        if (games.Games.Length > 10)
        {
            summary.Add($"• ... and {games.Games.Length - 10} more games");
        }

        return string.Join("\n", summary);
    }

    private static string ExtractUsernameFromUrl(string playerUrl)
    {
        var parts = playerUrl.Split('/');
        return parts.Length > 0 ? parts[^1] : "Unknown";
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

public class ChessPlayerClubsResult
{
    public string Username { get; set; } = "";
    public bool Success { get; set; }
    public string? Error { get; set; }
    public ChessPlayerClubs? Clubs { get; set; }
    public string? Summary { get; set; }
}

public class ChessPlayerDailyGamesResult
{
    public string Username { get; set; } = "";
    public bool Success { get; set; }
    public string? Error { get; set; }
    public ChessPlayerDailyGames? Games { get; set; }
    public string? Summary { get; set; }
}
