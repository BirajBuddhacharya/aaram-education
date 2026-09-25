# WebForms Migration — Context & Fixes

Commit `cb2eb42` ("refactor: rewrite on web-form") converted this project from
**ASP.NET Core MVC on .NET 10** to **WebForms on .NET Framework 4.8 + EF6 + MySQL**.
The conversion was incomplete: the project would not load in Visual Studio, would not
compile, and would not run. This document records what was wrong and how it was fixed.

`cb2eb42^` still contains the complete, working MVC version (Controllers, Views,
ViewModels) if that history is ever needed.

> The other files in `docs/` were written for the pre-`cb2eb42` stack and still
> describe ASP.NET Core MVC, EF Core 8 and SQL Server. They are stale.

---

## 1. Project & solution configuration

The project could not be loaded at all. Visual Studio reported, in sequence,
`Object reference not set to an instance of an object`, `The method or operation is
not implemented`, and eventually `Unable to open the Web site '...\*.csproj'` with a
telltale trailing backslash.

**Root cause: the wrong Web flavor GUID.** `{E24C65DC-7377-472B-9ABA-BC803B73C61A}`
is **ASP.NET Web Site** — the folder-based, project-less kind. Visual Studio was
trying to open the `.csproj` *path* as a website *directory*.

Correct values for a WebForms Web Application Project:

| Location | GUID | Meaning |
|---|---|---|
| `.csproj` → `<ProjectTypeGuids>` | `{349C5851-65DF-11DA-9384-00065B846F21};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}` | Web Application flavor, then C# |
| `.csproj` → `ProjectExtensions/FlavorProperties GUID` | `{349c5851-65df-11da-9384-00065b846f21}` | must match the flavor above |
| `.sln` project line | `{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}` | plain C# — the flavor never appears here |

A `ProjectSection(WebsiteProperties)` block appearing in the `.sln` is a symptom of
the wrong GUID, not a fix.

Also corrected in `AaramEducation.Web.csproj`:

- **`<ProjectExtensions>` was missing entirely.** Every VS-created web project has it.
- **Wildcard globs replaced with explicit items** — 61 `<Compile>` and 32 `<Content>`
  entries with `DependentUpon` metadata, so designer files nest under their `.aspx`.
  The legacy project system expects explicit items.
- **`VSToolsPath` is now self-correcting.** It tries what VS sets, then
  `$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v17.0`, then
  `$(VsInstallRoot)\MSBuild\...`, each only if the previous path lacks
  `Microsoft.WebApplication.targets`. This avoids hardcoding a VS edition.

### Gotcha: the unloaded flag is sticky

Once a project fails to load, Visual Studio records it as unloaded in
`.vs/<solution>/v17/.suo` and **replays that on every open**, even after the cause is
fixed. Delete the `.vs` folder to clear it. Never edit a `.csproj` while the solution
is open in VS — a failed live reload sets this flag.

---

## 2. Designer files

**28 `.aspx` pages had zero `.designer.cs` files.** In a Web Application Project,
every `<asp:TextBox ID="txtEmail" runat="server">` needs a matching field declaration
in `Login.aspx.designer.cs`; the codebehind references it directly. Without them the
project cannot compile — roughly 190 × `CS0103: The name 'txtEmail' does not exist in
the current context`.

Generated: **29 files, 192 control declarations** (28 pages + `Site.Master`).

### `tools/gen-aspx-designer.ts`

Visual Studio normally owns these files. Run this after changing any control `ID`:

```sh
bun tools/gen-aspx-designer.ts            # regenerate
bun tools/gen-aspx-designer.ts --check    # CI: non-zero exit if stale
```

It mirrors VS's rules: controls inside an `ITemplate` (`<ItemTemplate>` etc.) are
skipped, because they live in a child naming container and the runtime never assigns
them to a page field — reach those with `FindControl` instead. Elements with
`runat="server"` but no `ID` are also skipped; the runtime auto-names them.

---

## 3. `App_Code/` → `Helpers/`

`App_Code` is a **Web Site** project concept. In a Web Application Project the ASP.NET
runtime still treats the folder specially and dynamically compiles its contents into a
separate assembly, producing duplicate types.

Worse, the original `.csproj` had `Exclude="App_Code\**"` on its compile glob — so
`BasePage`, the base class of every page, was **never compiled into the assembly at
all**. Renaming the folder fixed both problems.

---

## 4. EF Core → EF6 differences

The rewrite kept EF Core idioms that do not exist in EF6.

| EF Core (was) | EF6 (now) | Notes |
|---|---|---|
| `AddRange(a, b, c)` | `AddRange(new[] { a, b, c })` | EF6 has no `params` overload, only `IEnumerable<T>`. 8 call sites. |
| `.WithOptionalDependent(...)` | `.WithOptional(...)` | Does not exist on `RequiredNavigationPropertyConfiguration`. |
| `using MySql.Data.Entity` | `using MySql.Data.EntityFramework` | Namespace renamed between the 6.x and 8.x connector packages. |
| `[assembly: DbConfigurationType(...)]` | `[DbConfigurationType(...)]` on the `DbContext` class | The attribute is `AttributeTargets.Class`; assembly scope does not compile. |

### The 1:0..1 relationship

`CourseProgress` ↔ `Enrollment` needed remodelling. **EF6 requires the dependent side
of a one-to-zero-or-one to use its foreign key as its own primary key** — it cannot
have a separate PK plus an independent FK column.

`CourseProgress` is therefore keyed on `EnrollmentId`, with
`HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)` because the seeder assigns
that value rather than letting the database generate it. `CourseProgressId` remains as
an ordinary column and is referenced nowhere.

This preserves both things the code depends on: the `Enrollment.CourseProgress`
navigation (three `.Include()` calls) and direct `cp.EnrollmentId == id` queries.

---

## 5. The async deadlock

**Symptom:** pages hang forever on "loading" with no exception and nothing in any log.
Only pages that touch no repository (`Default.aspx`) render.

**Cause:** the 28 pages call repositories synchronously — **71 `.Result` / `.Wait()`
calls**. Classic ASP.NET has an `AspNetSynchronizationContext` permitting one thread in
the request at a time. A page blocking on `.Result` while the repository's `await`
tries to resume on that same context deadlocks permanently.

ASP.NET Core has no SynchronizationContext, so this was harmless in the .NET 10
original and became fatal on WebForms.

**Fix:** every `await` in `AaramEducation.Infrastructure` ends with
`.ConfigureAwait(false)` — **all 75 of them**.

> **Rule for new code:** any new repository `await` must have `.ConfigureAwait(false)`.
> A method that merely *returns* a Task without awaiting
> (`=> _db.X.FirstOrDefaultAsync(...)`) needs nothing — EF6's own async methods use
> `ConfigureAwait(false)` internally. If a page hangs with no error, check this first.

The proper fix is async pages (`Async="true"` + `RegisterAsyncTask`), but that is a
28-page change.

---

## 6. `<script runat="server">`

In WebForms, `runat="server"` on a `<script>` tag does **not** resolve the URL — it
declares a **server-side code block**, and `src` names a file to compile as source.
`Site.Master` had three, so ASP.NET fed `jquery.min.js` and
`bootstrap.bundle.min.js` to the C# compiler.

```html
<!-- wrong: compiles the .js as C# -->
<script src="~/wwwroot/js/site.js" runat="server"></script>

<!-- right -->
<script src='<%= ResolveUrl("~/wwwroot/js/site.js") %>'></script>
```

`<link runat="server">` **is** legitimate — it maps to a real `HtmlLink` control that
resolves `~/`. Only `<script>` is special-cased.

---

## 7. Error handling

`Application_Error` called `Server.ClearError()` and redirected to `~/Shared/Error.aspx`
unconditionally. This destroyed every exception before it could be seen, and when the
error page itself failed it redirected to itself forever
(`ERR_TOO_MANY_REDIRECTS`, then `0x800704CD` once the browser gave up).

It now logs the full inner-exception chain to `%TEMP%\aaram-startup-error.log` (append
mode, shared with `Application_Start`), skips the redirect when the failing request *is*
the error page so ASP.NET renders real detail, skips it when the client has
disconnected, and uses `Redirect(url, false)` + `CompleteRequest()` to avoid
`ThreadAbortException`.

---

## 8. Configuration & secrets

Connection details resolve in this order:

1. **Real environment variables** — `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`,
   `DB_PASSWORD`, `DB_SSLMODE` (optional, defaults to `Required`)
2. **`.env` at the repository root** — gitignored; same keys
3. **`web.config` → `DefaultConnection`** — fallback only, holds a harmless `localhost`
   default. Never put real credentials here; the file is tracked.

`src/AaramEducation.Infrastructure/Data/DbConnectionString.cs` implements this. It
walks up from `AppDomain.CurrentDomain.BaseDirectory` to find `.env`, since `bin/` sits
several levels below the repository root. Real environment variables always win over
the file. No NuGet dependency — the parser is about a dozen lines.

Edit `DbConnectionString.cs` only to add a *new setting*, never to change a value.

**`SslMode`:** MySQL 8+/9 defaults to `caching_sha2_password`, which cannot complete a
first handshake over an unencrypted connection. Use `Preferred` (TLS without
certificate validation — works with a self-signed local cert) or keep
`AllowPublicKeyRetrieval=true`, which is always sent.

---

## 9. Database

The existing `aaram_education` database was built by EF Core and was **structurally
incompatible** with EF6. It was dropped and recreated from the EF6 model.

`CreateDatabaseIfNotExists` does **not** validate an existing schema against the model,
so this failed at query time rather than startup — the app booted fine and then threw
`FormatException: Input string was not in a correct format` on the first page that read
a user.

**EF Core stored enums as strings; EF6 stores them as integers.** Eight columns:

```
Users.Role                        longtext  →  "Admin", "Tutor", "Student"
Enrollments.EnrollmentStatus      longtext
GuestbookEntries.ModerationStatus longtext
CourseProgresses.Status           longtext
LessonProgresses.Status           longtext
ModuleProgresses.Status           longtext
QuizAttempts.AttemptStatus        longtext
QuizQuestions.QuestionType        longtext
```

`Convert.ToInt32("Admin")` is the `FormatException`. No configuration reconciles the
two. The database also carried EF Core's `__EFMigrationsHistory` table.

If the schema and model ever diverge again, drop the database and let
`Application_Start` rebuild and reseed it.

### Seeded accounts

| Email | Password | Role |
|---|---|---|
| `admin@aaram.edu` | `Admin@1234` | Admin |
| `tutor1@aaram.edu`, `tutor2@aaram.edu` | `Tutor@1234` | Tutor |
| `student1@aaram.edu` … `student3@aaram.edu` | see `DbSeeder.cs` | Student |

---

## 10. Environment

- **Visual Studio 2022** with the **ASP.NET and web development** workload. This
  supplies `Microsoft.WebApplication.targets`; the project will not build without it.
- **Windows only.** `System.Web`, WebForms and IIS Express do not run on .NET Core
  or Linux.
- **Local disk required.** IIS Express cannot read `web.config` or its own
  `applicationhost.config` from an SMB-mapped drive — it fails with
  `HTTP 500.19`, `0x80070001`, "Cannot read configuration file", and a `\\?\Z:\` path.
  Develop wherever you like, but run from a local drive.

---

## 11. Known remaining issues

- **`Dockerfile` and `docker/entrypoint.sh` are dead.** They target
  `dotnet/sdk:10.0`, `dotnet publish` and `dotnet ef migrations bundle` — EF Core on
  .NET 10. This stack is .NET Framework 4.8 + EF6. Running in Docker would require
  Windows containers (`dotnet/framework/aspnet:4.8`). `docker-compose.yml`'s port
  5287 is likewise a leftover.
- **Two rival quiz implementations.** `Courses/Quiz.aspx` and `Quiz/Take.aspx` both
  take quizzes and had drifted to different entity member names. Worth consolidating.
- **`src/AaramEducation.Tests` is not in the solution** and still contains EF Core
  idioms (`AddRange` with multiple arguments) and a reference to `CourseProgressId`.
  It does not build as part of the solution.
- **`CourseProgressId`** is now an unused column — see §4.
- **`Global.asax` creates `~/uploads` directories** that nothing reads or writes; the
  rewrite dropped the upload features.
- **`AttemptStatus` has no `Passed`/`Failed`.** It is `InProgress, Submitted, Graded`;
  pass/fail is derived by comparing `ScoreAchieved` against `Quiz.PassingScore`.
