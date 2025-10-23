# GitHub Actions Workflows

## Build, Test and Deploy Workflow

This workflow automates the build, test, and deployment process for the Selkie.AutoMocking project.

### Workflow Triggers

The workflow runs on:
- **Push to main/master branch**: Builds, tests, and creates NuGet packages
- **Pull Requests**: Builds and tests only (no deployment)
- **Git Tags** (v*): Builds, tests, packages, and publishes to NuGet.org and creates GitHub releases
- **Manual Trigger**: Can be run manually via GitHub Actions UI

### Version Numbering

The workflow uses an automatic versioning scheme:

- **Automatic versioning**: `v0.1.x` where `x` is the GitHub Actions run number
- **Manual versioning**: Create a git tag with format `v0.1.5` to override the automatic version

#### Example: Creating a Manual Release

```bash
# Tag the current commit with a specific version
git tag v0.1.5
git push origin v0.1.5
```

This will trigger the workflow, build version `0.1.5`, and publish it to NuGet.org.

### Workflow Steps

1. **Checkout code**: Fetches the repository with full history
2. **Setup .NET**: Installs .NET 8.0 SDK
3. **Calculate version**: Determines version number (automatic or from tag)
4. **Update version**: Updates the version in `Selkie.AutoMocking.csproj`
5. **Restore dependencies**: Restores NuGet packages
6. **Build**: Compiles the solution in Release configuration
7. **Test**: Runs all tests and generates test reports
8. **Pack**: Creates NuGet package (on main/master push or tags only)
9. **Upload artifact**: Saves NuGet package as workflow artifact
10. **Push to NuGet.org**: Publishes to NuGet (on tags only, requires secret)
11. **Create GitHub Release**: Creates a release with the package (on tags only)

### Required Secrets

To enable automatic publishing to NuGet.org, add the following secret to your repository:

- **NUGET_API_KEY**: Your NuGet.org API key
  1. Go to https://www.nuget.org/account/apikeys
  2. Create a new API key with "Push" permission
  3. Add it to GitHub repository secrets: Settings → Secrets and variables → Actions → New repository secret
  4. Name: `NUGET_API_KEY`, Value: your API key

### Testing Locally

You can test the version update logic locally:

```bash
# Simulate version update
BUILD_NUMBER=123
VERSION="0.1.${BUILD_NUMBER}"
sed -i "s/<Version>.*<\/Version>/<Version>${VERSION}<\/Version>/" ./src/Selkie.AutoMocking/Selkie.AutoMocking.csproj
```

### Viewing Test Results

Test results are automatically published in the GitHub Actions UI:
1. Go to the "Actions" tab
2. Click on a workflow run
3. View "Test Results" in the summary

### Artifacts

Built NuGet packages are available as workflow artifacts for 30 days:
1. Go to the "Actions" tab
2. Click on a workflow run
3. Download the "nuget-package" artifact from the Artifacts section

### Example Workflow Runs

- **Pull Request**: Build + Test only
- **Push to main**: Build + Test + Package (artifact only)
- **Push tag v0.1.5**: Build + Test + Package + Publish to NuGet + GitHub Release

### Troubleshooting

**Build Fails:**
- Check the build logs in GitHub Actions
- Ensure all dependencies are correctly specified
- Verify .NET 8.0 compatibility

**Tests Fail:**
- Review test output in the test reporter
- Run tests locally: `dotnet test ./src/Selkie.AutoMocking.sln`

**NuGet Publish Fails:**
- Verify NUGET_API_KEY secret is set correctly
- Ensure the version doesn't already exist on NuGet.org
- Check API key permissions (needs "Push" permission)

**Version Not Updated:**
- Check the workflow logs for "Update version in project file" step
- Verify the .csproj file has a `<Version>` element
