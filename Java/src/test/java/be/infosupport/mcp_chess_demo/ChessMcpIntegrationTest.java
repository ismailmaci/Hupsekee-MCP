package be.infosupport.mcp_chess_demo;

import static org.assertj.core.api.Assertions.assertThat;

import be.infosupport.mcp_chess_demo.model.ChessPlayerStatsResult;
import be.infosupport.mcp_chess_demo.model.ChessTools;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

/**
 * Integration test for the complete chess MCP server functionality This test makes real API calls
 * to chess.com and should only be run manually or in appropriate environments
 */
@SpringBootTest
class ChessMcpIntegrationTest {

  @Autowired private ChessTools chessTools;

  @Test
  void getChessPlayerStats_RealUser_ReturnsValidStats() {
    // Given
    String username = "hikaru";

    // When
    ChessPlayerStatsResult result = chessTools.getChessPlayerStats(username);

    // Then
    assertThat(result).isNotNull();
    assertThat(result.success()).isTrue();
    assertThat(result.username()).isEqualTo(username);
    assertThat(result.stats()).isNotNull();
    assertThat(result.summary()).isNotNull();
    assertThat(result.summary()).contains("Chess Player Statistics Summary for " + username);
    assertThat(result.error()).isNull();

    // Print the result for manual verification
    System.out.println("=== Chess Player Stats Integration Test Results ===");
    System.out.println("Username: " + result.username());
    System.out.println("Success: " + result.success());
    System.out.println("Summary:");
    System.out.println(result.summary());
    System.out.println("=== End Test Results ===");
  }

  @Test
  void getChessPlayerStats_NonExistentUser_ReturnsErrorResult() {
    // Given
    String username = "nonexistentuserfortesting123456789";

    // When
    ChessPlayerStatsResult result = chessTools.getChessPlayerStats(username);

    // Then
    assertThat(result).isNotNull();
    assertThat(result.success()).isFalse();
    assertThat(result.username()).isEqualTo(username);
    assertThat(result.stats()).isNull();
    assertThat(result.summary()).isNull();
    assertThat(result.error()).isNotNull();
    assertThat(result.error()).contains("No statistics found");

    System.out.println("=== Non-existent User Test Results ===");
    System.out.println("Username: " + result.username());
    System.out.println("Success: " + result.success());
    System.out.println("Error: " + result.error());
    System.out.println("=== End Test Results ===");
  }
}
