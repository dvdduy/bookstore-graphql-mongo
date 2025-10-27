# CI/CD Pipeline Documentation

This document describes the **minimal** CI/CD pipeline for the BookStore GraphQL MongoDB **learning project**.

> **Philosophy:** Intentionally simple and fast for rapid learning. Focus on tests, not bells and whistles.

---

## 🎯 Overview

This **learning-focused** CI/CD pipeline uses **GitHub Actions only** - no external cloud providers required!

**What you get:**
- ✅ Automated testing (backend + frontend) on every push
- ✅ Docker image building (manual trigger only)
- ✅ Fast feedback (~3-4 minutes)
- ✅ Simple and easy to understand

**What you DON'T need:**
- ❌ Security scanning (Dependabot handles this)
- ❌ Code quality analysis (CodeQL, SonarCloud)
- ❌ Test coverage tracking (Codecov)
- ❌ PR validation automation
- ❌ Scheduled maintenance tasks

> **Perfect for learning!** Everything runs on GitHub's free tier. Add security/quality tools later when you're ready for production.

---

## 📋 Table of Contents

- [Workflows](#workflows)
- [Setup Instructions](#setup-instructions)
- [Usage](#usage)
- [Badge Configuration](#badge-configuration)
- [Upgrading to Production](#upgrading-to-production)

---

## 🔄 Workflows

### **1. CI Pipeline (`ci.yml`)**

**Triggers:**
- Every push to `main`
- Every pull request to `main`

**What it does:**
```
1. Backend Tests
   ├── Restore .NET dependencies
   ├── Build solution
   ├── Run unit tests (11 tests)
   └── Run integration tests (13 tests)

2. Frontend Tests
   ├── Install npm dependencies
   ├── Run unit tests (79 tests)
   └── Build production bundle
```

**Duration:** ~3-4 minutes

**File:** `.github/workflows/ci.yml` (75 lines)

---

### **2. CD Pipeline (`cd.yml`)**

**Triggers:**
- ⚙️ Manual trigger (Actions → Run workflow)
- 🏷️ Git tags (e.g., `v1.0.0`)

**What it does:**
```
1. Build Docker Images
   ├── Build API image
   ├── Build Frontend image
   ├── Push to GitHub Container Registry
   └── Tag with version or 'latest'
```

**Duration:** ~4-5 minutes

**File:** `.github/workflows/cd.yml` (85 lines)

**Why manual?** For a learning project, you don't need Docker images built on every commit. Trigger it when you want to demonstrate containerization!

---

## 🚀 Setup Instructions

### **Prerequisites**

1. **GitHub repository** (public or private)
2. **No secrets required!** Everything uses `GITHUB_TOKEN` (automatic)

### **Enable GitHub Actions**

1. Go to your repository on GitHub
2. Click **Actions** tab
3. If disabled, click **"I understand my workflows, go ahead and enable them"**

That's it! ✅

### **Enable GitHub Container Registry (GHCR)**

For Docker image publishing (CD pipeline):

1. Go to **Settings** → **Actions** → **General**
2. Scroll to **Workflow permissions**
3. Select **"Read and write permissions"**
4. ✅ Check **"Allow GitHub Actions to create and approve pull requests"**
5. Click **Save**

---

## 💻 Usage

### **Run CI Automatically**

CI runs automatically on every push:

```bash
git add .
git commit -m "feat: add new feature"
git push origin main
```

→ GitHub Actions automatically runs tests!

---

### **Manually Trigger Docker Build**

**Option 1: Via GitHub UI**
1. Go to **Actions** tab
2. Click **"CD Pipeline - Docker Build"** in the left sidebar
3. Click **"Run workflow"** button
4. Choose tag (default: `latest`)
5. Click **"Run workflow"**

**Option 2: Via Git Tag**
```bash
# Create and push a version tag
git tag v1.0.0
git push origin v1.0.0
```

→ Docker images automatically built and pushed!

---

### **Pull Docker Images**

After CD runs, pull your images:

```bash
# Pull API image
docker pull ghcr.io/dvdduy/bookstore-graphql-mongo/api:latest

# Pull Frontend image
docker pull ghcr.io/dvdduy/bookstore-graphql-mongo/client:latest

# Run them
docker run -p 5000:5000 ghcr.io/dvdduy/bookstore-graphql-mongo/api:latest
docker run -p 4200:80 ghcr.io/dvdduy/bookstore-graphql-mongo/client:latest
```

---

## 🎨 Badge Configuration

Add CI badges to your README:

```markdown
[![CI Pipeline](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/ci.yml/badge.svg)](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/ci.yml)
```

**Available badges:**
- CI Pipeline: `workflows/ci.yml/badge.svg`
- CD Pipeline: `workflows/cd.yml/badge.svg`

---

## 📊 What's Different from "Enterprise" CI/CD?

This is **intentionally minimal** for learning. Here's what's missing (and why):

| Feature | Included? | Why Not? |
|---------|-----------|----------|
| **Automated Testing** | ✅ Yes | Core requirement! |
| **Docker Build** | ✅ Manual only | Don't need it every commit |
| **Security Scanning** | ❌ No | Dependabot handles dependencies |
| **Code Quality (CodeQL)** | ❌ No | Overkill for learning |
| **Test Coverage Tracking** | ❌ No | You can see it locally |
| **PR Validation** | ❌ No | GitHub shows test status |
| **Scheduled Scans** | ❌ No | Not deploying to production |
| **Multi-platform Docker** | ❌ No | Just linux/amd64 is enough |
| **SBOM Generation** | ❌ No | Not required for learning |

**Result:** Fast, simple, focused on the stack you're learning!

---

## 🔧 Troubleshooting

### **CI Tests Fail**

**Check MongoDB:**
The CI automatically starts MongoDB. If integration tests fail:
1. Check logs: Actions → CI Pipeline → backend-tests
2. Look for MongoDB connection errors
3. Verify connection string in test output

**Check npm:**
If frontend tests fail with `npm ci` error:
1. Ensure `package-lock.json` is committed
2. Run `npm install` locally
3. Commit updated `package-lock.json`

---

### **Docker Push Fails**

**Error: "denied: permission_denied"**

**Solution:**
1. Go to Settings → Actions → General
2. Under "Workflow permissions"
3. Select "Read and write permissions"
4. Save

---

### **Workflows Don't Trigger**

1. Check Actions tab is enabled
2. Verify `.github/workflows/*.yml` files exist
3. Push a commit to `main` branch
4. Check Actions tab for runs

---

## 📈 Upgrading to Production

When you're ready to deploy for real, consider adding:

### **Security** (Recommended)
```yaml
# Add to ci.yml
- name: Run Trivy Security Scan
  uses: aquasecurity/trivy-action@master
  with:
    scan-type: 'fs'
    scan-ref: '.'
```

### **Code Quality** (Optional)
```yaml
# Add CodeQL
- name: Initialize CodeQL
  uses: github/codeql-action/init@v3
  with:
    languages: 'csharp,javascript'
```

### **Test Coverage** (Optional)
```yaml
# Add Codecov
- name: Upload Coverage
  uses: codecov/codecov-action@v4
  with:
    token: ${{ secrets.CODECOV_TOKEN }}
```

### **Automatic Docker Builds** (If deploying)
```yaml
# Change cd.yml trigger
on:
  push:
    branches: [ main ]  # Build on every push
```

---

## 📚 File Structure

```
.github/workflows/
├── ci.yml          # 75 lines  - Automated tests
└── cd.yml          # 85 lines  - Manual Docker builds

Total: 160 lines (vs 956 lines in "enterprise" setup)
```

---

## 🎓 Learning Resources

**Want to understand what these do?**

1. **GitHub Actions Basics**
   - [Official Docs](https://docs.github.com/en/actions)
   - [Workflow Syntax](https://docs.github.com/en/actions/reference/workflow-syntax-for-github-actions)

2. **Docker & GHCR**
   - [Working with Container Registry](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry)
   - [Docker Build Push Action](https://github.com/docker/build-push-action)

3. **Testing in CI**
   - [Testing .NET in GitHub Actions](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net)
   - [Testing Node.js in GitHub Actions](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-nodejs)

---

## 🤝 Related Documentation

- **[Testing Guide](TESTING.md)** - Run tests locally (103 tests)
- **[Docker Setup](DOCKER.md)** - Local Docker development
- **[API Documentation](API.md)** - GraphQL API reference

---

## 💡 Why This Approach?

**From 956 lines to 160 lines (-83%)**

This project is for **learning GraphQL, MongoDB, and Angular** - not for learning DevOps (yet!).

**Benefits:**
- ✅ Fast CI feedback (3-4 min vs 8-10 min)
- ✅ Easy to understand (no complex workflows)
- ✅ Lower GitHub Actions minutes usage
- ✅ Focus on the stack, not the pipeline
- ✅ Still demonstrates CI/CD concepts

**When to upgrade:**
- 🚀 When deploying to production
- 🔒 When security becomes a priority
- 📊 When you need compliance/auditing
- 👥 When working in a team

---

**Happy Learning!** 🎉

For questions or issues, check the [main README](../README.md) or open an [issue](https://github.com/dvdduy/bookstore-graphql-mongo/issues).
