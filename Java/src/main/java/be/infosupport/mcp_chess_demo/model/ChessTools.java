package be.infosupport.mcp_chess_demo.model;

import java.time.Instant;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.ai.tool.annotation.Tool;
import org.springframework.stereotype.Service;

/** Spring AI MCP tools for chess-related functionality */
@Service
@RequiredArgsConstructor
@Slf4j
public class ChessTools {

  private final ChessClient chessClient;

  /**
   * Gets comprehensive chess statistics for a player from Chess.com including ratings, records, and
   * performance metrics across different time controls
   */
  @Tool(
      name = "get_chess_player_stats",
      description =
          "Gets comprehensive chess statistics for a player from Chess.com including ratings, records, and performance metrics across different time controls")
  public ChessPlayerStatsResult getChessPlayerStats(String username) {
    log.info("Fetching chess player stats for: {}", username);

    try {
      ChessPlayerStats stats = chessClient.getPlayerStats(username);

      if (stats == null) {
        log.warn("No statistics found for player: {}", username);
        return new ChessPlayerStatsResult(
            username, false, "No statistics found for this player", null, null);
      }

      String summary = createStatsSummary(username, stats);
      log.debug("Successfully created stats summary for: {}", username);

      return new ChessPlayerStatsResult(username, true, null, stats, summary);

    } catch (IllegalArgumentException e) {
      log.error("Invalid username provided: {}", username, e);
      return new ChessPlayerStatsResult(
          username, false, "Invalid username: " + e.getMessage(), null, null);
    } catch (Exception e) {
      log.error("Error fetching chess stats for {}: {}", username, e.getMessage(), e);
      return new ChessPlayerStatsResult(
          username, false, "Failed to fetch chess statistics: " + e.getMessage(), null, null);
    }
  }

  /** Creates a human-readable summary of chess player statistics */
  private String createStatsSummary(String username, ChessPlayerStats stats) {
    List<String> summary = new ArrayList<>();
    summary.add("Chess Player Statistics Summary for " + username + ":");

    if (stats.chessRapid() != null && stats.chessRapid().last() != null) {
      var rapid = stats.chessRapid();
      summary.add(
          String.format(
              "• Rapid: %d (W:%d L:%d D:%d)",
              rapid.last().ratingValue(),
              rapid.record() != null ? rapid.record().win() : 0,
              rapid.record() != null ? rapid.record().loss() : 0,
              rapid.record() != null ? rapid.record().draw() : 0));
    }

    if (stats.chessBlitz() != null && stats.chessBlitz().last() != null) {
      var blitz = stats.chessBlitz();
      summary.add(
          String.format(
              "• Blitz: %d (W:%d L:%d D:%d)",
              blitz.last().ratingValue(),
              blitz.record() != null ? blitz.record().win() : 0,
              blitz.record() != null ? blitz.record().loss() : 0,
              blitz.record() != null ? blitz.record().draw() : 0));
    }

    if (stats.chessBullet() != null && stats.chessBullet().last() != null) {
      var bullet = stats.chessBullet();
      summary.add(
          String.format(
              "• Bullet: %d (W:%d L:%d D:%d)",
              bullet.last().ratingValue(),
              bullet.record() != null ? bullet.record().win() : 0,
              bullet.record() != null ? bullet.record().loss() : 0,
              bullet.record() != null ? bullet.record().draw() : 0));
    }

    if (stats.chessDaily() != null && stats.chessDaily().last() != null) {
      var daily = stats.chessDaily();
      summary.add(
          String.format(
              "• Daily: %d (W:%d L:%d D:%d)",
              daily.last().ratingValue(),
              daily.record() != null ? daily.record().win() : 0,
              daily.record() != null ? daily.record().loss() : 0,
              daily.record() != null ? daily.record().draw() : 0));
    }

    if (stats.tactics() != null && stats.tactics().highest() != null) {
      summary.add(String.format("• Tactics: %d (highest)", stats.tactics().highest().rating()));
    }

    if (stats.puzzleRush() != null && stats.puzzleRush().best() != null) {
      summary.add(
          String.format("• Puzzle Rush: %d (best score)", stats.puzzleRush().best().score()));
    }

        return String.join("\n", summary);
  }

  /**
   * Gets the list of chess clubs that a player is a member of from Chess.com, including club names, join dates, and activity information
   */
  @Tool(
      name = "get_chess_player_clubs",
      description =
          "Gets the list of chess clubs that a player is a member of from Chess.com, including club names, join dates, and activity information")
  public ChessPlayerClubsResult getChessPlayerClubs(String username) {
    log.info("Fetching chess player clubs for: {}", username);

    try {
      ChessPlayerClubs clubs = chessClient.getPlayerClubs(username);

      if (clubs == null || clubs.clubs().length == 0) {
        log.info("No clubs found for player: {}", username);
        return new ChessPlayerClubsResult(
            username, true, null, clubs, "This player is not a member of any chess clubs.");
      }

      String summary = createClubsSummary(username, clubs);
      log.debug("Successfully created clubs summary for: {}", username);

      return new ChessPlayerClubsResult(username, true, null, clubs, summary);

    } catch (IllegalArgumentException e) {
      log.error("Invalid username provided: {}", username, e);
      return new ChessPlayerClubsResult(
          username, false, "Invalid username: " + e.getMessage(), null, null);
    } catch (Exception e) {
      log.error("Error fetching chess clubs for {}: {}", username, e.getMessage(), e);
      return new ChessPlayerClubsResult(
          username, false, "Failed to fetch chess clubs: " + e.getMessage(), null, null);
    }
  }

  /**
   * Gets the current daily chess games that a player is actively playing on Chess.com, including game positions, time controls, and opponent information
   */
  @Tool(
      name = "get_chess_player_daily_games",
      description =
          "Gets the current daily chess games that a player is actively playing on Chess.com, including game positions, time controls, and opponent information")
  public ChessPlayerDailyGamesResult getChessPlayerDailyGames(String username) {
    log.info("Fetching chess player daily games for: {}", username);

    try {
      ChessPlayerDailyGames games = chessClient.getPlayerDailyGames(username);

      if (games == null || games.games().length == 0) {
        log.info("No daily games found for player: {}", username);
        return new ChessPlayerDailyGamesResult(
            username, true, null, games, "This player is not currently playing any daily chess games.");
      }

      String summary = createDailyGamesSummary(username, games);
      log.debug("Successfully created daily games summary for: {}", username);

      return new ChessPlayerDailyGamesResult(username, true, null, games, summary);

    } catch (IllegalArgumentException e) {
      log.error("Invalid username provided: {}", username, e);
      return new ChessPlayerDailyGamesResult(
          username, false, "Invalid username: " + e.getMessage(), null, null);
    } catch (Exception e) {
      log.error("Error fetching chess daily games for {}: {}", username, e.getMessage(), e);
      return new ChessPlayerDailyGamesResult(
          username, false, "Failed to fetch chess daily games: " + e.getMessage(), null, null);
    }
  }

  /** Creates a human-readable summary of chess player club memberships */
  private String createClubsSummary(String username, ChessPlayerClubs clubs) {
    List<String> summary = new ArrayList<>();
    summary.add("Chess Player Club Memberships Summary for " + username + ":");
    summary.add("• Total clubs: " + clubs.clubs().length);

    DateTimeFormatter formatter = DateTimeFormatter.ofPattern("MMM yyyy")
        .withZone(ZoneId.systemDefault());

    // Limit to first 10 clubs to avoid overly long summaries
    for (int i = 0; i < Math.min(clubs.clubs().length, 10); i++) {
      var club = clubs.clubs()[i];
      String joinedDate = formatter.format(Instant.ofEpochSecond(club.joined()));
      String lastActivityDate = formatter.format(Instant.ofEpochSecond(club.lastActivity()));
      summary.add(String.format("• %s (joined: %s, last activity: %s)", 
          club.name(), joinedDate, lastActivityDate));
    }

    if (clubs.clubs().length > 10) {
      summary.add("• ... and " + (clubs.clubs().length - 10) + " more clubs");
    }

    return String.join("\n", summary);
  }

  /** Creates a human-readable summary of chess player daily games */
  private String createDailyGamesSummary(String username, ChessPlayerDailyGames games) {
    List<String> summary = new ArrayList<>();
    summary.add("Current Daily Chess Games Summary for " + username + ":");
    summary.add("• Total active games: " + games.games().length);

    DateTimeFormatter formatter = DateTimeFormatter.ofPattern("MMM dd, HH:mm")
        .withZone(ZoneId.systemDefault());

    // Limit to first 10 games to avoid overly long summaries
    for (int i = 0; i < Math.min(games.games().length, 10); i++) {
      var game = games.games()[i];
      String whitePlayer = extractUsernameFromUrl(game.white());
      String blackPlayer = extractUsernameFromUrl(game.black());
      String timeControl = game.timeControl();
      String turn = "white".equals(game.turn()) ? "White" : "Black";
      String moveByDate = game.moveBy() > 0 
          ? formatter.format(Instant.ofEpochSecond(game.moveBy())) 
          : "No deadline";
      
      summary.add(String.format("• %s vs %s (%s) - %s to move by %s", 
          whitePlayer, blackPlayer, timeControl, turn, moveByDate));
    }

    if (games.games().length > 10) {
      summary.add("• ... and " + (games.games().length - 10) + " more games");
    }

    return String.join("\n", summary);
  }

  /** Extracts username from Chess.com player URL */
  private String extractUsernameFromUrl(String playerUrl) {
    if (playerUrl == null || playerUrl.isEmpty()) {
      return "Unknown";
    }
    String[] parts = playerUrl.split("/");
    return parts.length > 0 ? parts[parts.length - 1] : "Unknown";
  }
}