using System.Text.Json.Serialization;

namespace TogglMCP.Models;

public class TimeEntry
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("workspace_id")]
    public long WorkspaceId { get; set; }
    
    [JsonPropertyName("project_id")]
    public long? ProjectId { get; set; }
    
    [JsonPropertyName("task_id")]
    public long? TaskId { get; set; }
    
    [JsonPropertyName("billable")]
    public bool Billable { get; set; }
    
    [JsonPropertyName("start")]
    public string Start { get; set; } = "";
    
    [JsonPropertyName("stop")]
    public string? Stop { get; set; }
    
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }
    
    [JsonPropertyName("tag_ids")]
    public List<long>? TagIds { get; set; }
    
    [JsonPropertyName("duronly")]
    public bool? DurOnly { get; set; }
    
    [JsonPropertyName("at")]
    public string? At { get; set; }
    
    [JsonPropertyName("server_deleted_at")]
    public string? ServerDeletedAt { get; set; }
    
    [JsonPropertyName("user_id")]
    public long? UserId { get; set; }
    
    [JsonPropertyName("uid")]
    public long? Uid { get; set; }
    
    [JsonPropertyName("wid")]
    public long? Wid { get; set; }
    
    [JsonPropertyName("pid")]
    public long? Pid { get; set; }
    
    [JsonPropertyName("tid")]
    public long? Tid { get; set; }
}

public class CreateTimeEntryRequest
{
    [JsonPropertyName("workspace_id")]
    public long WorkspaceId { get; set; }
    
    [JsonPropertyName("project_id")]
    public long? ProjectId { get; set; }
    
    [JsonPropertyName("task_id")]
    public long? TaskId { get; set; }
    
    [JsonPropertyName("billable")]
    public bool? Billable { get; set; }
    
    [JsonPropertyName("start")]
    public string Start { get; set; } = "";
    
    [JsonPropertyName("stop")]
    public string? Stop { get; set; }
    
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }
    
    [JsonPropertyName("tag_ids")]
    public List<long>? TagIds { get; set; }
    
    [JsonPropertyName("created_with")]
    public string CreatedWith { get; set; } = "Toggl MCP Server";
}

public class UpdateTimeEntryRequest
{
    [JsonPropertyName("workspace_id")]
    public long WorkspaceId { get; set; }
    
    [JsonPropertyName("project_id")]
    public long? ProjectId { get; set; }
    
    [JsonPropertyName("task_id")]
    public long? TaskId { get; set; }
    
    [JsonPropertyName("billable")]
    public bool? Billable { get; set; }
    
    [JsonPropertyName("start")]
    public string? Start { get; set; }
    
    [JsonPropertyName("stop")]
    public string? Stop { get; set; }
    
    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }
    
    [JsonPropertyName("tag_ids")]
    public List<int>? TagIds { get; set; }
    
    [JsonPropertyName("tag_action")]
    public string? TagAction { get; set; }
}

public class Workspace
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    [JsonPropertyName("profile")]
    public int? Profile { get; set; }
    
    [JsonPropertyName("premium")]
    public bool? Premium { get; set; }
    
    [JsonPropertyName("admin")]
    public bool? Admin { get; set; }
    
    [JsonPropertyName("default_hourly_rate")]
    public decimal? DefaultHourlyRate { get; set; }
    
    [JsonPropertyName("default_currency")]
    public string? DefaultCurrency { get; set; }
    
    [JsonPropertyName("only_admins_may_create_projects")]
    public bool? OnlyAdminsMayCreateProjects { get; set; }
    
    [JsonPropertyName("only_admins_see_billable_rates")]
    public bool? OnlyAdminsSeeBillableRates { get; set; }
    
    [JsonPropertyName("rounding")]
    public int? Rounding { get; set; }
    
    [JsonPropertyName("rounding_minutes")]
    public int? RoundingMinutes { get; set; }
    
    [JsonPropertyName("api_token")]
    public string? ApiToken { get; set; }
    
    [JsonPropertyName("at")]
    public string? At { get; set; }
    
    [JsonPropertyName("logo_url")]
    public string? LogoUrl { get; set; }
    
    [JsonPropertyName("ical_enabled")]
    public bool? IcalEnabled { get; set; }
}

public class Project
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("workspace_id")]
    public long WorkspaceId { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    [JsonPropertyName("billable")]
    public bool? Billable { get; set; }
    
    [JsonPropertyName("is_private")]
    public bool? IsPrivate { get; set; }
    
    [JsonPropertyName("active")]
    public bool? Active { get; set; }
    
    [JsonPropertyName("template")]
    public bool? Template { get; set; }
    
    [JsonPropertyName("at")]
    public string? At { get; set; }
    
    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
    
    [JsonPropertyName("color")]
    public string? Color { get; set; }
    
    [JsonPropertyName("auto_estimates")]
    public bool? AutoEstimates { get; set; }
    
    [JsonPropertyName("actual_hours")]
    public int? ActualHours { get; set; }
    
    [JsonPropertyName("wid")]
    public long? Wid { get; set; }
    
    [JsonPropertyName("cid")]
    public long? Cid { get; set; }
}

public class TimeEntryListResponse
{
    [JsonPropertyName("data")]
    public List<TimeEntry> Data { get; set; } = new();
}

public class TogglApiError
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
    [JsonPropertyName("tip")]
    public string? Tip { get; set; }
    
    [JsonPropertyName("code")]
    public int? Code { get; set; }
}