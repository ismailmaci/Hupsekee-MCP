using System.ComponentModel;
using ModelContextProtocol.Server;
using TogglMCP.Services;
using TogglMCP.Models;

namespace TogglMCP.Tools;

public class TogglTools(TogglApiService togglApiService)
{
    private readonly TogglApiService _togglApiService = togglApiService;
    private bool _isAuthenticated = true;

    [McpServerTool(Name = "get_workspaces")]
    [Description("Get all available Toggl workspaces for the authenticated user")]
    public async Task<TogglResult<List<Workspace>>> GetWorkspacesAsync(CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<List<Workspace>>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var workspaces = await _togglApiService.GetWorkspacesAsync(cancellationToken);
            return new TogglResult<List<Workspace>>
            {
                Success = true,
                Data = workspaces,
                Message = $"Retrieved {workspaces.Count} workspace(s)"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<List<Workspace>>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "get_projects")]
    [Description("Get all projects in a specific workspace")]
    public async Task<TogglResult<List<Project>>> GetProjectsAsync(
        [Description("The workspace ID to get projects from")] long workspaceId,
        CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<List<Project>>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var projects = await _togglApiService.GetProjectsAsync(workspaceId, cancellationToken);
            return new TogglResult<List<Project>>
            {
                Success = true,
                Data = projects,
                Message = $"Retrieved {projects.Count} project(s) from workspace {workspaceId}"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<List<Project>>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "start_timer")]
    [Description("Start a new time tracking timer with a description and optional project")]
    public async Task<TogglResult<TimeEntry>> StartTimerAsync(
        [Description("The workspace ID where the timer should be created")] long workspaceId,
        [Description("Description of what you're working on")] string description,
        [Description("Optional project ID to associate with this time entry")] long? projectId = null,
        [Description("Optional comma-separated list of tags")] string? tags = null,
        CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var tagList = string.IsNullOrEmpty(tags) ? null : tags.Split(',').Select(t => t.Trim()).ToList();
            var timeEntry = await _togglApiService.StartTimerAsync(workspaceId, description, projectId, tagList, cancellationToken);
            
            return new TogglResult<TimeEntry>
            {
                Success = true,
                Data = timeEntry,
                Message = $"Timer started: '{description}' (ID: {timeEntry.Id})"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "stop_timer")]
    [Description("Stop the currently running timer")]
    public async Task<TogglResult<TimeEntry>> StopTimerAsync(CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var currentTimer = await _togglApiService.GetCurrentTimeEntryAsync(cancellationToken);
            if (currentTimer == null)
            {
                return new TogglResult<TimeEntry>
                {
                    Success = false,
                    Error = "No timer is currently running."
                };
            }

            var stoppedTimer = await _togglApiService.StopTimeEntryAsync(currentTimer.WorkspaceId, currentTimer.Id, cancellationToken);
            var duration = TimeSpan.FromSeconds(stoppedTimer.Duration);
            
            return new TogglResult<TimeEntry>
            {
                Success = true,
                Data = stoppedTimer,
                Message = $"Timer stopped. Duration: {duration:hh\\:mm\\:ss} - '{stoppedTimer.Description}'"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "get_current_timer")]
    [Description("Get information about the currently running timer, if any")]
    public async Task<TogglResult<TimeEntry?>> GetCurrentTimerAsync(CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<TimeEntry?>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var currentTimer = await _togglApiService.GetCurrentTimeEntryAsync(cancellationToken);
            
            if (currentTimer == null)
            {
                return new TogglResult<TimeEntry?>
                {
                    Success = true,
                    Data = null,
                    Message = "No timer is currently running."
                };
            }

            var elapsed = DateTime.UtcNow - DateTime.Parse(currentTimer.Start);
            
            return new TogglResult<TimeEntry?>
            {
                Success = true,
                Data = currentTimer,
                Message = $"Timer running: '{currentTimer.Description}' - Elapsed: {elapsed:hh\\:mm\\:ss}"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<TimeEntry?>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "get_time_entries")]
    [Description("Get time entries for a specific date range")]
    public async Task<TogglResult<List<TimeEntry>>> GetTimeEntriesAsync(
        [Description("Start date in YYYY-MM-DD format (optional, defaults to today)")] string? startDate = null,
        [Description("End date in YYYY-MM-DD format (optional, defaults to today)")] string? endDate = null,
        CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<List<TimeEntry>>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var entries = await _togglApiService.GetTimeEntriesAsync(startDate, endDate, cancellationToken);
            var totalDuration = TimeSpan.FromSeconds(entries.Where(e => e.Duration > 0).Sum(e => e.Duration));
            
            return new TogglResult<List<TimeEntry>>
            {
                Success = true,
                Data = entries,
                Message = $"Retrieved {entries.Count} time entries. Total duration: {totalDuration:hh\\:mm\\:ss}"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<List<TimeEntry>>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "create_time_entry")]
    [Description("Create a new completed time entry with start and stop times")]
    public async Task<TogglResult<TimeEntry>> CreateTimeEntryAsync(
        [Description("The workspace ID where the time entry should be created")] long workspaceId,
        [Description("Description of the work done")] string description,
        [Description("Start time in ISO 8601 format (e.g., 2023-12-01T09:00:00Z)")] string startTime,
        [Description("Stop time in ISO 8601 format (e.g., 2023-12-01T10:30:00Z)")] string stopTime,
        [Description("Optional project ID to associate with this time entry")] long? projectId = null,
        [Description("Optional comma-separated list of tags")] string? tags = null,
        [Description("Whether the time entry should be marked as billable")] bool billable = false,
        CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var start = DateTime.Parse(startTime);
            var stop = DateTime.Parse(stopTime);
            var duration = (int)(stop - start).TotalSeconds;
            
            if (duration <= 0)
            {
                return new TogglResult<TimeEntry>
                {
                    Success = false,
                    Error = "Stop time must be after start time."
                };
            }

            var tagList = string.IsNullOrEmpty(tags) ? null : tags.Split(',').Select(t => t.Trim()).ToList();
            
            var request = new CreateTimeEntryRequest
            {
                WorkspaceId = workspaceId,
                Description = description,
                Start = start.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Stop = stop.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Duration = duration,
                ProjectId = projectId,
                Tags = tagList,
                Billable = billable,
                CreatedWith = "Toggl MCP Server"
            };

            var timeEntry = await _togglApiService.CreateTimeEntryAsync(workspaceId, request, cancellationToken);
            var durationSpan = TimeSpan.FromSeconds(timeEntry.Duration);
            
            return new TogglResult<TimeEntry>
            {
                Success = true,
                Data = timeEntry,
                Message = $"Time entry created: '{description}' - Duration: {durationSpan:hh\\:mm\\:ss} (ID: {timeEntry.Id})"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "update_time_entry")]
    [Description("Update an existing time entry")]
    public async Task<TogglResult<TimeEntry>> UpdateTimeEntryAsync(
        [Description("The workspace ID containing the time entry")] long workspaceId,
        [Description("The ID of the time entry to update")] long timeEntryId,
        [Description("New description (optional)")] string? description = null,
        [Description("New project ID (optional)")] long? projectId = null,
        [Description("New comma-separated list of tags (optional)")] string? tags = null,
        [Description("Whether the time entry should be marked as billable (optional)")] bool? billable = null,
        CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            var tagList = string.IsNullOrEmpty(tags) ? null : tags.Split(',').Select(t => t.Trim()).ToList();
            
            var request = new UpdateTimeEntryRequest
            {
                WorkspaceId = workspaceId,
                Description = description,
                ProjectId = projectId,
                Tags = tagList,
                Billable = billable
            };

            var timeEntry = await _togglApiService.UpdateTimeEntryAsync(workspaceId, timeEntryId, request, cancellationToken);
            
            return new TogglResult<TimeEntry>
            {
                Success = true,
                Data = timeEntry,
                Message = $"Time entry updated: '{timeEntry.Description}' (ID: {timeEntry.Id})"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<TimeEntry>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    [McpServerTool(Name = "delete_time_entry")]
    [Description("Delete a time entry")]
    public async Task<TogglResult<string>> DeleteTimeEntryAsync(
        [Description("The workspace ID containing the time entry")] long workspaceId,
        [Description("The ID of the time entry to delete")] long timeEntryId,
        CancellationToken cancellationToken = default)
    {
        if (!_isAuthenticated)
        {
            return new TogglResult<string>
            {
                Success = false,
                Error = "Not authenticated. Please set your API token first using set_toggl_api_token."
            };
        }

        try
        {
            await _togglApiService.DeleteTimeEntryAsync(workspaceId, timeEntryId, cancellationToken);
            
            return new TogglResult<string>
            {
                Success = true,
                Data = "Time entry deleted successfully",
                Message = $"Time entry {timeEntryId} has been deleted from workspace {workspaceId}"
            };
        }
        catch (Exception ex)
        {
            return new TogglResult<string>
            {
                Success = false,
                Error = ex.Message
            };
        }
    }
}

public class TogglResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
}