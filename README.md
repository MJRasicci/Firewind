# Firewind

Firewind is a `net10.0` Razor component library for Blazor, built around Tailwind CSS and daisyUI (`fw-` prefixed classes).

It includes:
- A reusable component library (`Source/Firewind`)
- A showcase app for local development (`Source/Firewind.Showcase`)
- Unit tests (`Tests/Firewind.UnitTests`)

## Requirements

- .NET 10 SDK
- (Optional) WASM tools for local showcase work:

```bash
dotnet workload install wasm-tools
```

## Add Firewind To Your Project

This repository is currently set up for project-reference usage.

### Option 1: Project reference (recommended right now)

From your app project:

```xml
<ItemGroup>
  <ProjectReference Include="..\path\to\Firewind\Source\Firewind\Firewind.csproj" />
</ItemGroup>
```

### Option 2: NuGet package

If/when Firewind is published as a package, install it with:

```bash
dotnet add package Firewind
```

## Basic Usage

In a Razor component:

```razor
@using Firewind.Components
@using Firewind.Variant

<FWButton Style="ButtonStyle.Primary" Size="ComponentSize.Large">
    Save
</FWButton>
```

## Tailwind / daisyUI Setup Notes

Firewind expects daisyUI-style utility classes with an `fw-` prefix.

In this repo, the showcase app handles this through Tailwind.MSBuild and the Tailwind input at:
- `Source/Firewind.Showcase/Styles/tailwind.input.css`

If you consume Firewind in another app, make sure your Tailwind pipeline includes:
- The Firewind component sources in content scanning
- daisyUI configured with `prefix: "fw-"`

## Build

```bash
dotnet restore Firewind.slnx
dotnet build Firewind.slnx -c Debug
```

## Run Showcase

```bash
dotnet run --project Source/Firewind.Showcase/Firewind.Showcase.csproj --launch-profile https
```

Default local URLs:
- `https://localhost:7193`
- `http://localhost:5269`

## Test

```bash
dotnet test --solution Firewind.slnx -c Debug /p:TreatWarningsAsErrors=true
```

## Repo Layout

- `Source/Firewind` — component library
- `Source/Firewind.Showcase` — demo/showcase application
- `Tests/Firewind.UnitTests` — unit tests

## Contributing

1. Create a feature branch.
2. Keep changes scoped and testable.
3. Run build + tests before opening a PR.

## License

MIT — see `LICENSE.md`.
