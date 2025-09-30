package be.infosupport.mcp_chess_demo.model;

import com.fasterxml.jackson.annotation.JsonProperty;

/** Comprehensive chess player statistics from chess.com API */
public record ChessPlayerStats(
    @JsonProperty("chess_daily") GameStats chessDaily,
    @JsonProperty("chess960_daily") GameStats chess960Daily,
    @JsonProperty("chess_rapid") GameStats chessRapid,
    @JsonProperty("chess_blitz") GameStats chessBlitz,
    @JsonProperty("chess_bullet") GameStats chessBullet,
    @JsonProperty("tactics") TacticsStats tactics,
    @JsonProperty("lessons") LessonsStats lessons,
    @JsonProperty("puzzle_rush") PuzzleRushStats puzzleRush) {}

/** Game statistics for a specific time control */
record GameStats(
    @JsonProperty("last") Rating last,
    @JsonProperty("best") BestRating best,
    @JsonProperty("record") GameRecord record) {}

/** Current or most recent rating information */
record Rating(
    @JsonProperty("rating") int ratingValue,
    @JsonProperty("date") long date,
    @JsonProperty("rd") int rd) {}

/** Best rating ever achieved */
record BestRating(
    @JsonProperty("rating") int ratingValue,
    @JsonProperty("date") long date,
    @JsonProperty("game") String gameUrl) {}

/** Win/loss/draw record */
record GameRecord(
    @JsonProperty("win") int win,
    @JsonProperty("loss") int loss,
    @JsonProperty("draw") int draw,
    @JsonProperty("time_per_move") Integer timePerMove,
    @JsonProperty("timeout_percent") Double timeoutPercent) {}

/** Tactics puzzle statistics */
record TacticsStats(
    @JsonProperty("highest") TacticsRating highest, @JsonProperty("lowest") TacticsRating lowest) {}

/** Tactics puzzle rating information */
record TacticsRating(@JsonProperty("rating") int rating, @JsonProperty("date") long date) {}

/** Chess lessons statistics */
record LessonsStats(
    @JsonProperty("highest") LessonsRating highest, @JsonProperty("lowest") LessonsRating lowest) {}

/** Chess lessons rating information */
record LessonsRating(@JsonProperty("rating") int rating, @JsonProperty("date") long date) {}

/** Puzzle Rush game statistics */
record PuzzleRushStats(@JsonProperty("best") PuzzleRushBest best) {}

/** Best Puzzle Rush performance */
record PuzzleRushBest(@JsonProperty("score") int score, @JsonProperty("date") long date) {}
