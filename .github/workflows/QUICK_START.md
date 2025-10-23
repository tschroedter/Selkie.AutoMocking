# Quick Start Guide

This guide will help you get started with the CI/CD workflow for Selkie.AutoMocking.

## Overview

The workflow automatically:
- ✅ Builds your project
- ✅ Runs all tests
- ✅ Versions your package (v0.1.x format)
- ✅ Creates NuGet packages
- ✅ Publishes to NuGet.org (on releases)
- ✅ Creates GitHub releases

## Basic Usage

### Automatic Versioning (Recommended)

The workflow automatically increments the build number:
- **Version format**: `v0.1.x` where `x` is the GitHub Actions run number
- **Example**: Build #23 → Version `0.1.23`

Just push to `main` or `master` and the workflow runs automatically!

```bash
git commit -m "Your changes"
git push origin main
```

### Manual Release (For specific versions)

To create a specific version (e.g., `v0.1.5`):

```bash
# Option 1: Use the helper script (easiest)
.github/workflows/create-release.sh 0.1.5

# Option 2: Manual tag creation
git tag v0.1.5
git push origin v0.1.5
```

This will:
1. Trigger the workflow
2. Build and test
3. Create version `0.1.5`
4. Publish to NuGet.org
5. Create GitHub Release

## Initial Setup

### 1. Enable GitHub Actions
GitHub Actions should be enabled by default. If not:
1. Go to your repository settings
2. Click "Actions" → "General"
3. Enable "Allow all actions and reusable workflows"

### 2. Configure NuGet Publishing (Optional)

To enable automatic publishing to NuGet.org:

1. **Get your NuGet API Key**:
   - Go to https://www.nuget.org/account/apikeys
   - Click "Create" 
   - Name: `GitHub Actions`
   - Expiration: Choose appropriate duration
   - Scopes: Select "Push"
   - Glob Pattern: `Selkie.AutoMocking`
   - Click "Create"
   - **Copy the key** (you won't see it again!)

2. **Add the key to GitHub**:
   - Go to your repository on GitHub
   - Click "Settings" → "Secrets and variables" → "Actions"
   - Click "New repository secret"
   - Name: `NUGET_API_KEY`
   - Value: Paste your API key
   - Click "Add secret"

3. **That's it!** The next tagged release will publish to NuGet.org automatically.

### 3. Verify Everything Works

Create a test push to verify the workflow:

```bash
# Make a small change
echo "# Test" >> README.md
git add README.md
git commit -m "Test CI/CD workflow"
git push origin main
```

Then check:
1. Go to "Actions" tab in your repository
2. You should see a workflow run in progress
3. Wait for it to complete (usually 2-3 minutes)
4. All steps should be green ✅

## Common Workflows

### Push to Main
```bash
git commit -m "Fix bug"
git push origin main
```
**Result**: Builds, tests, creates package artifact

### Create PR
```bash
git checkout -b feature/my-feature
git commit -m "Add feature"
git push origin feature/my-feature
# Create PR on GitHub
```
**Result**: Builds and tests only (no deployment)

### Create Release
```bash
.github/workflows/create-release.sh 0.1.10
```
**Result**: Builds, tests, publishes to NuGet.org, creates GitHub Release

## Checking Results

### View Workflow Status
1. Go to "Actions" tab
2. Click on the workflow run
3. View logs for each step

### Download Artifacts
1. Go to "Actions" tab
2. Click on a completed workflow run
3. Scroll to "Artifacts" section
4. Download "nuget-package"

### View Test Results
1. Go to "Actions" tab
2. Click on a workflow run
3. View "Test Results" in the summary

## Troubleshooting

### Workflow doesn't start
- Check if GitHub Actions is enabled in repository settings
- Verify the workflow file is in `.github/workflows/` directory

### Tests fail
- Run tests locally: `dotnet test ./src/Selkie.AutoMocking.sln`
- Check test logs in the workflow output

### NuGet publish fails
- Verify `NUGET_API_KEY` secret is set
- Check if the version already exists on NuGet.org
- Ensure API key has "Push" permission

### Version is wrong
- Check if you're using the correct branch (`main` or `master`)
- For manual versions, ensure tag format is correct: `vX.Y.Z`

## Tips

💡 **Use semantic versioning**: When you're ready for v1.0.0, create a tag `v1.0.0`

💡 **Test before releasing**: Always test changes in a PR before merging to main

💡 **Review before publishing**: Check the package artifact before creating a release tag

💡 **Skip duplicate publishes**: The workflow automatically skips if version exists on NuGet.org

## Next Steps

- Read the full [README.md](README.md) for detailed information
- Review the [workflow file](build-test-deploy.yml) to understand the steps
- Create your first release with the helper script!

## Need Help?

- Check the [README.md](README.md) for detailed documentation
- Review workflow logs in the "Actions" tab
- Check [GitHub Actions documentation](https://docs.github.com/en/actions)
