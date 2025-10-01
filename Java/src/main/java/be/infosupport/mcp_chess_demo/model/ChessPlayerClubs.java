package be.infosupport.mcp_chess_demo.model;

import com.fasterxml.jackson.annotation.JsonProperty;

/** Chess player club memberships from chess.com API */
public record ChessPlayerClubs(@JsonProperty("clubs") ClubInfo[] clubs) {}

/** Information about a chess club */
record ClubInfo(
    @JsonProperty("@id") String id,
    @JsonProperty("name") String name,
    @JsonProperty("last_activity") long lastActivity,
    @JsonProperty("joined") long joined,
    @JsonProperty("icon") String icon,
    @JsonProperty("url") String url) {}