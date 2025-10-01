using System.Text.Json;
using ChessMCP.Models;

namespace ChessMCP.Services;

public class ChessStatsService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ChessStatsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ChessPlayerStats?> GetPlayerStatsAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        username = username.Trim().ToLowerInvariant();

        try
        {
            var response = await _httpClient.GetAsync($"https://api.chess.com/pub/player/{username}/stats", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new InvalidOperationException($"Chess player '{username}' not found.");
                }
                
                response.EnsureSuccessStatusCode();
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<ChessPlayerStats>(content, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch chess player stats for '{username}': {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse chess player stats for '{username}': {ex.Message}", ex);
        }
    }

    public async Task<ChessPlayerClubs?> GetPlayerClubsAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        username = username.Trim().ToLowerInvariant();

        try
        {
            var response = await _httpClient.GetAsync($"https://api.chess.com/pub/player/{username}/clubs", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new InvalidOperationException($"Chess player '{username}' not found.");
                }
                
                response.EnsureSuccessStatusCode();
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<ChessPlayerClubs>(content, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch chess player clubs for '{username}': {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse chess player clubs for '{username}': {ex.Message}", ex);
        }
    }

    public async Task<ChessPlayerDailyGames?> GetPlayerDailyGamesAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        username = username.Trim().ToLowerInvariant();

        try
        {
            var response = await _httpClient.GetAsync($"https://api.chess.com/pub/player/{username}/games", cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new InvalidOperationException($"Chess player '{username}' not found.");
                }
                
                response.EnsureSuccessStatusCode();
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<ChessPlayerDailyGames>(content, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch chess player daily games for '{username}': {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse chess player daily games for '{username}': {ex.Message}", ex);
        }
    }
}
