# 📚 BookStore GraphQL API

Modern full-stack application with **GraphQL**, **MongoDB**, and **Angular 18**. Built with .NET 9, featuring comprehensive testing and Docker support.

[![CI Pipeline](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/ci.yml/badge.svg)](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/ci.yml)
[![CD Pipeline](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/cd.yml/badge.svg)](https://github.com/dvdduy/bookstore-graphql-mongo/actions/workflows/cd.yml)
[![License](https://img.shields.io/github/license/dvdduy/bookstore-graphql-mongo)](LICENSE)

---

## 🚀 Quick Start

### **Prerequisites**
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (recommended)

### **Start in 3 Commands**

```bash
# 1. Start MongoDB
docker compose -f docker-compose.dev.yml up -d

# 2. Run Backend (new terminal)
cd BookStore/BookStore.API && dotnet run

# 3. Run Frontend (new terminal)  
cd BookStore/client && npm install && npm start
```

**Visit:** http://localhost:4200 🎉

> **New to the project?** See [docs/SETUP.md](docs/SETUP.md) for detailed instructions.

---

## ✨ What This Is

A **learning project** demonstrating modern full-stack development with GraphQL, MongoDB, and Angular:

- ✅ Modern .NET 9 with HotChocolate GraphQL
- ✅ MongoDB with proper patterns (singleton, indexing)
- ✅ Angular 18 with Apollo Client
- ✅ Comprehensive testing (unit + integration)
- ✅ Docker support (dev & production)
- ✅ CI/CD with GitHub Actions (no cloud provider required)
- ✅ Best practices (error handling, validation, logging)

> **Note:** This project is designed for learning and demonstration purposes. The CI/CD pipeline uses only GitHub Actions and GitHub Container Registry - no external cloud providers needed!

---

## 📊 Features

### **Backend**
- **GraphQL API** with queries & mutations
- **MongoDB** with indexes and health checks
- **CRUD Operations** with pagination
- **Error Handling** & validation
- **Structured Logging** with Serilog
- **Health Checks** for monitoring

### **Frontend**
- **Angular 18** with Bootstrap 5
- **Apollo Client** for GraphQL
- **Type-safe** queries
- **Memory leak prevention**
- **Loading & error states**

### **DevOps**
- **GitHub Actions** CI/CD pipeline
- **Automated testing** on push/PR
- **Docker** image building & publishing
- **Dependabot** dependency updates
- **Security scanning** with CodeQL & Trivy

### **Testing**
- **97 total tests** (11 backend unit, 13 integration, 79 frontend)
- **100% passing** backend & frontend
- **FluentAssertions** for .NET, **Jasmine/Karma** for Angular
- **WebApplicationFactory** for integration tests

---

## 📖 Documentation

| Document | What's Inside |
|----------|---------------|
| **[docs/SETUP.md](docs/SETUP.md)** | Complete setup guide (manual & Docker) |
| **[docs/DOCKER.md](docs/DOCKER.md)** | Docker workflows & troubleshooting |
| **[docs/TESTING.md](docs/TESTING.md)** | How to run & write tests |
| **[docs/API.md](docs/API.md)** | GraphQL API reference with examples |
| **[docs/CONTRIBUTING.md](docs/CONTRIBUTING.md)** | Contribution guidelines |

---

## 🧪 Testing

```bash
# All tests (requires MongoDB running)
cd BookStore && dotnet test

# Unit tests only (no dependencies)
dotnet test BookStore.UnitTests/BookStore.UnitTests.csproj

# Integration tests (requires MongoDB)
dotnet test BookStore.IntegrationTests/BookStore.IntegrationTests.csproj
```

**Expected:** All 24 tests pass ✅

> See [docs/TESTING.md](docs/TESTING.md) for detailed testing guide.

---

## 🏗️ Architecture

```
bookstore-graphql-mongo/
├── BookStore/
│   ├── BookStore.API/              # GraphQL API (.NET 9)
│   ├── BookStore.Core/             # Domain entities
│   ├── BookStore.Infrastructure/   # MongoDB repository
│   ├── BookStore.UnitTests/        # Unit tests
│   ├── BookStore.IntegrationTests/ # Integration tests
│   └── client/                     # Angular 18 frontend
├── docs/                           # Documentation
├── docker/                         # Docker init scripts
└── docker-compose*.yml             # Docker configurations
```

---

## 🔧 Tech Stack

**Backend:** .NET 9 • HotChocolate 14 • MongoDB 7 • Serilog  
**Frontend:** Angular 18 • Apollo Client • Bootstrap 5 • RxJS  
**Testing:** xUnit • Moq • FluentAssertions • WebApplicationFactory  
**DevOps:** Docker • Docker Compose • Health Checks

---

## 🐳 Docker

Three configurations for different needs:

```bash
# Development (no auth) - perfect for testing
docker compose -f docker-compose.dev.yml up -d

# Production-like (with auth) - for security testing  
docker compose -f docker-compose.dev-auth.yml up -d

# Full stack (all services) - complete deployment
docker compose up -d
```

> See [docs/DOCKER.md](docs/DOCKER.md) for complete Docker guide.

---

## 📖 GraphQL API

Explore the interactive API at: http://localhost:5000/graphql

**Quick Examples:**

```graphql
# Get all books
{ books { id title authors { name } } }

# Get paginated books
{ pagedBooks(page: 1, pageSize: 10) { 
    books { id title } 
    totalCount 
    hasNextPage 
} }

# Add a book
mutation {
  addBook(input: {
    title: "New Book"
    description: "Great book"
    publisher: "Publisher"
    publishedDate: "2024-01-01"
    length: 300
    authors: [{ name: "Author" }]
  }) { id title }
}
```

> See [docs/API.md](docs/API.md) for complete API documentation.

---

## 🎯 What Makes This Special

This project demonstrates **production-ready** patterns:

✅ **Clean Architecture** - Clear separation of concerns  
✅ **Testing First** - 24 tests, high coverage  
✅ **Docker Ready** - Multiple configurations  
✅ **Error Handling** - Graceful failures everywhere  
✅ **Type Safety** - TypeScript + C# strict mode  
✅ **Modern Stack** - Latest .NET 9 & Angular 18  
✅ **Best Practices** - Singleton patterns, async/await, memory leak prevention  

---

## 🎓 Learning Focus

This project is built for **educational purposes** to demonstrate:

- **Modern full-stack architecture** with real-world patterns (see [Architecture Diagrams](docs/ARCHITECTURE.md))
- **GraphQL API development** with HotChocolate and Apollo
- **MongoDB integration** with proper indexing and health checks
- **Angular best practices** including memory leak prevention
- **Comprehensive testing** strategies (unit + integration)
- **DevOps practices** with Docker and GitHub Actions CI/CD
- **Code quality** with automated testing and security scanning

> **No cloud provider needed!** Everything runs locally or on GitHub's free tier. Perfect for learning without spending money.
> 
> **Want to understand the architecture?** Check out our [C4 diagrams](docs/ARCHITECTURE.md) with visual documentation!

---

## 🤝 Contributing

Contributions welcome! Please see [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md) for guidelines.

**Quick Steps:**
1. Fork the repository
2. Create a feature branch
3. Make your changes with tests
4. Submit a pull request

---

## 📄 License

This project is licensed under the MIT License.

---

## 📞 Get Help

- **Issues:** [GitHub Issues](https://github.com/dvdduy/bookstore-graphql-mongo/issues)
- **Questions:** Check the [docs](docs/) folder first
- **Setup Problems:** See [docs/SETUP.md](docs/SETUP.md)

---

**Happy Coding!** 🚀
