package be.infosupport.mcp_chess_demo.model;

/** Result object returned by chess player daily games tool */
public record ChessPlayerDailyGamesResult(
    String username, boolean success, String error, ChessPlayerDailyGames games, String summary) {}