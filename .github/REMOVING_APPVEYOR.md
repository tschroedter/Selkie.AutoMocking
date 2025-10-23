# Removing AppVeyor Status Check from Branch Protection

## Background

This repository previously used AppVeyor for continuous integration. The project now uses GitHub Actions for all CI/CD operations (see `.github/workflows/build-test-deploy.yml`). 

AppVeyor should be removed from branch protection rules to avoid blocking pull requests with the outdated `continuous-integration/appveyor/pr` status check.

## Steps to Remove AppVeyor Status Check

Follow these steps to remove the AppVeyor status check from your branch protection rules:

### 1. Navigate to Branch Protection Settings

1. Go to your repository on GitHub: https://github.com/tschroedter/Selkie.AutoMocking
2. Click on **Settings** (gear icon)
3. In the left sidebar, click on **Branches**
4. Find the branch protection rule for your main branch (usually `main` or `master`)
5. Click **Edit** next to the branch protection rule

### 2. Remove AppVeyor Status Check

1. Scroll down to the **Require status checks to pass before merging** section
2. In the list of required status checks, find `continuous-integration/appveyor/pr`
3. Click the **X** or **Remove** button next to `continuous-integration/appveyor/pr`
4. Ensure that the GitHub Actions status checks are still enabled:
   - Look for status checks like `build-and-test` or similar (from `.github/workflows/build-test-deploy.yml`)
   - These should remain checked/enabled

### 3. Save Changes

1. Scroll to the bottom of the page
2. Click **Save changes**

## Verification

After removing the AppVeyor status check:

1. Create a new pull request or view an existing one
2. The PR should no longer show `continuous-integration/appveyor/pr` as a required check
3. The GitHub Actions workflows should still run and be required for merging
4. Pull requests should be mergeable once the GitHub Actions checks pass

## Current CI/CD Setup

The repository now uses GitHub Actions for all CI/CD operations:

- **Workflow File**: `.github/workflows/build-test-deploy.yml`
- **Triggers**: Push to main/master, pull requests, tagged releases
- **Operations**: Build, test, package, and publish to NuGet.org
- **Documentation**: See `.github/workflows/README.md` for details

## Troubleshooting

### AppVeyor Check Still Appears

If the AppVeyor check still appears after following these steps:

1. Verify that you saved the branch protection rule changes
2. Check if there are multiple branch protection rules that might include AppVeyor
3. Close and reopen the pull request to refresh status checks
4. Contact GitHub support if the issue persists

### Accidentally Removed Wrong Status Check

If you accidentally removed a GitHub Actions status check:

1. Go back to branch protection settings
2. Add the correct status check back by searching for its name
3. The status check name can be found in the workflow YAML file under the `jobs:` section (e.g., `build-and-test`)

## Additional Resources

- [GitHub Documentation: Managing Branch Protection Rules](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/managing-a-branch-protection-rule)
- [GitHub Documentation: About Status Checks](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/collaborating-on-repositories-with-code-quality-features/about-status-checks)
