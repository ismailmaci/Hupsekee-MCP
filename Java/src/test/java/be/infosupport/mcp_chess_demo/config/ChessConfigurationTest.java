package be.infosupport.mcp_chess_demo.config;

import static org.assertj.core.api.Assertions.assertThat;

import java.time.Duration;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.TestPropertySource;
import org.springframework.web.reactive.function.client.WebClient;

/** Tests for ChessConfiguration and properties binding */
@SpringBootTest(classes = {ChessConfiguration.class})
@EnableConfigurationProperties(ChessConfiguration.ChessProperties.class)
@TestPropertySource(
    properties = {
      "chess.api-base-url=https://test.chess.com/api",
      "chess.user-agent=Test Chess MCP Server 2.0",
      "chess.timeout-seconds=60",
      "chess.max-retries=5"
    })
class ChessConfigurationTest {

  @Autowired private ChessConfiguration.ChessProperties chessProperties;

  @Autowired private WebClient chessWebClient;

  @Test
  void chessProperties_LoadsCorrectly() {
    // Then
    assertThat(chessProperties.getApiBaseUrl()).isEqualTo("https://test.chess.com/api");
    assertThat(chessProperties.getUserAgent()).isEqualTo("Test Chess MCP Server 2.0");
    assertThat(chessProperties.getTimeoutSeconds()).isEqualTo(60);
    assertThat(chessProperties.getMaxRetries()).isEqualTo(5);
    assertThat(chessProperties.getTimeout()).isEqualTo(Duration.ofSeconds(60));
  }

  @Test
  void chessWebClient_IsConfiguredCorrectly() {
    // Then
    assertThat(chessWebClient).isNotNull();
    assertThat(chessWebClient).isInstanceOf(WebClient.class);
  }

  @Test
  void chessProperties_DefaultValues_AreCorrect() {
    // Given - create new properties instance to test defaults
    ChessConfiguration.ChessProperties defaultProperties = new ChessConfiguration.ChessProperties();

    // Then
    assertThat(defaultProperties.getApiBaseUrl()).isEqualTo("https://api.chess.com/pub");
    assertThat(defaultProperties.getUserAgent()).isEqualTo("Chess MCP Server 1.0");
    assertThat(defaultProperties.getTimeoutSeconds()).isEqualTo(30);
    assertThat(defaultProperties.getMaxRetries()).isEqualTo(3);
    assertThat(defaultProperties.getTimeout()).isEqualTo(Duration.ofSeconds(30));
  }

  @Test
  void chessProperties_CustomValues_WorkCorrectly() {
    // Given
    ChessConfiguration.ChessProperties properties = new ChessConfiguration.ChessProperties();
    properties.setApiBaseUrl("https://custom.api.com");
    properties.setUserAgent("Custom Agent");
    properties.setTimeoutSeconds(45);
    properties.setMaxRetries(7);

    // Then
    assertThat(properties.getApiBaseUrl()).isEqualTo("https://custom.api.com");
    assertThat(properties.getUserAgent()).isEqualTo("Custom Agent");
    assertThat(properties.getTimeoutSeconds()).isEqualTo(45);
    assertThat(properties.getMaxRetries()).isEqualTo(7);
    assertThat(properties.getTimeout()).isEqualTo(Duration.ofSeconds(45));
  }
}
