# Chess.com MCP Server

A Model Context Protocol (MCP) server that provides chess player statistics from Chess.com.

## Prerequisites
- .NET 9.0+
- Docker
- VS Code with MCP support

## 🛠️ Development Setup

1. **Clone and setup**:
   ```bash
   git clone <repository-url>
   cd chessdotcom-mcp-server/.NET
   ```

2. **How to Run**:
   
   To run application:
   ```bash
   dotnet run
   ```


    Build the project:
    ```bash
    dotnet build
    ```

## 📋 VS Code MCP Configuration

The `.vscode/mcp.json` is pre-configured with two options:

- **`chess-mcp-dev`** - Runs dotnet directly, for local development
- **`chess-mcp`** - Uses a docker image, to be used everywhere

1. **Build Docker image**:
   ```bash
   cd .NET
   docker build -t chess-mcp-server:latest .
   ```

## 🔧 Available Tools

**`get_chess_player_stats`**
- **Input**: Chess.com username
- **Output**: Player ratings, records, and statistics across all game modes
    ```
    Chess Player Statistics Summary for hikaru:
    • Rapid: 2839 (W:201 L:67 D:209)
    • Blitz: 3384 (W:32466 L:5193 D:4076)  
    • Bullet: 3348 (W:15697 L:2163 D:1004)
    • Daily: 2239 (W:73 L:11 D:4)
    ```
- **Usage**:
  - "Get chess stats for Magnus Carlsen"
  - "What's Hikaru's chess rating?"
  - "Show me chess statistics for any Chess.com username"

## MCP connection configuration
This list provides various methods to connect to the same MCP server. Note that http is only available if you configure your MCP is an http-based server.
During development, use the dotnet command for fast testing.

```json
{
  "servers": {
    "chess-stats-dotnet": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "path/to/hupsekee-mcp/.NET/ChessMCP.csproj"
      ],
      "env": {
        "DOTNET_ENVIRONMENT": "Production"
      }
    },
    "chess-dotnet": {
      "command": "docker",
      "args": [
        "run",
        "--rm",
        "-i",
        "--name",
        "hupsekee-mcp-dotnet",
        "chess-mcp:latest"
      ]
    },
    "chess-dotnet-compose": {
      "command": "docker-compose",
      "args": [
        "-f",
        "path/to/hupsekee-mcp/.NET/docker-compose.yml",
        "run",
        "--rm",
        "chess-mcp-server"
      ],
      "cwd": "path/to/hupsekee-mcp/.NET"
    }
  }
}
```
