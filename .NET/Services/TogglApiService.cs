using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TogglMCP.Models;

namespace TogglMCP.Services;

public class TogglApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private const string BaseUrl = "https://api.track.toggl.com/api/v9";
    private string? _apiToken = "";

    public TogglApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
        
        // Set authentication if token is available
        if (!string.IsNullOrEmpty(_apiToken))
        {
            var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiToken}:api_token"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        }
    }

    private void EnsureAuthenticated()
    {
        if (string.IsNullOrEmpty(_apiToken))
        {
            throw new InvalidOperationException("API token is not set. Please call SetApiToken first.");
        }
    }

    public async Task<List<Workspace>> GetWorkspacesAsync(CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        var response = await _httpClient.GetAsync($"{BaseUrl}/workspaces", cancellationToken);
        await EnsureSuccessStatusCodeAsync(response);
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<Workspace>>(content, _jsonOptions) ?? new List<Workspace>();
    }

    public async Task<List<Project>> GetProjectsAsync(long workspaceId, CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        var response = await _httpClient.GetAsync($"{BaseUrl}/workspaces/{workspaceId}/projects", cancellationToken);
        await EnsureSuccessStatusCodeAsync(response);
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<Project>>(content, _jsonOptions) ?? new List<Project>();
    }

    public async Task<List<TimeEntry>> GetTimeEntriesAsync(string? startDate = null, string? endDate = null, CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        var url = $"{BaseUrl}/me/time_entries";
        var queryParams = new List<string>();
        
        if (!string.IsNullOrEmpty(startDate))
            queryParams.Add($"start_date={startDate}");
        
        if (!string.IsNullOrEmpty(endDate))
            queryParams.Add($"end_date={endDate}");
        
        if (queryParams.Any())
            url += "?" + string.Join("&", queryParams);
        
        var response = await _httpClient.GetAsync(url, cancellationToken);
        await EnsureSuccessStatusCodeAsync(response);
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<List<TimeEntry>>(content, _jsonOptions) ?? new List<TimeEntry>();
    }

    public async Task<TimeEntry?> GetCurrentTimeEntryAsync(CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/me/time_entries/current", cancellationToken);
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // No running timer
            }
            
            await EnsureSuccessStatusCodeAsync(response);
            
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TimeEntry>(content, _jsonOptions);
        }
        catch (HttpRequestException)
        {
            return null; // No running timer
        }
    }

    public async Task<TimeEntry?> GetTimeEntryByIdAsync(long workspaceId, long timeEntryId, CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        var response = await _httpClient.GetAsync($"{BaseUrl}/workspaces/{workspaceId}/time_entries/{timeEntryId}", cancellationToken);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        
        await EnsureSuccessStatusCodeAsync(response);
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TimeEntry>(content, _jsonOptions);
    }

    public async Task<TimeEntry> CreateTimeEntryAsync(long workspaceId, CreateTimeEntryRequest request, CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        request.WorkspaceId = workspaceId;
        
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"{BaseUrl}/workspaces/{workspaceId}/time_entries", content, cancellationToken);
        await EnsureSuccessStatusCodeAsync(response);
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TimeEntry>(responseContent, _jsonOptions) 
               ?? throw new InvalidOperationException("Failed to deserialize created time entry.");
    }

    public async Task<TimeEntry> UpdateTimeEntryAsync(long workspaceId, long timeEntryId, UpdateTimeEntryRequest request, CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        request.WorkspaceId = workspaceId;
        
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync($"{BaseUrl}/workspaces/{workspaceId}/time_entries/{timeEntryId}", content, cancellationToken);
        await EnsureSuccessStatusCodeAsync(response);
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TimeEntry>(responseContent, _jsonOptions) 
               ?? throw new InvalidOperationException("Failed to deserialize updated time entry.");
    }

    public async Task<TimeEntry> StopTimeEntryAsync(long workspaceId, long timeEntryId, CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        var response = await _httpClient.PatchAsync($"{BaseUrl}/workspaces/{workspaceId}/time_entries/{timeEntryId}/stop", null, cancellationToken);
        await EnsureSuccessStatusCodeAsync(response);
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TimeEntry>(responseContent, _jsonOptions) 
               ?? throw new InvalidOperationException("Failed to deserialize stopped time entry.");
    }

    public async Task DeleteTimeEntryAsync(long workspaceId, long timeEntryId, CancellationToken cancellationToken = default)
    {
        EnsureAuthenticated();
        
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/workspaces/{workspaceId}/time_entries/{timeEntryId}", cancellationToken);
        await EnsureSuccessStatusCodeAsync(response);
    }

    public async Task<TimeEntry> StartTimerAsync(long workspaceId, string description, long? projectId = null, List<string>? tags = null, CancellationToken cancellationToken = default)
    {
        var request = new CreateTimeEntryRequest
        {
            WorkspaceId = workspaceId,
            Description = description,
            ProjectId = projectId,
            Tags = tags,
            Start = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            Duration = -1, // Running timer
            CreatedWith = "Toggl MCP Server"
        };
        
        return await CreateTimeEntryAsync(workspaceId, request, cancellationToken);
    }

    private static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            TogglApiError? error = null;
            
            try
            {
                error = JsonSerializer.Deserialize<TogglApiError>(errorContent);
            }
            catch
            {
                // Ignore JSON parsing errors for error responses
            }
            
            var errorMessage = error?.Message ?? $"HTTP {(int)response.StatusCode} {response.StatusCode}";
            
            if (!string.IsNullOrEmpty(error?.Tip))
            {
                errorMessage += $" - {error.Tip}";
            }
            
            throw new HttpRequestException($"Toggl API error: {errorMessage}");
        }
    }
}