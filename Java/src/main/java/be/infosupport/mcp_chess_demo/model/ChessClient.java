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

  /**
   * Fetch chess club memberships for a player
   *
   * @param username The chess.com username (case-insensitive)
   * @return Chess player clubs or null if player not found
   * @throws IllegalArgumentException if username is null or empty
   * @throws RuntimeException if API request fails
   */
  public ChessPlayerClubs getPlayerClubs(String username) {
    if (username == null || username.trim().isEmpty()) {
      throw new IllegalArgumentException("Username cannot be null or empty");
    }

    String normalizedUsername = username.trim().toLowerCase();
    log.debug("Fetching chess clubs for user: {}", normalizedUsername);

    try {
      return chessWebClient
          .get()
          .uri("/player/{username}/clubs", normalizedUsername)
          .retrieve()
          .bodyToMono(ChessPlayerClubs.class)
          .timeout(chessProperties.getTimeout())
          .doOnSuccess(
              clubs -> log.debug("Successfully fetched clubs for user: {}", normalizedUsername))
          .doOnError(
              error ->
                  log.error(
                      "Error fetching clubs for user {}: {}",
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
                      "Failed to fetch chess player clubs for '"
                          + normalizedUsername
                          + "': "
                          + ex.getMessage(),
                      ex))
          .onErrorMap(
              Exception.class,
              ex ->
                  new RuntimeException(
                      "Unexpected error fetching chess player clubs for '"
                          + normalizedUsername
                          + "': "
                          + ex.getMessage(),
                      ex))
          .block();

    } catch (Exception e) {
      log.error("Failed to fetch chess clubs for user: {}", normalizedUsername, e);
      throw new RuntimeException(
          "Failed to fetch chess player clubs for '" + normalizedUsername + "': " + e.getMessage(),
          e);
    }
  }

  /**
   * Fetch current daily chess games for a player
   *
   * @param username The chess.com username (case-insensitive)
   * @return Chess player daily games or null if player not found
   * @throws IllegalArgumentException if username is null or empty
   * @throws RuntimeException if API request fails
   */
  public ChessPlayerDailyGames getPlayerDailyGames(String username) {
    if (username == null || username.trim().isEmpty()) {
      throw new IllegalArgumentException("Username cannot be null or empty");
    }

    String normalizedUsername = username.trim().toLowerCase();
    log.debug("Fetching chess daily games for user: {}", normalizedUsername);

    try {
      return chessWebClient
          .get()
          .uri("/player/{username}/games", normalizedUsername)
          .retrieve()
          .bodyToMono(ChessPlayerDailyGames.class)
          .timeout(chessProperties.getTimeout())
          .doOnSuccess(
              games -> log.debug("Successfully fetched daily games for user: {}", normalizedUsername))
          .doOnError(
              error ->
                  log.error(
                      "Error fetching daily games for user {}: {}",
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
                      "Failed to fetch chess player daily games for '"
                          + normalizedUsername
                          + "': "
                          + ex.getMessage(),
                      ex))
          .onErrorMap(
              Exception.class,
              ex ->
                  new RuntimeException(
                      "Unexpected error fetching chess player daily games for '"
                          + normalizedUsername
                          + "': "
                          + ex.getMessage(),
                      ex))
          .block();

    } catch (Exception e) {
      log.error("Failed to fetch chess daily games for user: {}", normalizedUsername, e);
      throw new RuntimeException(
          "Failed to fetch chess player daily games for '" + normalizedUsername + "': " + e.getMessage(),
          e);
    }
  }
}
