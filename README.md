# PlainApp

## Overview
PlainApp is a lightweight WPF planner application that displays user plans/tasks in a rich `DataGrid` with filtering, progress visualization and localization support.

## Achievements
- Localized UI strings via resource files (`Languages/Strings.resx`) and a `LanguageManager` service.
- Localized DateTime display with translated weekday names using `Services/DateTimeLocalizer.cs`.
- A responsive `MyPlansView` (`Views/MyPlansView.xaml`) with: filtering, hide/unhide columns, progress bars, and sortable date columns.

## Recent additions (what was added)
- `Services/DateTimeLocalizer.cs`: formats `DateTime` values using translated day names from `LanguageManager`.
- `Views/MyPlansView.xaml`: both `CreatedDate` and `DueDate` columns now use the `DateTimeLocalizer` and include `SortMemberPath` so sorting works on the underlying `DateTime` values.
- Resource key `DateFormat_Full` in `Languages/Strings.resx` provides the format pattern used for full datetime display.

## How to use the recent additions
- Change language at runtime: call `LanguageManager.Instance.ChangeLanguage("<culture-code>")` (for example `"ja-JP"` or `"vi-VN"`). The UI and localized weekday names in the datetime columns will update.
- To adjust the displayed datetime format, edit the `DateFormat_Full` value in `Languages/Strings.resx` or bind a different format key in `MyPlansView.xaml`.
- Sorting by `CreatedDate` or `DueDate` is enabled in the UI (click the column headers) and sorts by the underlying `DateTime` values.

## Future work (possible additions)
- Localize month names similarly by extending the converter to use `MonthNames` / `AbbreviatedMonthNames`.
- Add editable detail dialogs for plans, cloud sync, user accounts, and unit/integration tests.
- Improve accessibility and add keyboard navigation and automated UI tests.

## Build & Run
- Open the solution in Visual Studio (targets .NET 9) and run. No special setup is required for the recent localization features beyond the resource files.

---
Short and focused — update resource files or the `DateTimeLocalizer` if you need different date formats or additional translations.
