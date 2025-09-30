package be.infosupport.mcp_chess_demo.config;

import java.time.Duration;
import lombok.Data;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.reactive.function.client.WebClient;

@Configuration
@EnableConfigurationProperties(ChessConfiguration.ChessProperties.class)
@RequiredArgsConstructor
@Slf4j
public class ChessConfiguration {

  private final ChessProperties chessProperties;

  @Bean
  public WebClient chessWebClient() {
    log.info(
        "Creating WebClient for chess.com API with baseUrl: {}", chessProperties.getApiBaseUrl());

    return WebClient.builder()
        .baseUrl(chessProperties.getApiBaseUrl())
        .defaultHeader("User-Agent", chessProperties.getUserAgent())
        .codecs(configurer -> configurer.defaultCodecs().maxInMemorySize(1024 * 1024)) // 1MB
        .build();
  }

  @ConfigurationProperties(prefix = "chess")
  @Data
  public static class ChessProperties {

    /** Base URL for the chess.com public API */
    private String apiBaseUrl = "https://api.chess.com/pub";

    /** User agent string for HTTP requests */
    private String userAgent = "Chess MCP Server 1.0";

    /** HTTP client timeout in seconds */
    private int timeoutSeconds = 30;

    /** Maximum number of retries for failed requests */
    private int maxRetries = 3;

    public Duration getTimeout() {
      return Duration.ofSeconds(timeoutSeconds);
    }
  }
}
