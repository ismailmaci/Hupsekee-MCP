package be.infosupport.mcp_chess_demo.model;

/** Result object returned by chess player clubs tool */
public record ChessPlayerClubsResult(
    String username, boolean success, String error, ChessPlayerClubs clubs, String summary) {}