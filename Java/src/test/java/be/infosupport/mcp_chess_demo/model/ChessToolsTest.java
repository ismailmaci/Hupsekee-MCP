package be.infosupport.mcp_chess_demo.model;

import static org.assertj.core.api.Assertions.assertThat;
import static org.mockito.Mockito.when;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

/** Unit tests for ChessTools MCP functionality */
@ExtendWith(MockitoExtension.class)
class ChessToolsTest {

  @Mock private ChessClient chessClient;

  private ChessTools chessTools;

  @BeforeEach
  void setUp() {
    chessTools = new ChessTools(chessClient);
  }

  @Test
  void getChessPlayerStats_ValidUsername_ReturnsSuccessResult() {
    // Given
    String username = "testuser";
    ChessPlayerStats mockStats = createSampleChessPlayerStats();
    when(chessClient.getPlayerStats(username)).thenReturn(mockStats);

    // When
    ChessPlayerStatsResult result = chessTools.getChessPlayerStats(username);

    // Then
    assertThat(result).isNotNull();
    assertThat(result.success()).isTrue();
    assertThat(result.username()).isEqualTo(username);
    assertThat(result.stats()).isEqualTo(mockStats);
    assertThat(result.summary()).isNotNull();
    assertThat(result.summary()).contains("Chess Player Statistics Summary for testuser");
    assertThat(result.summary()).contains("Rapid: 1500");
    assertThat(result.error()).isNull();
  }

  @Test
  void getChessPlayerStats_PlayerNotFound_ReturnsErrorResult() {
    // Given
    String username = "nonexistentuser";
    when(chessClient.getPlayerStats(username))
        .thenThrow(new IllegalArgumentException("Chess player 'nonexistentuser' not found"));

    // When
    ChessPlayerStatsResult result = chessTools.getChessPlayerStats(username);

    // Then
    assertThat(result).isNotNull();
    assertThat(result.success()).isFalse();
    assertThat(result.username()).isEqualTo(username);
    assertThat(result.stats()).isNull();
    assertThat(result.summary()).isNull();
    assertThat(result.error()).contains("Invalid username");
    assertThat(result.error()).contains("Chess player 'nonexistentuser' not found");
  }

  @Test
  void getChessPlayerStats_NetworkError_ReturnsErrorResult() {
    // Given
    String username = "testuser";
    when(chessClient.getPlayerStats(username))
        .thenThrow(new RuntimeException("Failed to fetch chess player stats: Connection timeout"));

    // When
    ChessPlayerStatsResult result = chessTools.getChessPlayerStats(username);

    // Then
    assertThat(result).isNotNull();
    assertThat(result.success()).isFalse();
    assertThat(result.username()).isEqualTo(username);
    assertThat(result.stats()).isNull();
    assertThat(result.summary()).isNull();
    assertThat(result.error()).contains("Failed to fetch chess statistics");
    assertThat(result.error()).contains("Connection timeout");
  }

  @Test
  void getChessPlayerStats_NullStats_ReturnsErrorResult() {
    // Given
    String username = "testuser";
    when(chessClient.getPlayerStats(username)).thenReturn(null);

    // When
    ChessPlayerStatsResult result = chessTools.getChessPlayerStats(username);

    // Then
    assertThat(result).isNotNull();
    assertThat(result.success()).isFalse();
    assertThat(result.username()).isEqualTo(username);
    assertThat(result.stats()).isNull();
    assertThat(result.summary()).isNull();
    assertThat(result.error()).isEqualTo("No statistics found for this player");
  }

  @Test
  void getChessPlayerStats_ComprehensiveStats_CreatesDetailedSummary() {
    // Given
    String username = "chessmaster";
    ChessPlayerStats mockStats = createComprehensiveChessPlayerStats();
    when(chessClient.getPlayerStats(username)).thenReturn(mockStats);

    // When
    ChessPlayerStatsResult result = chessTools.getChessPlayerStats(username);

    // Then
    assertThat(result).isNotNull();
    assertThat(result.success()).isTrue();
    assertThat(result.summary()).contains("Chess Player Statistics Summary for chessmaster");
    assertThat(result.summary()).contains("Rapid: 2200");
    assertThat(result.summary()).contains("Blitz: 2100");
    assertThat(result.summary()).contains("Bullet: 1900");
    assertThat(result.summary()).contains("Daily: 2000");
    assertThat(result.summary()).contains("Tactics: 2500");
    assertThat(result.summary()).contains("Puzzle Rush: 45");
  }

  private ChessPlayerStats createSampleChessPlayerStats() {
    Rating rapidRating = new Rating(1500, System.currentTimeMillis() / 1000, 50);
    GameStats rapidStats = new GameStats(rapidRating, null, null);

    return new ChessPlayerStats(
        null, // chessDaily
        null, // chess960Daily
        rapidStats, // chessRapid
        null, // chessBlitz
        null, // chessBullet
        null, // tactics
        null, // lessons
        null // puzzleRush
        );
  }

  private ChessPlayerStats createComprehensiveChessPlayerStats() {
    // Create comprehensive stats for testing
    Rating rapidRating = new Rating(2200, System.currentTimeMillis() / 1000, 30);
    Rating blitzRating = new Rating(2100, System.currentTimeMillis() / 1000, 35);
    Rating bulletRating = new Rating(1900, System.currentTimeMillis() / 1000, 40);
    Rating dailyRating = new Rating(2000, System.currentTimeMillis() / 1000, 25);

    GameStats rapidStats = new GameStats(rapidRating, null, null);
    GameStats blitzStats = new GameStats(blitzRating, null, null);
    GameStats bulletStats = new GameStats(bulletRating, null, null);
    GameStats dailyStats = new GameStats(dailyRating, null, null);

    // Add the imports for tactics and puzzle rush classes
    var tacticsRating = new TacticsRating(2500, System.currentTimeMillis() / 1000);
    var tacticsStats = new TacticsStats(tacticsRating, null);

    var puzzleRushBest = new PuzzleRushBest(45, System.currentTimeMillis() / 1000);
    var puzzleRushStats = new PuzzleRushStats(puzzleRushBest);

    return new ChessPlayerStats(
        dailyStats, null, rapidStats, blitzStats, bulletStats, tacticsStats, null, puzzleRushStats);
  }
}
