#!/bin/bash

# Script to create a new release tag for Selkie.AutoMocking
# Usage: ./create-release.sh [version]
# Example: ./create-release.sh 0.1.5

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Get version from argument or prompt
if [ -z "$1" ]; then
    echo -e "${YELLOW}Enter version number (e.g., 0.1.5):${NC}"
    read -r VERSION
else
    VERSION=$1
fi

# Validate version format (0.1.z for v0.1.* pattern)
if ! [[ $VERSION =~ ^0\.1\.[0-9]+$ ]]; then
    echo -e "${RED}Error: Invalid version format. Expected format: 0.1.z (e.g., 0.1.5, 0.1.123)${NC}"
    echo -e "${YELLOW}Note: Only v0.1.* tags trigger NuGet deployment${NC}"
    exit 1
fi

TAG="v${VERSION}"

# Check if tag already exists locally
if git rev-parse "$TAG" >/dev/null 2>&1; then
    echo -e "${RED}Error: Tag $TAG already exists locally${NC}"
    exit 1
fi

# Check if tag exists on remote
if git ls-remote --tags origin | grep -q "refs/tags/$TAG"; then
    echo -e "${RED}Error: Tag $TAG already exists on remote${NC}"
    exit 1
fi

echo -e "${GREEN}Creating release tag: $TAG${NC}"
echo ""
echo "This will:"
echo "  1. Create and push tag $TAG"
echo "  2. Trigger GitHub Actions workflow"
echo "  3. Build and test the project"
echo "  4. Create NuGet package with version $VERSION"
echo "  5. Publish to NuGet.org (if NUGET_API_KEY is configured)"
echo "  6. Create nuget-v$VERSION tag after successful publish"
echo "  7. Create GitHub Release with package"
echo ""
echo -e "${YELLOW}Continue? (y/n)${NC}"
read -r CONFIRM

if [ "$CONFIRM" != "y" ] && [ "$CONFIRM" != "Y" ]; then
    echo "Cancelled"
    exit 0
fi

# Create and push tag
echo -e "${GREEN}Creating tag...${NC}"
git tag -a "$TAG" -m "Release version $VERSION"

echo -e "${GREEN}Pushing tag to origin...${NC}"
git push origin "$TAG"

echo ""
echo -e "${GREEN}✓ Success!${NC}"
echo ""
echo "Tag $TAG has been created and pushed."
echo "View workflow progress at:"
echo "  https://github.com/$(git config --get remote.origin.url | sed 's/.*github.com[:\/]\(.*\)\.git/\1/')/actions"
echo ""
echo "To delete this release tag if needed:"
echo "  git tag -d $TAG"
echo "  git push origin :refs/tags/$TAG"
