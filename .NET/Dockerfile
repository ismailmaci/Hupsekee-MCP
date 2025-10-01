FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ChessMCP.csproj ./
RUN dotnet restore ChessMCP.csproj

COPY Program.cs ./
COPY Models/ ./Models/
COPY Services/ ./Services/
COPY Tools/ ./Tools/

RUN dotnet publish ChessMCP.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out .

# Expose the application (MCP typically uses stdin/stdout but we expose for potential HTTP transport)
EXPOSE 8080

ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

RUN useradd -m -u 1001 mcpuser && chown -R mcpuser:mcpuser /app
USER mcpuser

ENTRYPOINT ["dotnet", "ChessMcpServer.dll"]
