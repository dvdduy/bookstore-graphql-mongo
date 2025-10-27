# 🏗️ Architecture Documentation

Comprehensive architecture documentation for the BookStore GraphQL MongoDB application using C4 Model diagrams.

---

## 📋 Table of Contents

- [Overview](#overview)
- [C4 Model Diagrams](#c4-model-diagrams)
  - [Level 1: System Context](#level-1-system-context)
  - [Level 2: Container Diagram](#level-2-container-diagram)
  - [Level 3: Component Diagram (API)](#level-3-component-diagram-api)
  - [Level 3: Component Diagram (Frontend)](#level-3-component-diagram-frontend)
- [Data Flow](#data-flow)
- [Deployment Architecture](#deployment-architecture)
- [Testing Architecture](#testing-architecture)
- [Technology Stack](#technology-stack)

---

## 🎯 Overview

This is a **full-stack learning project** demonstrating modern web application architecture with:

- **Frontend:** Angular 18 SPA with Apollo GraphQL Client
- **Backend:** .NET 9 GraphQL API with HotChocolate
- **Database:** MongoDB with proper indexing and patterns
- **DevOps:** Docker containerization and GitHub Actions CI/CD

**Architecture Style:** Clean Architecture with clear separation of concerns

---

## 🏛️ C4 Model Diagrams

The [C4 Model](https://c4model.com/) provides a hierarchical way to visualize software architecture at different levels of abstraction.

### Level 1: System Context

**What:** Shows how the BookStore system fits into the world around it.

```mermaid
C4Context
    title System Context Diagram - BookStore Learning Platform

    Person(user, "Developer/Learner", "Someone learning full-stack development with GraphQL")
    
    System(bookstore, "BookStore System", "Allows users to browse books, view details, and learn GraphQL patterns")
    
    System_Ext(github, "GitHub", "Source control, CI/CD, and container registry")
    System_Ext(docker, "Docker Hub", "Base images for containers")
    
    Rel(user, bookstore, "Browses books, views details", "HTTPS")
    Rel(bookstore, github, "Pulls code, pushes images", "HTTPS")
    Rel(bookstore, docker, "Pulls base images", "HTTPS")
    
    UpdateLayoutConfig($c4ShapeInRow="3", $c4BoundaryInRow="1")
```

**Key Points:**
- Single user type: Developers/Learners
- Self-contained system (no external APIs)
- Integrates with GitHub for CI/CD
- Uses Docker for containerization

---

### Level 2: Container Diagram

**What:** Shows the high-level technology choices and how containers communicate.

```mermaid
C4Container
    title Container Diagram - BookStore System

    Person(user, "Developer/Learner", "Browses books via web interface")

    Container_Boundary(bookstore, "BookStore System") {
        Container(spa, "Single Page Application", "Angular 18, TypeScript", "Provides book browsing UI in the user's browser")
        Container(api, "GraphQL API", ".NET 9, HotChocolate", "Provides GraphQL queries and mutations for book data")
        ContainerDb(db, "Database", "MongoDB", "Stores book data, authors, reviews")
    }

    System_Ext(browser, "Web Browser", "Chrome, Firefox, Edge")
    System_Ext(ghcr, "GitHub Container Registry", "Docker image storage")

    Rel(user, spa, "Uses", "HTTPS")
    Rel(spa, api, "Makes API calls to", "GraphQL over HTTPS, Port 5000")
    Rel(api, db, "Reads from and writes to", "MongoDB Protocol, Port 27017")
    Rel(spa, browser, "Runs in")
    Rel(ghcr, spa, "Pulls Docker image")
    Rel(ghcr, api, "Pulls Docker image")

    UpdateLayoutConfig($c4ShapeInRow="2", $c4BoundaryInRow="1")
```

**Key Points:**
- **SPA (Angular):** Client-side application, served via Nginx in Docker
- **API (.NET 9):** Stateless GraphQL API server
- **Database (MongoDB):** NoSQL document database
- **Communication:** GraphQL over HTTP (mutations and queries)

---

### Level 3: Component Diagram (API)

**What:** Shows the internal structure of the GraphQL API container.

```mermaid
C4Component
    title Component Diagram - GraphQL API Container

    Container(spa, "Single Page Application", "Angular 18", "User interface")
    ContainerDb(db, "Database", "MongoDB", "Book data storage")

    Container_Boundary(api, "GraphQL API") {
        Component(graphql, "GraphQL Server", "HotChocolate", "Handles GraphQL queries and mutations")
        Component(query, "Query Resolvers", "C#", "GetBooks, GetBook, GetPagedBooks")
        Component(mutation, "Mutation Resolvers", "C#", "AddBook, UpdateBook, DeleteBook")
        Component(types, "GraphQL Types", "C#", "Book, Author, Review types")
        
        Component(repo, "Book Repository", "C#", "Data access layer")
        Component(context, "Book Context", "C#", "MongoDB connection and collections")
        
        Component(middleware, "Middleware", "C#", "Exception handling, logging, CORS")
        Component(health, "Health Checks", "C#", "API and MongoDB health endpoints")
        Component(logger, "Serilog Logger", "Serilog", "Structured logging")
    }

    Rel(spa, graphql, "Sends GraphQL queries/mutations", "HTTP POST")
    Rel(graphql, query, "Routes queries to")
    Rel(graphql, mutation, "Routes mutations to")
    Rel(query, repo, "Uses")
    Rel(mutation, repo, "Uses")
    Rel(repo, context, "Uses")
    Rel(context, db, "Reads/writes data", "MongoDB Driver")
    Rel(graphql, types, "Uses for schema")
    Rel(middleware, graphql, "Wraps")
    Rel(middleware, logger, "Logs to")
    Rel(health, context, "Checks")

    UpdateLayoutConfig($c4ShapeInRow="3", $c4BoundaryInRow="1")
```

**Key Components:**

| Component | Responsibility | Technology |
|-----------|---------------|------------|
| **GraphQL Server** | Schema definition, request routing | HotChocolate 14 |
| **Query Resolvers** | Read operations (GetBooks, GetBook, etc.) | C# methods |
| **Mutation Resolvers** | Write operations (Add, Update, Delete) | C# methods |
| **Book Repository** | Data access abstraction | Repository pattern |
| **Book Context** | MongoDB client singleton | MongoDB.Driver |
| **Middleware** | Cross-cutting concerns | ASP.NET Core |
| **Health Checks** | Monitoring and diagnostics | Microsoft.Extensions |

**Patterns Used:**
- ✅ Repository Pattern (data access)
- ✅ Singleton Pattern (MongoDB client)
- ✅ Dependency Injection (all services)
- ✅ Middleware Pipeline (request processing)

---

### Level 3: Component Diagram (Frontend)

**What:** Shows the internal structure of the Angular SPA container.

```mermaid
C4Component
    title Component Diagram - Angular SPA Container

    Container(api, "GraphQL API", ".NET 9", "Provides book data")
    Person(user, "Developer/Learner", "Browses books")

    Container_Boundary(spa, "Single Page Application") {
        Component(root, "App Component", "Angular", "Root component, manages book list and selection")
        Component(list, "Book List Component", "Angular", "Displays list of books")
        Component(item, "Book Item Component", "Angular", "Individual book in list")
        Component(detail, "Book Detail Component", "Angular", "Shows detailed book information")
        
        Component(service, "Book Service", "TypeScript", "GraphQL queries via Apollo Client")
        Component(apollo, "Apollo Client", "Apollo Angular", "GraphQL client library")
        
        Component(pipes, "Pipes", "TypeScript", "Data transformation (AuthorNamesPipe)")
        Component(models, "Models", "TypeScript", "BookItem, BookDetail, Author, Review")
    }

    Rel(user, root, "Interacts with")
    Rel(root, list, "Contains")
    Rel(root, detail, "Contains")
    Rel(list, item, "Contains multiple")
    Rel(root, service, "Uses")
    Rel(item, service, "Uses")
    Rel(service, apollo, "Uses")
    Rel(apollo, api, "Sends GraphQL queries", "HTTP POST")
    Rel(detail, pipes, "Uses")
    Rel(service, models, "Returns typed data")
    Rel(root, models, "Uses")

    UpdateLayoutConfig($c4ShapeInRow="3", $c4BoundaryInRow="1")
```

**Key Components:**

| Component | Responsibility | Lines of Code |
|-----------|---------------|---------------|
| **App Component** | Root, manages state, handles errors | ~100 |
| **Book List Component** | Displays book list, emits selections | ~30 |
| **Book Item Component** | Individual book display, click handling | ~40 |
| **Book Detail Component** | Detailed view, reviews, authors | ~50 |
| **Book Service** | GraphQL queries (secure with variables) | ~60 |
| **Apollo Client** | GraphQL HTTP client, caching | Library |
| **AuthorNamesPipe** | Transforms author array to string | ~10 |

**Patterns Used:**
- ✅ Smart/Dumb Components (Container/Presentation)
- ✅ Reactive Programming (RxJS Observables)
- ✅ Async Pipe (memory leak prevention)
- ✅ Service Injection (dependency injection)

---

## 🔄 Data Flow

### Query Flow (Read Operation)

```mermaid
sequenceDiagram
    actor User
    participant Angular as Angular SPA
    participant Apollo as Apollo Client
    participant API as GraphQL API
    participant Resolver as Query Resolver
    participant Repo as Book Repository
    participant Mongo as MongoDB

    User->>Angular: Clicks on book
    Angular->>Apollo: getBookDetail$(id)
    Apollo->>API: POST /graphql<br/>{query: getBook(id: $id)}
    API->>Resolver: Route to GetBookAsync
    Resolver->>Repo: GetByIdAsync(id)
    Repo->>Mongo: db.books.findOne({_id})
    Mongo-->>Repo: Book document
    Repo-->>Resolver: Book entity
    Resolver-->>API: Book GraphQL type
    API-->>Apollo: JSON response
    Apollo-->>Angular: Observable<BookDetail>
    Angular->>User: Display book details
```

### Mutation Flow (Write Operation)

```mermaid
sequenceDiagram
    actor User
    participant Angular as Angular SPA
    participant Apollo as Apollo Client
    participant API as GraphQL API
    participant Mutation as Mutation Resolver
    participant Repo as Book Repository
    participant Mongo as MongoDB

    User->>Angular: Submits new book form
    Angular->>Apollo: addBook({title, author, ...})
    Apollo->>API: POST /graphql<br/>{mutation: addBook(input)}
    API->>Mutation: Route to AddBookAsync
    Mutation->>Mutation: Validate input
    Mutation->>Repo: CreateAsync(book)
    Repo->>Mongo: db.books.insertOne({...})
    Mongo-->>Repo: Inserted ID
    Repo-->>Mutation: Created book
    Mutation-->>API: Book GraphQL type
    API-->>Apollo: JSON response
    Apollo-->>Angular: Observable<Book>
    Angular->>User: Show success message
```

---

## 🚀 Deployment Architecture

```mermaid
C4Deployment
    title Deployment Diagram - Docker Compose (Local Development)

    Deployment_Node(dev, "Developer Machine", "Windows/Mac/Linux") {
        Deployment_Node(docker, "Docker Engine", "Docker 24+") {
            Container(nginx, "Nginx", "Web Server", "Serves Angular SPA on port 4200")
            Container(api, "API", ".NET 9", "GraphQL API on port 5000")
            Container(mongo, "MongoDB", "NoSQL DB", "Database on port 27017")
        }
        
        Deployment_Node(browser, "Web Browser", "Chrome/Firefox") {
            Container(spa, "Angular App", "JavaScript/TypeScript", "Runs in browser")
        }
    }

    Rel(spa, nginx, "Loads from", "HTTP :4200")
    Rel(spa, api, "GraphQL requests", "HTTP :5000")
    Rel(api, mongo, "Data access", "MongoDB :27017")
```

**Deployment Configurations:**

| Environment | Compose File | Purpose |
|-------------|--------------|---------|
| **Development (No Auth)** | `docker-compose.dev.yml` | MongoDB without authentication |
| **Development (With Auth)** | `docker-compose.dev-auth.yml` | MongoDB with user/password |
| **Production** | `docker-compose.yml` | Full stack with all services |

**Port Mapping:**
- **Frontend:** `http://localhost:4200` (Nginx serving Angular)
- **API:** `http://localhost:5000` (GraphQL endpoint)
- **MongoDB:** `localhost:27017` (Database connection)

---

## 🧪 Testing Architecture

```mermaid
graph TB
    subgraph "Frontend Tests (79 tests)"
        FT1[Component Tests<br/>Jasmine + Karma]
        FT2[Service Tests<br/>Apollo Mock]
        FT3[Pipe Tests<br/>Unit Tests]
    end

    subgraph "Backend Integration Tests (13 tests)"
        IT1[GraphQL Query Tests<br/>WebApplicationFactory]
        IT2[GraphQL Mutation Tests<br/>Test Server]
        IT3[Validation Tests<br/>Error Cases]
    end

    subgraph "Backend Unit Tests (11 tests)"
        UT1[Repository Tests<br/>Moq + FluentAssertions]
        UT2[Entity Tests<br/>Business Logic]
    end

    subgraph "CI/CD Pipeline"
        CI[GitHub Actions<br/>Automated Testing]
    end

    FT1 --> CI
    FT2 --> CI
    FT3 --> CI
    IT1 --> CI
    IT2 --> CI
    IT3 --> CI
    UT1 --> CI
    UT2 --> CI

    style FT1 fill:#e1f5ff
    style FT2 fill:#e1f5ff
    style FT3 fill:#e1f5ff
    style IT1 fill:#fff4e1
    style IT2 fill:#fff4e1
    style IT3 fill:#fff4e1
    style UT1 fill:#e8f5e9
    style UT2 fill:#e8f5e9
    style CI fill:#f3e5f5
```

**Test Coverage:**
- 🎨 **Frontend:** 79 tests (Components, Services, Pipes)
- 🔗 **Integration:** 13 tests (Full API endpoints with MongoDB)
- 🧪 **Unit:** 11 tests (Repository and entity logic)
- 📊 **Total:** **103 tests** running in CI/CD

---

## 🛠️ Technology Stack

### Frontend Stack

```mermaid
graph LR
    A[Angular 18] --> B[TypeScript 5.5]
    A --> C[Apollo Angular 7]
    C --> D[GraphQL 16]
    A --> E[RxJS 7]
    A --> F[Bootstrap 5]
    
    style A fill:#dd0031
    style B fill:#3178c6
    style C fill:#311c87
    style D fill:#e10098
    style E fill:#d81b60
    style F fill:#7952b3
```

**Key Libraries:**
- **Angular 18** - Modern web framework
- **Apollo Angular 7** - GraphQL client
- **RxJS 7** - Reactive programming
- **Bootstrap 5** - UI framework
- **TypeScript 5.5** - Type safety

### Backend Stack

```mermaid
graph LR
    A[.NET 9] --> B[HotChocolate 14]
    A --> C[MongoDB.Driver 3]
    A --> D[Serilog 4]
    B --> E[GraphQL.NET]
    
    style A fill:#512bd4
    style B fill:#f25cc1
    style C fill:#13aa52
    style D fill:#1e88e5
    style E fill:#e10098
```

**Key Libraries:**
- **.NET 9** - Modern web platform
- **HotChocolate 14** - GraphQL server
- **MongoDB.Driver 3** - Database driver
- **Serilog 4** - Structured logging
- **xUnit** - Testing framework

### DevOps Stack

```mermaid
graph TB
    A[Docker] --> B[Docker Compose]
    C[GitHub Actions] --> D[CI Pipeline]
    C --> E[CD Pipeline]
    D --> F[Automated Tests]
    E --> G[GHCR Images]
    
    style A fill:#2496ed
    style C fill:#2088ff
    style F fill:#28a745
    style G fill:#6e5494
```

**Key Tools:**
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **GitHub Actions** - CI/CD automation
- **GHCR** - Container image registry
- **Dependabot** - Dependency updates

---

## 📊 Architecture Decisions

### Why GraphQL?

✅ **Strong typing** - Schema-first development  
✅ **Single endpoint** - Simplified API surface  
✅ **Flexible queries** - Clients request exactly what they need  
✅ **Self-documenting** - Built-in introspection and IDE support  
✅ **Learning value** - Modern API pattern  

### Why MongoDB?

✅ **Document model** - Natural fit for nested data (authors, reviews)  
✅ **Flexible schema** - Easy to evolve during learning  
✅ **No ORM needed** - Direct document access  
✅ **Good for demos** - Quick setup, no migrations  

### Why Clean Architecture?

✅ **Separation of concerns** - Easy to understand and test  
✅ **Testability** - Dependency injection throughout  
✅ **Maintainability** - Clear boundaries between layers  
✅ **Learning value** - Professional patterns  

---

## 🎓 Learning Objectives

This architecture demonstrates:

1. **Full-Stack Development**
   - Frontend SPA with modern framework
   - Backend API with GraphQL
   - Database integration

2. **Modern Patterns**
   - Repository pattern
   - Dependency injection
   - Reactive programming
   - Smart/Dumb components

3. **DevOps Practices**
   - Containerization with Docker
   - CI/CD with GitHub Actions
   - Automated testing
   - Infrastructure as Code

4. **Best Practices**
   - Clean Architecture
   - Type safety (TypeScript + C#)
   - Error handling
   - Structured logging
   - Health checks
   - Security (input validation, variable usage)

---

## 📚 Related Documentation

- **[API Documentation](API.md)** - GraphQL schema and queries
- **[Testing Guide](TESTING.md)** - 103 comprehensive tests
- **[Docker Setup](DOCKER.md)** - Containerization guide
- **[CI/CD Pipeline](CICD.md)** - Automation setup
- **[Setup Guide](SETUP.md)** - Getting started

---

## 🤝 Contributing

Want to improve the architecture?

1. **Performance:** Add caching, pagination improvements
2. **Security:** JWT auth, role-based access
3. **Features:** Filtering, sorting, search
4. **Monitoring:** Application Insights, metrics
5. **Testing:** E2E tests with Playwright

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

---

**Built for learning, designed for clarity.** 🎓✨

