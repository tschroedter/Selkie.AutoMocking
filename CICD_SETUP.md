# CI/CD Workflow Setup Summary

This document summarizes the CI/CD workflow setup for the Selkie.AutoMocking project.

## What Was Added

### 1. GitHub Actions Workflow
**Location**: `.github/workflows/build-test-deploy.yml`

A comprehensive CI/CD workflow that automates:
- Building the solution on every push and pull request
- Running all unit tests with result reporting
- Automatic versioning using format `v0.1.x` (x = build number)
- Creating NuGet packages
- Publishing to NuGet.org on tagged releases
- Creating GitHub releases with attached packages

### 2. Documentation
**Location**: `.github/workflows/`

Three comprehensive documentation files:
- **QUICK_START.md** - Quick start guide for immediate usage
- **README.md** - Detailed documentation with troubleshooting
- **create-release.sh** - Helper script to create release tags easily

## Version Numbering Scheme

The workflow implements automatic versioning:

### Automatic Versioning (Default)
- **Format**: `v0.1.x` where `x` is the GitHub Actions run number
- **Example**: Run #25 produces version `0.1.25`
- **Trigger**: Automatic on every push to main/master

### Manual Versioning (For Releases)
- **Format**: `vX.Y.Z` (e.g., `v0.1.5` or `v1.0.0`)
- **Usage**: Create and push a git tag
- **Example**: 
  ```bash
  git tag v0.1.5
  git push origin v0.1.5
  ```
- **Result**: Builds, tests, publishes to NuGet.org, creates GitHub Release

## How It Works

### On Pull Request
```
Developer creates PR → Workflow runs → Build + Test
```
- Only builds and tests
- No deployment or package creation
- Test results published in PR checks

### On Push to Main/Master
```
Push to main → Workflow runs → Build + Test + Package → Artifact stored
```
- Builds and tests
- Creates NuGet package with auto-incremented version
- Stores package as workflow artifact (30 days retention)
- Does NOT publish to NuGet.org

### On Tagged Release
```
Create tag v0.1.5 → Push tag → Workflow runs → 
Build + Test + Package → Publish to NuGet.org + GitHub Release
```
- Builds and tests with specified version
- Creates NuGet package
- Publishes to NuGet.org (if NUGET_API_KEY is configured)
- Creates GitHub Release with package and auto-generated notes

## Version Updates in Code

The workflow automatically updates the version in:
- `src/Selkie.AutoMocking/Selkie.AutoMocking.csproj` (the `<Version>` element)

Example before workflow:
```xml
<Version>0.0.36</Version>
```

Example after workflow (build #50):
```xml
<Version>0.1.50</Version>
```

## Configuration Required

### Optional: NuGet Publishing
To enable automatic publishing to NuGet.org:

1. Create a NuGet API key at https://www.nuget.org/account/apikeys
2. Add it as a GitHub repository secret named `NUGET_API_KEY`
3. See `.github/workflows/QUICK_START.md` for detailed instructions

**Note**: The workflow will skip NuGet publishing if the secret is not configured, but all other steps will still run.

## Quick Start

### For Day-to-Day Development
```bash
# Make changes
git add .
git commit -m "Your changes"
git push origin main
```
The workflow automatically builds, tests, and creates a package artifact.

### For Creating a Release
```bash
# Easy way - use the helper script
.github/workflows/create-release.sh 0.1.5

# Manual way
git tag v0.1.5
git push origin v0.1.5
```
The workflow builds, tests, publishes to NuGet.org, and creates a GitHub Release.

## Security

- **CodeQL Analysis**: Passed with no security alerts
- **Explicit Permissions**: Workflow uses minimal required permissions:
  - `contents: write` - For creating releases and uploading assets
  - `checks: write` - For publishing test results
  - `packages: write` - For publishing packages
- **Secret Protection**: NuGet API key stored as encrypted GitHub secret

## Workflow Triggers

The workflow runs on:
- ✅ Push to `main` branch
- ✅ Push to `master` branch
- ✅ Pull requests to `main` or `master`
- ✅ Tags matching `v*` pattern (e.g., `v0.1.5`, `v1.0.0`)
- ✅ Manual trigger via GitHub Actions UI

## Features

- **Test Reporting**: Automatic test result reporting in the Actions UI
- **Artifact Storage**: NuGet packages stored for 30 days
- **Smart Publishing**: Skips duplicate versions on NuGet.org
- **Release Notes**: Auto-generated release notes on GitHub
- **Cross-platform**: Runs on Ubuntu (can be changed to Windows/macOS)

## Next Steps

1. **Review Documentation**: Read `.github/workflows/QUICK_START.md`
2. **Test the Workflow**: Push a commit to trigger the workflow
3. **Configure NuGet**: Add `NUGET_API_KEY` secret for publishing
4. **Create First Release**: Use the helper script or create a tag manually

## Files Overview

```
.github/
└── workflows/
    ├── build-test-deploy.yml    # Main workflow file
    ├── QUICK_START.md           # Quick start guide
    ├── README.md                # Detailed documentation
    └── create-release.sh        # Helper script for releases
```

## Support

For detailed information and troubleshooting:
- See `.github/workflows/QUICK_START.md` for quick start instructions
- See `.github/workflows/README.md` for comprehensive documentation
- Check the "Actions" tab in GitHub for workflow runs and logs
