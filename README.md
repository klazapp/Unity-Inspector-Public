# Unity Inspector Package

The Unity Inspector package provided by `com.Klazapp` enhances the default Unity Inspector with added functionalities for better visibility and interaction with object properties in the Unity Editor. This package includes attributes and editors that allow developers to add detailed notes, handle script editing directly from the inspector, and toggle views for custom visualization of components.

## Features

- **Script Headers**: Add descriptive headers to your scripts with `ScriptHeader` attributes.
- **Todo Headers**: Mark areas of your code needing attention with `TodoHeader` attributes.
- **Note Attributes**: Attach notes directly to fields or properties to describe behavior or requirements.
- **ReadOnly Fields**: Mark fields as read-only in the inspector to prevent unwanted changes.
- **Enhanced Inspector Views**: Toggle between custom, classic, and debug views within the inspector to suit your workflow.
- **Script Interaction**: Directly open and edit scripts from the inspector.
- **Property Grouping**: Organize properties visually by parent and child classes for clarity.

## Dependencies

- Unity 2020.3 LTS or later
- .NET Standard 2.0 or later

## Compatibility
| Compatibility | URP | BRP | HDRP |
|---------------|-----|-----|------|
| Compatible    | ✔️   | ✔️   | ✔️    |

## Installation

1. Download the latest release from the [Releases page](#).
2. Import the package into your Unity project by navigating to `Assets -> Import Package -> Custom Package` and selecting the downloaded file.

## Usage

### Script Headers

Apply the `ScriptHeader` attribute to your classes to add a descriptive header above the class in the Unity Inspector.

```csharp
[ScriptHeader("This is a descriptive header for MyClass.")]
public class MyClass : MonoBehaviour
{
}
```

### Todo Headers

Use the `TodoHeader` to mark scripts or sections of code that need future revisions.

```csharp
[TodoHeader("This needs to be refactored for better performance.")]
public class MyComponent : MonoBehaviour
{
}
```

### ReadOnly Fields

Prevent modifications to fields in the Unity Editor by applying the `ReadOnlyAttribute`.

```csharp
public class PlayerSettings : MonoBehaviour
{
    [ReadOnly]
    public int playerHealth = 100;
}
```

### Viewing and Editing Scripts

The inspector extensions allow you to view and edit scripts directly from the inspector without searching through your project folders.

## To-Do List

- Add support for array and list visual enhancements.
- Implement color-coded notes for better visibility.
- Increase customization options for developer preferences.

## License

This package is released under the MIT License. See the LICENSE file in the repository for full details.
