#!/bin/sh
set -e

# Applies pending EF Core migrations via the bundle built into the image
# (see Dockerfile: `dotnet ef migrations bundle`), then hands off to the app.
CONNSTR="Server=${DB_HOST};Port=${DB_PORT};Database=${DB_NAME};User=${DB_USER};Password=${DB_PASSWORD};"
/app/efbundle --connection "$CONNSTR"

exec dotnet /app/AaramEducation.Web.dll
