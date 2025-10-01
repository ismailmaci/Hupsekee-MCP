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
}
