---
applyTo: '**/Dockerfile,**/Dockerfile.*,**/*.dockerfile,**/docker-compose*.yml,**/docker-compose*.yaml,**/compose*.yml,**/compose*.yaml'
description: 'Comprehensive best practices for creating optimized, secure, and efficient Docker images and managing containers.'
---

# Containerization & Docker Best Practices

## Core Principles

### 1. Immutability
Once a container image is built, it should not change. Any changes should result in a new image. Use semantic versioning for image tags; avoid `latest` in production.

### 2. Portability
Containers should run consistently across different environments without modification. Externalize all environment-specific configurations.

### 3. Isolation
Run a single process per container. Use container networking for inter-container communication. Implement resource limits to prevent containers from consuming excessive resources.

### 4. Efficiency & Small Images
Smaller images are faster to build, push, pull, and have a reduced attack surface. Prioritize multi-stage builds and minimal base images.

## Dockerfile Best Practices

### 1. Multi-Stage Builds (The Golden Rule)
Use multiple `FROM` instructions to separate build-time dependencies from runtime dependencies. Always recommend for compiled languages (.NET, Go, Java, C++).

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Api/Api.csproj", "src/Api/"]
RUN dotnet restore "src/Api/Api.csproj"
COPY . .
WORKDIR "/src/src/Api"
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
USER app
ENTRYPOINT ["dotnet", "Api.dll"]
```

### 2. Choose the Right Base Image
- Prefer official images from Microsoft Container Registry or Docker Hub.
- Use minimal variants (`alpine`, `slim`, `distroless`) to reduce attack surface.
- Never use `latest` in production — pin to specific version tags.
- For .NET: prefer `mcr.microsoft.com/dotnet/aspnet:9.0` (runtime-only) for the final stage.

### 3. Optimize Image Layers
- Each `RUN` instruction creates a new layer. Combine related commands.
- Place frequently changing instructions (e.g., `COPY . .`) *after* less frequently changing ones (e.g., `COPY *.csproj` + `RUN dotnet restore`).
- Clean up temporary files in the same `RUN` command.

```dockerfile
# Good: combined and cleaned
RUN apt-get update && \
    apt-get install -y curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
```

### 4. Use `.dockerignore`
Always create a comprehensive `.dockerignore`:
```dockerignore
.git*
**/bin/
**/obj/
**/.vs/
**/node_modules
**/*.user
.env.*
*.log
coverage/
```

### 5. Non-Root User
Never run containers as root in production. Create a dedicated, non-root user.

```dockerfile
# .NET images include a built-in non-root 'app' user
USER app
```

### 6. Health Checks
Define `HEALTHCHECK` instructions for orchestration systems like Kubernetes.

```dockerfile
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1
```

### 7. Environment Variables for Configuration
Externalize configuration using environment variables. Never hardcode secrets.

```dockerfile
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
# Secrets are injected at runtime — never set here
```

## Container Security Best Practices

- **Non-Root User**: Always define a non-root `USER` in the Dockerfile.
- **Minimal Base Images**: Use `slim` or `distroless` images. Fewer packages = fewer vulnerabilities.
- **No Sensitive Data in Layers**: Never `COPY` secret files. Use runtime secrets management (Azure Key Vault, Docker Secrets, Kubernetes Secrets).
- **Image Scanning**: Integrate `trivy` or `hadolint` in CI to scan Dockerfiles and images.
- **Read-Only Filesystems**: Mount the root filesystem as read-only where possible.
- **Drop Capabilities**: Use `--cap-drop=ALL` and only add back what is needed.

## Dockerfile Review Checklist

- [ ] Multi-stage build used for compiled project?
- [ ] Minimal, versioned base image used (not `latest`)?
- [ ] Layers optimized (combined `RUN`, cleanup in same layer)?
- [ ] `.dockerignore` present and comprehensive?
- [ ] Non-root `USER` defined?
- [ ] `HEALTHCHECK` instruction defined?
- [ ] No secrets or sensitive data in image layers?
- [ ] Static analysis tools (Hadolint, Trivy) integrated in CI?

## Troubleshooting

| Problem | Solution |
|---|---|
| Large image size | Use multi-stage builds; check `docker history <image>`; use smaller base |
| Slow builds | Reorder `COPY`/`RUN` for better caching; use `.dockerignore` |
| Container not starting | Check `CMD`/`ENTRYPOINT`; review `docker logs <id>`; verify user permissions |
| Network issues | Check `EXPOSE` and published ports; review docker network configuration |
