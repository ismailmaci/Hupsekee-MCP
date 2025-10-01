using System.Text.Json.Serialization;

namespace ChessMCP.Models;

public record ChessPlayerStats(
    [property: JsonPropertyName("chess_daily")] GameStats? ChessDaily = null,
    [property: JsonPropertyName("chess960_daily")] GameStats? Chess960Daily = null,
    [property: JsonPropertyName("chess_rapid")] GameStats? ChessRapid = null,
    [property: JsonPropertyName("chess_blitz")] GameStats? ChessBlitz = null,
    [property: JsonPropertyName("chess_bullet")] GameStats? ChessBullet = null,
    [property: JsonPropertyName("tactics")] TacticsStats? Tactics = null,
    [property: JsonPropertyName("lessons")] LessonsStats? Lessons = null,
    [property: JsonPropertyName("puzzle_rush")] PuzzleRushStats? PuzzleRush = null
);

public record GameStats(
    [property: JsonPropertyName("last")] Rating? Last = null,
    [property: JsonPropertyName("best")] BestRating? Best = null,
    [property: JsonPropertyName("record")] GameRecord? Record = null
);

public record Rating(
    [property: JsonPropertyName("rating")] int RatingValue,
    [property: JsonPropertyName("date")] long Date,
    [property: JsonPropertyName("rd")] int Rd
);

public record BestRating(
    [property: JsonPropertyName("rating")] int RatingValue,
    [property: JsonPropertyName("date")] long Date,
    [property: JsonPropertyName("game")] string? GameUrl = null
);

public record GameRecord(
    [property: JsonPropertyName("win")] int Win,
    [property: JsonPropertyName("loss")] int Loss,
    [property: JsonPropertyName("draw")] int Draw,
    [property: JsonPropertyName("time_per_move")] int? TimePerMove = null,
    [property: JsonPropertyName("timeout_percent")] double? TimeoutPercent = null
);

public record TacticsStats(
    [property: JsonPropertyName("highest")] TacticsRating? Highest = null,
    [property: JsonPropertyName("lowest")] TacticsRating? Lowest = null
);

public record TacticsRating(
    [property: JsonPropertyName("rating")] int Rating,
    [property: JsonPropertyName("date")] long Date
);

public record LessonsStats(
    [property: JsonPropertyName("highest")] LessonsRating? Highest = null,
    [property: JsonPropertyName("lowest")] LessonsRating? Lowest = null
);

public record LessonsRating(
    [property: JsonPropertyName("rating")] int Rating,
    [property: JsonPropertyName("date")] long Date
);

public record PuzzleRushStats(
    [property: JsonPropertyName("best")] PuzzleRushBest? Best = null
);

public record PuzzleRushBest(
    [property: JsonPropertyName("score")] int Score,
    [property: JsonPropertyName("date")] long Date
);

public record ChessPlayerClubs(
    [property: JsonPropertyName("clubs")] ClubInfo[] Clubs
);

public record ClubInfo(
    [property: JsonPropertyName("@id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("last_activity")] long LastActivity,
    [property: JsonPropertyName("joined")] long Joined,
    [property: JsonPropertyName("icon")] string? Icon = null,
    [property: JsonPropertyName("url")] string? Url = null
);

public record ChessPlayerDailyGames(
    [property: JsonPropertyName("games")] DailyGameInfo[] Games
);

public record DailyGameInfo(
    [property: JsonPropertyName("white")] string White,
    [property: JsonPropertyName("black")] string Black,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("fen")] string Fen,
    [property: JsonPropertyName("pgn")] string Pgn,
    [property: JsonPropertyName("turn")] string Turn,
    [property: JsonPropertyName("move_by")] long MoveBy,
    [property: JsonPropertyName("last_activity")] long LastActivity,
    [property: JsonPropertyName("start_time")] long StartTime,
    [property: JsonPropertyName("time_control")] string TimeControl,
    [property: JsonPropertyName("time_class")] string TimeClass,
    [property: JsonPropertyName("rules")] string Rules,
    [property: JsonPropertyName("draw_offer")] string? DrawOffer = null,
    [property: JsonPropertyName("tournament")] string? Tournament = null,
    [property: JsonPropertyName("match")] string? Match = null
);
