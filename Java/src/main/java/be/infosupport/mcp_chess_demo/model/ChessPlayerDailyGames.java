package be.infosupport.mcp_chess_demo.model;

import com.fasterxml.jackson.annotation.JsonProperty;

/** Chess player daily games from chess.com API */
public record ChessPlayerDailyGames(@JsonProperty("games") DailyGameInfo[] games) {}

/** Information about a daily chess game */
record DailyGameInfo(
    @JsonProperty("white") String white,
    @JsonProperty("black") String black,
    @JsonProperty("url") String url,
    @JsonProperty("fen") String fen,
    @JsonProperty("pgn") String pgn,
    @JsonProperty("turn") String turn,
    @JsonProperty("move_by") long moveBy,
    @JsonProperty("last_activity") long lastActivity,
    @JsonProperty("start_time") long startTime,
    @JsonProperty("time_control") String timeControl,
    @JsonProperty("time_class") String timeClass,
    @JsonProperty("rules") String rules,
    @JsonProperty("draw_offer") String drawOffer,
    @JsonProperty("tournament") String tournament,
    @JsonProperty("match") String match) {}