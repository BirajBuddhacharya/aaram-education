# Setup Guide

## Prerequisites

| Tool | Version | Install |
|---|---|---|
| .NET SDK | 8.0+ | https://dotnet.microsoft.com/download |
| SQL Server | 2019+ or LocalDB | Included with Visual Studio, or install SQL Server Express |
| Visual Studio / Rider / VS Code | Latest | — |
| EF Core CLI | 8.0 | `dotnet tool install --global dotnet-ef` |

For dev you can use SQLite instead of SQL Server — see step 3.

---

## 1. Clone & Restore

```bash
git clone <repo-url>
cd AaramEducation
dotnet restore
dotnet build
```

---

## 2. Configure Connection String

Edit `src/AaramEducation.Web/appsettings.Development.json`:

**SQL Server (LocalDB):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AaramEducation;Trusted_Connection=True;"
  }
}
```

**SQLite (zero-setup dev):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=aaram.db"
  },
  "UseInMemory": false,
  "UseSQLite": true
}
```

Then in `Program.cs` toggle:
```csharp
// SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(conn));

// SQLite
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlite(conn));
```

---

## 3. Apply Migrations

```bash
dotnet ef database update \
  --project src/AaramEducation.Infrastructure \
  --startup-project src/AaramEducation.Web
```

This creates all tables from `ERD.md`.

To create a new migration after entity changes:
```bash
dotnet ef migrations add <MigrationName> \
  --project src/AaramEducation.Infrastructure \
  --startup-project src/AaramEducation.Web
```

---

## 4. Seed Dev Data

`DbSeeder.cs` runs on app start in Development environment.  
It creates:
- 1 Admin user: `admin@aaram.edu` / `Admin@1234`
- 2 Tutor users: `tutor1@aaram.edu` / `Tutor@1234`
- 3 Student users: `student1@aaram.edu` / `Student@1234`
- 2 published courses with modules, lessons, videos, study notes, quizzes
- Sample guestbook entries (approved and pending)
- Badge definitions

---

## 5. Run

```bash
dotnet run --project src/AaramEducation.Web
```

Open: `https://localhost:5001`

For hot reload during development:
```bash
dotnet watch run --project src/AaramEducation.Web
```

---

## 6. Upload Folder

Ensure the uploads folder exists and is writable:

```bash
mkdir -p src/AaramEducation.Web/wwwroot/uploads/videos
mkdir -p src/AaramEducation.Web/wwwroot/uploads/notes
```

In production: configure a CDN or Azure Blob Storage instead.

---

## 7. Running Tests

```bash
dotnet test tests/AaramEducation.Tests
```

---

## Project Structure Quick Reference

```
src/AaramEducation.Web/          ← Start here: Controllers, Views, wwwroot
src/AaramEducation.Core/         ← Entities + interfaces (no EF dep)
src/AaramEducation.Infrastructure/ ← DbContext, Migrations, Repositories
tests/AaramEducation.Tests/      ← xUnit tests
docs/                            ← Architecture, DB design, module docs
ERD.md                           ← Mermaid ER diagram
```
