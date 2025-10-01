package be.infosupport.mcp_chess_demo.model;

import be.infosupport.mcp_chess_demo.config.ChessConfiguration;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;
import org.springframework.web.reactive.function.client.WebClientResponseException;
import reactor.core.publisher.Mono;

/** Service for fetching chess player statistics from chess.com API */
@Service
@RequiredArgsConstructor
@Slf4j
public class ChessClient {

  private final WebClient chessWebClient;
  private final ChessConfiguration.ChessProperties chessProperties;

  /**
   * Fetch comprehensive chess statistics for a player
   *
   * @param username The chess.com username (case-insensitive)
   * @return Chess player statistics or null if player not found
   * @throws IllegalArgumentException if username is null or empty
   * @throws RuntimeException if API request fails
   */
  public ChessPlayerStats getPlayerStats(String username) {
    if (username == null || username.trim().isEmpty()) {
      throw new IllegalArgumentException("Username cannot be null or empty");
    }

    String normalizedUsername = username.trim().toLowerCase();
    log.debug("Fetching chess stats for user: {}", normalizedUsername);

    try {
      return chessWebClient
          .get()
          .uri("/player/{username}/stats", normalizedUsername)
          .retrieve()
          .bodyToMono(ChessPlayerStats.class)
          .timeout(chessProperties.getTimeout())
          .doOnSuccess(
              stats -> log.debug("Successfully fetched stats for user: {}", normalizedUsername))
          .doOnError(
              error ->
                  log.error(
                      "Error fetching stats for user {}: {}",
                      normalizedUsername,
                      error.getMessage()))
          .onErrorResume(
              WebClientResponseException.NotFound.class,
              ex -> {
                log.warn("Chess player '{}' not found", normalizedUsername);
                return Mono.empty();
              })
          .onErrorMap(
              WebClientResponseException.class,
              ex ->
                  new RuntimeException(
                      "Failed to fetch chess player stats for '"
                          + normalizedUsername
                          + "': "
                          + ex.getMessage(),
                      ex))
          .onErrorMap(
              Exception.class,
              ex ->
                  new RuntimeException(
                      "Unexpected error fetching chess player stats for '"
                          + normalizedUsername
                          + "': "
                          + ex.getMessage(),
                      ex))
          .block();

    } catch (Exception e) {
      log.error("Failed to fetch chess stats for user: {}", normalizedUsername, e);
      throw new RuntimeException(
          "Failed to fetch chess player stats for '" + normalizedUsername + "': " + e.getMessage(),
          e);
    }
  }
}
