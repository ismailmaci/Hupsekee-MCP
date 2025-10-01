# Toggl MCP Server (.NET)

A Model Context Protocol (MCP) server that provides access to the Toggl Track API for time tracking operations.

## Features

- Get workspaces and projects
- Retrieve time entries (current and historical)
- Create and manage time entries
- Start and stop timers
- Update and delete time entries

## Configuration

### API Token Setup

To use this MCP server, you need to provide your Toggl Track API token. There are two ways to do this:

#### Method 1: Environment Variable (Recommended)

1. Get your API token from [Toggl Track Profile Settings](https://track.toggl.com/profile)
2. Set the `TOGGL_API_TOKEN` environment variable in your MCP configuration

**Example MCP Configuration:**

```json
{
  "servers": {
    "toggl-mcp-dotnet": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "path/to/TogglMCP.csproj"
      ],
      "env": {
        "DOTNET_ENVIRONMENT": "Production",
        "TOGGL_API_TOKEN": "your_actual_api_token_here"
      }
    }
  }
}
```

#### Method 2: Runtime Token Setting

If you can't set the environment variable, you can use the `set_toggl_api_token` tool after the server starts:

```
set_toggl_api_token("your_actual_api_token_here")
```

**Note:** The environment variable method is more secure and recommended for production use.

## Available Tools

- `get_workspaces` - Get all available Toggl workspaces
- `get_projects` - Get all projects in a specific workspace
- `get_time_entries` - Get time entries with optional date filtering
- `get_current_time_entry` - Get the currently running timer (if any)
- `create_time_entry` - Create a new time entry
- `start_timer` - Start a new timer with description and optional project/tags
- `stop_timer` - Stop the currently running timer
- `update_time_entry` - Update an existing time entry
- `delete_time_entry` - Delete a time entry
- `set_toggl_api_token` - Set API token at runtime (not recommended for production)

## Getting Your Toggl API Token

1. Log in to [Toggl Track](https://track.toggl.com/)
2. Go to Profile Settings (click on your profile picture → Profile settings)
3. Scroll down to find your API token
4. Copy the token and use it in your configuration

## Security Notes

- Never commit your API token to version control
- Use environment variables to store sensitive credentials
- The `set_toggl_api_token` tool is provided for convenience but environment variables are preferred
- Your API token provides full access to your Toggl account, so keep it secure

## Requirements

- .NET 8.0 or later
- Valid Toggl Track account and API token
