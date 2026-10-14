# PlainApp

PlainApp is a lightweight WPF planner application (desktop) that stores plans and tasks on disk, provides a localized UI, and offers a data-driven `DataGrid` experience with sorting, filtering and progress visualization.

This repository targets .NET 9 and C# 13.

## Key features
- Localized UI via resource files in `Languages/` and a `LanguageManager` service.
- Localized `DateTime` display using `Services/DateTimeLocalizer.cs` (translates weekday names and respects format strings from resources).
- Rich `MyPlansView` (`Views/MyPlansView.xaml`) with: filtering, hide/unhide columns, progress bars, and sortable date columns.
- Simple file-based persistence under an application `Data` folder (see `Services/FileManager.cs`).
- JSON serialization with polymorphic handling for external sources via `Services/JSONUtils.cs`.

## What's new / Up-to-date notes
- Project targets: .NET 9 and uses C# 13 language features where applicable.
- `Services/JSONUtils.cs`: improved error handling for deserialization to avoid swallowing fatal exceptions; JSON parsing errors are logged.
- `Services/FileManager.cs`: directory creation was made idempotent by using `Directory.CreateDirectory(...)` (safer and avoids races). `SavePlan` and `SaveTask` now respect the result of plan directory creation and return early on failure.
- Date/time localization improvements: `DateTimeLocalizer` and resource key `DateFormat_Full` control how datetimes are rendered in the main views.

If you need a specific changelog for past commits, run `git log --oneline` locally to see the commit history.

## Getting started (build & run)
Prerequisites:
- .NET 9 SDK (install from dotnet.microsoft.com)
- Visual Studio 2022/2023 or a compatible IDE with WPF tooling

Steps:
1. Clone the repository: `git clone https://github.com/KangDu1506/PlainApp.git`
2. Open the solution (`.sln`) in Visual Studio or load the folder in an editor that supports .NET projects.
3. Restore packages (Visual Studio does this automatically) or run `dotnet restore` from the solution directory.
4. Build the solution: `dotnet build` or via Visual Studio Build.
5. Run the WPF application from Visual Studio (F5) or `dotnet run --project <project-path>` for the startup project.

## Configuration & data storage
- The application stores its data under the application's base directory in a `Data/` folder. Plan folders use the pattern `Data/plan_<UID>/` and contain subfolders for `Tasks`, `External Source`, and `External Attachments`.
- JSON serialization options are configured in `Services/JSONUtils.cs` (polymorphism for external sources uses the `Type` discriminator).

## Usage notes
- Change UI language at runtime by calling `LanguageManager.Instance.ChangeLanguage("<culture-code>")`, for example `"ja-JP"` or `"vi-VN"`.
- Adjust full datetime formatting by editing the `DateFormat_Full` key in `Languages/Strings.resx`.

## Persistence & external sources
- Plans, tasks and external source metadata are stored as JSON files in the plan folder. `FileManager` provides helper methods `SavePlan`, `SaveTask`, and `SaveExternalSource` (the latter should sanitize filenames and store metadata; review implementation before storing arbitrary remote content).

## Troubleshooting
- If you encounter issues writing files, check application permissions and whether the process has write access to the install directory. Consider configuring a dedicated data directory if the install location is protected.
- For JSON parsing issues, check console logs where `JSONUtils` reports parsing errors.

## Contributing
- Fork the repository and open a pull request. Keep changes small and focused.
- Add unit tests when adding logic that can be validated automatically.

## Development tips
- Prefer `Directory.CreateDirectory(...)` rather than `Directory.Exists(...)` followed by `CreateDirectory` to avoid TOCTOU races.
- Sanitize any user-provided values used for filenames using `Path.GetInvalidFileNameChars()` before writing files.
- Use structured logging instead of `Console.WriteLine` when moving to production.

## Developer guide

This section describes the overall architecture, the purpose of the main files and folders, and common conventions used across the codebase.

- Architecture
  - The app follows a lightweight MVVM pattern for WPF: `Views/` hold XAML UI, `ViewModels/` contain presentation logic, and `Models/` define data objects.
  - `Services/` contains infrastructure and helper services (persistence, JSON utilities, localization helpers). UI components that are reused live in `Partials/`.
  - Runtime data is stored as files under the `Data/` folder using simple JSON documents; polymorphic external sources are handled via a `Type` discriminator.

- Project structure (purpose of key folders/files)
  - `Views/` — XAML views shown to users (e.g., `MyPlansView.xaml`).
  - `ViewModels/` — view models bound to the views (expose commands, properties, filtering logic).
  - `Models/` — domain models (`PlanModel`, `TaskModel`, external source models).
  - `Services/` — application services such as `FileManager.cs` (disk IO), `JSONUtils.cs` (serialization settings), `LanguageManager` and `DateTimeLocalizer`.
  - `Partials/` — reusable UI pieces (`Header`, `SideMenu`).
  - `Languages/` and resource `.resx` files — localization strings and date/time formats (`DateFormat_Full`).
  - `Enums/` — shared enum types used by models and services.
  - `App.xaml` / `MainWindow.xaml` — WPF startup and shell.
  - `Data/` (created at runtime) — persisted plans and tasks. Plan folders follow the `Data/plan_<UID>/` pattern and contain `Tasks`, `External Sources`, `External Attachments`.

- Conventions and notes for contributors
  - Filenames and directory creation:
    - Plan directories use `plan_<UID>` and task files use `task_<UID>.json`.
    - Prefer `Directory.CreateDirectory(...)` (idempotent) over existence checks to avoid TOCTOU issues.
    - Sanitize user-provided strings for filenames with `Path.GetInvalidFileNameChars()`.
  - Serialization:
    - Use `JSONUtils.DefaultOption` for (de)serialization to preserve polymorphism (the `Type` discriminator is used for `AExternalSource`).
    - Keep JSON indented for ease of debugging.
  - Localization and formatting:
    - Change languages via `LanguageManager.Instance.ChangeLanguage("<culture-code>")`.
    - Adjust full datetime presentation with the `DateFormat_Full` resource key.
  - Threading and IO:
    - Avoid blocking the UI thread on file IO; run disk operations on background threads or tasks.
  - Code style and practices:
    - Project targets .NET 9 and C# 13 — prefer modern language features where they improve clarity.
    - Add small, focused unit tests when introducing business logic that can be validated.

## License
See the repository `LICENSE` file (if present) or add one to clarify project licensing.

---
This README provides a concise but detailed overview to help new contributors and maintainers get up to speed. Update the sections above as the project evolves.
