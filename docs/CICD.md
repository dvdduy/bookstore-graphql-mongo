# CI/CD Pipeline Documentation

This document describes the Continuous Integration and Continuous Deployment (CI/CD) pipelines implemented for the BookStore GraphQL MongoDB **learning project**.

## 📋 Table of Contents

- [Overview](#overview)
- [Workflows](#workflows)
- [Setup Instructions](#setup-instructions)
- [Secrets Configuration](#secrets-configuration)
- [Badge Configuration](#badge-configuration)
- [Troubleshooting](#troubleshooting)

---

## 🎯 Overview

This **learning-focused** CI/CD pipeline uses **GitHub Actions only** - no external cloud providers required!

**What you get:**
- ✅ Automated testing (backend + frontend)
- ✅ Code quality analysis with CodeQL
- ✅ Security vulnerability scanning
- ✅ Docker image building and publishing to GHCR (free)
- ✅ Automated dependency updates with Dependabot
- ✅ Pull request validation
- ✅ Scheduled maintenance tasks

**What you DON'T need:**
- ❌ Azure subscription
- ❌ AWS account
- ❌ Any cloud provider
- ❌ Credit card or payment info

> **Perfect for learning!** Everything runs on GitHub's free tier for public repositories.

---

## 🔄 Workflows

### 1. CI Pipeline (`ci.yml`)

**Triggers:** Push to `main`/`develop`, Pull Requests

**Jobs:**
- **backend-tests**: Runs .NET unit and integration tests with MongoDB
- **frontend-tests**: Runs Angular unit tests with coverage
- **code-quality**: CodeQL security analysis and dependency review
- **docker-build**: Validates Docker image builds
- **build-summary**: Generates comprehensive build report

**Status Badge:**
```markdown
![CI Pipeline](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/ci.yml/badge.svg)
```

### 2. CD Pipeline (`cd.yml`)

**Triggers:** Push to `main`, Git tags (`v*`), Manual dispatch

**Jobs:**
- **build-and-push**: Builds and pushes Docker images to GitHub Container Registry
- **create-release**: Creates GitHub releases for tagged versions
- **deployment-summary**: Generates deployment report

> **Note:** Azure deployment is commented out by default. No cloud provider needed!

**Features:**
- Multi-platform Docker images (linux/amd64, linux/arm64)
- Images published to GitHub Container Registry (free for public repos)
- SBOM (Software Bill of Materials) generation
- Automated changelog generation
- Semantic versioning support
- Works without any external cloud providers

**Status Badge:**
```markdown
![CD Pipeline](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/cd.yml/badge.svg)
```

### 3. PR Validation (`pr-validation.yml`)

**Triggers:** Pull request events

**Jobs:**
- **pr-check**: Validates PR title follows conventional commits
- **code-review**: Automated code review with Reviewdog
- **conflict-check**: Detects merge conflicts
- **coverage-check**: Verifies test coverage with Codecov
- **security-scan**: Trivy vulnerability scanning
- **performance-check**: Basic performance benchmarks
- **pr-summary**: Posts validation summary as PR comment

**Features:**
- Semantic PR title validation (feat, fix, docs, etc.)
- PR size labeling (XS, S, M, L, XL)
- Automated code review comments
- Coverage reports on Codecov
- Security vulnerability reports

### 4. Scheduled Maintenance (`scheduled.yml`)

**Triggers:** Daily at 2 AM UTC, Manual dispatch

**Jobs:**
- **dependency-scan**: Checks for vulnerable dependencies
- **outdated-check**: Lists outdated packages
- **docker-security-scan**: Scans Docker images with Trivy
- **code-metrics**: Calculates code statistics
- **health-check**: Monitors external services
- **cleanup-artifacts**: Removes artifacts older than 30 days
- **maintenance-summary**: Generates maintenance report

**Features:**
- Automatic issue creation for vulnerabilities
- Dependency audit reports
- Code metrics tracking
- Artifact cleanup to save storage

### 5. Dependabot (`dependabot.yml`)

**Automatic Dependency Updates:**
- .NET NuGet packages (Monday 9 AM)
- npm packages (Monday 9 AM)
- Docker base images (Tuesday 9 AM)
- GitHub Actions (Wednesday 9 AM)

**Configuration:**
- Weekly update schedule
- Max 10 open PRs for packages, 5 for Docker/Actions
- Ignores major version updates by default
- Auto-labels PRs by ecosystem

---

## 🚀 Setup Instructions

### 1. Fork/Clone the Repository

```bash
git clone https://github.com/dvdduy/bookstore-graphql-mongo.git
cd bookstore-graphql-mongo
```

### 2. Enable GitHub Actions

GitHub Actions are enabled by default. Workflows will trigger automatically on push/PR.

### 3. Configure Branch Protection (Optional but Recommended)

Go to **Settings > Branches > Add rule** for `main`:

- ✅ Require pull request reviews
- ✅ Require status checks to pass:
  - `backend-tests`
  - `frontend-tests`
  - `code-quality`
  - `docker-build`
- ✅ Require branches to be up to date
- ✅ Require linear history
- ✅ Include administrators

### 4. Enable GitHub Container Registry

GHCR (GitHub Container Registry) is used for Docker images and requires no special setup. Images are automatically pushed to:

```
ghcr.io/dvdduy/bookstore-graphql-mongo/api
ghcr.io/dvdduy/bookstore-graphql-mongo/client
```

### 5. Set Up Codecov (Optional)

For detailed coverage reports:

1. Sign up at https://codecov.io
2. Link your repository
3. Add `CODECOV_TOKEN` to repository secrets
4. Coverage reports will appear on PRs

---

## 🔐 Secrets Configuration

### Required Secrets

No secrets are required for basic CI/CD functionality.

### Optional Secrets (Not Required for Basic CI/CD)

Add these **ONLY IF** you want the specific features:

| Secret | Description | Required For | Status |
|--------|-------------|--------------|--------|
| `CODECOV_TOKEN` | Codecov integration | Coverage reports | Optional |
| `AZURE_CREDENTIALS` | Azure service principal | Azure deployment | **Disabled by default** |
| `DOCKER_HUB_USERNAME` | Docker Hub username | Docker Hub publishing | Optional |
| `DOCKER_HUB_TOKEN` | Docker Hub token | Docker Hub publishing | Optional |
| `SLACK_WEBHOOK` | Slack webhook URL | Slack notifications | Optional |

> **Note:** Azure deployment is completely commented out in the CD pipeline. The CI/CD will work perfectly without any cloud provider!

### Azure Deployment Setup (Optional - Skip if you don't have Azure)

**You don't need Azure for the CI/CD to work!** It's completely optional.

If you want to enable Azure deployment in the future:

1. **Create Azure Service Principal:**
```bash
az ad sp create-for-rbac \
  --name "bookstore-github-actions" \
  --role contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group} \
  --sdk-auth
```

2. **Add Output as `AZURE_CREDENTIALS` Secret:**
```json
{
  "clientId": "<GUID>",
  "clientSecret": "<STRING>",
  "subscriptionId": "<GUID>",
  "tenantId": "<GUID>",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

3. **Uncomment Azure deployment steps in `cd.yml`**

---

## 📊 Badge Configuration

Add these badges to your `README.md`:

```markdown
[![CI Pipeline](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/ci.yml/badge.svg)](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/ci.yml)
[![CD Pipeline](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/cd.yml/badge.svg)](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/cd.yml)
[![codecov](https://codecov.io/gh/dvdduy/bookstore-graphql-mongo/branch/main/graph/badge.svg)](https://codecov.io/gh/dvdduy/bookstore-graphql-mongo)
[![License](https://img.shields.io/github/license/dvdduy/bookstore-graphql-mongo)](LICENSE)
```

---

## 🐛 Troubleshooting

### CI Pipeline Fails

**Problem:** Backend tests fail with MongoDB connection error

**Solution:**
```yaml
# Ensure MongoDB service is configured in workflow
services:
  mongodb:
    image: mongo:7.0
    ports:
      - 27017:27017
```

**Problem:** Frontend tests fail with memory issues

**Solution:** Increase Node.js memory
```yaml
- name: Run Tests
  run: npm test -- --watch=false --max_old_space_size=4096
```

### Docker Build Fails

**Problem:** Docker build exceeds time limit

**Solution:** Use layer caching
```yaml
- name: Build Docker Image
  uses: docker/build-push-action@v5
  with:
    cache-from: type=gha
    cache-to: type=gha,mode=max
```

### CD Pipeline Issues

**Problem:** Can't push to GitHub Container Registry

**Solution:** Ensure workflow has correct permissions
```yaml
permissions:
  contents: read
  packages: write
```

**Problem:** Release creation fails

**Solution:** Check if tag follows semantic versioning
```bash
git tag v1.0.0
git push origin v1.0.0
```

### Dependabot Issues

**Problem:** Too many open PRs

**Solution:** Adjust `open-pull-requests-limit` in `.github/dependabot.yml`

**Problem:** Breaking updates

**Solution:** Dependabot ignores major versions by default. Review and test before merging.

---

## 📚 Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Docker Build Push Action](https://github.com/docker/build-push-action)
- [Dependabot Configuration](https://docs.github.com/en/code-security/dependabot/dependabot-version-updates/configuration-options-for-the-dependabot.yml-file)
- [Codecov Documentation](https://docs.codecov.com/)
- [Azure Container Apps](https://docs.microsoft.com/en-us/azure/container-apps/)

---

## 🎯 Best Practices

1. **Never commit secrets** - Use GitHub Secrets for sensitive data
2. **Review Dependabot PRs** - Don't auto-merge without testing
3. **Monitor workflow runs** - Check for patterns in failures
4. **Keep workflows DRY** - Reuse actions and share steps
5. **Use matrix strategies** - Test across multiple environments
6. **Set timeouts** - Prevent hung workflows from consuming minutes
7. **Clean up artifacts** - Scheduled cleanup saves storage costs
8. **Use CODEOWNERS** - Require reviews from specific teams
9. **Enable branch protection** - Enforce quality gates
10. **Document custom actions** - Help future contributors

---

## 📝 Workflow Triggers Summary

| Workflow | Push | PR | Schedule | Manual | Tags |
|----------|------|-----|----------|--------|------|
| CI | ✅ | ✅ | ❌ | ❌ | ❌ |
| CD | ✅ | ❌ | ❌ | ✅ | ✅ |
| PR Validation | ❌ | ✅ | ❌ | ❌ | ❌ |
| Scheduled | ❌ | ❌ | ✅ | ✅ | ❌ |

---

**Last Updated:** October 27, 2025
**Maintained By:** @dvdduy

