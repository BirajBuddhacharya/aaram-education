# syntax=docker/dockerfile:1

# ── restore (cached until any .csproj changes) ─────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

COPY src/AaramEducation.Core/AaramEducation.Core.csproj src/AaramEducation.Core/
COPY src/AaramEducation.Infrastructure/AaramEducation.Infrastructure.csproj src/AaramEducation.Infrastructure/
COPY src/AaramEducation.Web/AaramEducation.Web.csproj src/AaramEducation.Web/

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore src/AaramEducation.Web/AaramEducation.Web.csproj

# ── build / publish (invalidated by source changes, restore layer reused) ──
FROM restore AS build
COPY src/ src/

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish src/AaramEducation.Web/AaramEducation.Web.csproj \
    -c Release -o /app/publish --no-restore

# EF Core migrations bundle: a self-contained native executable that applies
# pending migrations at container startup (see docker/entrypoint.sh), so the
# runtime image never needs the SDK or the `dotnet ef` tool.
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet tool install --global dotnet-ef --version 9.0.0 \
    && ~/.dotnet/tools/dotnet-ef migrations bundle \
    --project src/AaramEducation.Infrastructure/AaramEducation.Infrastructure.csproj \
    --startup-project src/AaramEducation.Web/AaramEducation.Web.csproj \
    --self-contained -r linux-x64 \
    -o /app/publish/efbundle

# ── runtime ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY docker/entrypoint.sh .

RUN chmod +x efbundle entrypoint.sh \
    && mkdir -p wwwroot/uploads/avatars wwwroot/uploads/videos wwwroot/uploads/notes \
    && chown -R app:app wwwroot/uploads
USER app

EXPOSE 8080
ENTRYPOINT ["./entrypoint.sh"]
