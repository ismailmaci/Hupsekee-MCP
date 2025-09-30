package be.infosupport.mcp_chess_demo.model;

/** Result object returned by chess player stats tool */
public record ChessPlayerStatsResult(
    String username, boolean success, String error, ChessPlayerStats stats, String summary) {}
