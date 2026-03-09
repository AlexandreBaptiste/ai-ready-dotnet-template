# ── Build stage ───────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first (layer cache for NuGet restore)
COPY ["global.json", "global.json"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["Solution.slnx", "Solution.slnx"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Api/Api.csproj", "src/Api/"]

RUN dotnet restore "src/Api/Api.csproj"

# Copy everything else and publish
COPY . .
RUN dotnet publish "src/Api/Api.csproj" \
    --no-restore \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Use the built-in non-root user provided by the .NET runtime image
USER app

COPY --from=build /app/publish .

EXPOSE 40010
EXPOSE 50010

ENTRYPOINT ["dotnet", "Api.dll"]
