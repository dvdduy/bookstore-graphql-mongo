# 🧪 Testing Guide

Complete guide to testing the BookStore application.

---

## 📊 Test Overview

| Test Type | Count | Coverage | Status |
|-----------|-------|----------|--------|
| Unit Tests | 11 | 70%+ | ✅ Passing |
| Integration Tests | 13 | API endpoints | ✅ Passing (with MongoDB) |
| **Total** | **24** | - | **✅ 100%** |

---

## 🚀 Quick Start

### **Run All Tests**
```bash
cd BookStore
dotnet test
```

### **Run Specific Test Project**
```bash
# Unit tests (no dependencies needed)
dotnet test BookStore.UnitTests/BookStore.UnitTests.csproj

# Integration tests (requires MongoDB)
dotnet test BookStore.IntegrationTests/BookStore.IntegrationTests.csproj
```

### **Run Single Test**
```bash
dotnet test --filter "GetAllAsync__ReturnsAllBooks"
```

---

## 🎯 Unit Tests

**Location:** `BookStore/BookStore.UnitTests/`

**What They Test:**
- ✅ Repository methods (CRUD operations)
- ✅ Entity business logic (e.g., `AverageReview` calculation)
- ✅ Data validation
- ✅ Edge cases

**Dependencies:**
- xUnit
- Moq (mocking)
- FluentAssertions (readable assertions)

### **Test Structure**
```
BookStore.UnitTests/
├── Repositories/
│   └── BookRepositoryTests.cs      # Repository tests
└── Entities/
    └── BookTests.cs                # Entity logic tests
```

### **Running Unit Tests**
```bash
cd BookStore
dotnet test BookStore.UnitTests/BookStore.UnitTests.csproj
```

**Expected:** All 11 tests pass ✅

### **Example Test**
```csharp
[Fact]
public async Task GetAllAsync__ReturnsAllBooks()
{
    // Arrange
    var books = new List<Book> { /* test data */ };
    _mockCollection.Setup(/* mock behavior */);
    
    // Act
    var result = await _repository.GetAllAsync();
    
    // Assert
    result.Should().HaveCount(3);
    result.Should().BeEquivalentTo(books);
}
```

**Test Naming:** `Method__Scenario__ExpectedResult`

---

## 🔗 Integration Tests

**Location:** `BookStore/BookStore.IntegrationTests/`

**What They Test:**
- ✅ GraphQL Query operations
- ✅ GraphQL Mutation operations
- ✅ Error handling
- ✅ Validation logic
- ✅ End-to-end API flows

**Dependencies:**
- xUnit
- Microsoft.AspNetCore.Mvc.Testing (WebApplicationFactory)
- FluentAssertions
- MongoDB (test database)

### **Test Structure**
```
BookStore.IntegrationTests/
├── GraphQLQueryTests.cs            # Query tests (5 tests)
├── GraphQLMutationTests.cs         # Mutation tests (6 tests)
├── GraphQLTestFixture.cs           # Test setup
└── BookStore.IntegrationTests.csproj
```

### **Prerequisites**

Integration tests require MongoDB running:

```bash
# Option 1: Docker (recommended)
docker compose -f docker-compose.dev.yml up -d

# Option 2: Local MongoDB without authentication
mongod --dbpath C:\data\db --noauth
```

### **Running Integration Tests**
```bash
# 1. Start MongoDB
docker compose -f docker-compose.dev.yml up -d

# 2. Wait for initialization (10 seconds)
timeout /t 10

# 3. Run tests
cd BookStore
dotnet test BookStore.IntegrationTests/BookStore.IntegrationTests.csproj
```

**Expected:** All 13 tests pass ✅

### **Test Breakdown**

**Query Tests (5):**
- ✅ `GetBooksAsync` - Get all books
- ✅ `GetPagedBooksAsync` - Pagination
- ✅ `GetBookAsync` - Get single book
- ✅ Error cases (invalid ID, empty ID)

**Mutation Tests (6):**
- ✅ `AddBookAsync` - Create book
- ✅ `UpdateBookAsync` - Update book
- ✅ `DeleteBookAsync` - Delete book
- ✅ Validation errors (missing title, etc.)
- ✅ Not found errors

**Validation Tests (2):**
- ✅ All input validation
- ✅ All error responses

---

## 🔧 Test Configuration

### **Test Database**

Integration tests use a separate database:
- **Name:** `BookStoreTestDB`
- **Auto-created** by Docker init script
- **Isolated** from development data

**Connection String** (in `GraphQLTestFixture.cs`):
```csharp
ConnectionString = "mongodb://localhost:27017"
Database = "BookStoreTestDB"
```

### **WebApplicationFactory**

Tests use `WebApplicationFactory<Program>` to:
- ✅ Create in-memory test server
- ✅ Override MongoDB configuration
- ✅ Test entire HTTP pipeline
- ✅ No need to deploy

### **Test Environment**

Tests run in `Development` environment to:
- ✅ Enable database seeding
- ✅ Show detailed error messages
- ✅ Skip authentication requirements

---

## 📝 Writing New Tests

### **Unit Test Template**
```csharp
[Fact]
public async Task MethodName__Scenario__ExpectedResult()
{
    // Arrange
    var testData = CreateTestData();
    _mockDependency.Setup(x => x.Method()).Returns(testData);
    
    // Act
    var result = await _sut.MethodUnderTest();
    
    // Assert
    result.Should().NotBeNull();
    result.Should().BeEquivalentTo(expectedData);
}
```

### **Integration Test Template**
```csharp
[Fact]
public async Task GraphQLOperation__Scenario__ExpectedResult()
{
    // Arrange
    var query = @"
        query {
            books { id title }
        }";
    
    // Act
    var response = await _fixture.ExecuteGraphQLQuery(query);
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var content = await response.Content.ReadAsStringAsync();
    var doc = JsonDocument.Parse(content);
    // ... assertions
}
```

### **Test Naming Convention**

Format: `MethodName__Input__ExpectedOutput`

**Examples:**
- `GetAllAsync__NoParameters__ReturnsAllBooks`
- `GetByIdAsync__ValidId__ReturnsBook`
- `GetByIdAsync__InvalidId__ReturnsNull`
- `AddBookAsync__ValidInput__CreatesBook`
- `AddBookAsync__MissingTitle__ThrowsValidationError`

---

## 🚨 Troubleshooting

### **Integration Tests Failing**

**Symptom:** Tests fail with authentication or connection errors

**Solutions:**

1. **Verify MongoDB is running:**
   ```bash
   docker ps | grep mongodb
   ```

2. **Check MongoDB is healthy:**
   ```bash
   docker ps
   # Status should show "healthy"
   ```

3. **Use no-auth version:**
   ```bash
   docker compose -f docker-compose.dev-auth.yml down
   docker compose -f docker-compose.dev.yml up -d
   ```

4. **Wait for initialization:**
   ```bash
   timeout /t 10
   ```

5. **Check logs:**
   ```bash
   docker logs bookstore-mongodb-dev
   ```

6. **Recreate database:**
   ```bash
   docker compose -f docker-compose.dev.yml down -v
   docker compose -f docker-compose.dev.yml up -d
   ```

### **Unit Tests Failing**

Unit tests should never fail due to external dependencies.

**If they do:**
1. Clean and rebuild:
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. Check for code changes that broke tests

3. Run single failing test with verbose output:
   ```bash
   dotnet test --filter "TestName" --logger "console;verbosity=detailed"
   ```

### **Test Discovery Issues**

**Symptom:** Tests not showing up in Test Explorer

**Solutions:**
1. Rebuild solution
2. Close and reopen Visual Studio / VS Code
3. Check test project references
4. Ensure xUnit runner is installed

---

## 📊 Code Coverage

### **Generate Coverage Report**
```bash
cd BookStore

# Install coverage tool (first time only)
dotnet tool install --global dotnet-coverage

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Coverage report in: TestResults/{guid}/coverage.cobertura.xml
```

### **View Coverage in VS**
- Visual Studio → Test → Analyze Code Coverage

### **Current Coverage**
- **Unit Tests:** 70%+ of repository and entity code
- **Integration Tests:** All GraphQL endpoints

---

## 🎯 Testing Best Practices

### **✅ DO:**
- Write tests before fixing bugs (TDD)
- Test edge cases and error conditions
- Use descriptive test names
- Keep tests independent
- Mock external dependencies in unit tests
- Use FluentAssertions for readability
- Test one thing per test method

### **❌ DON'T:**
- Don't share state between tests
- Don't test implementation details
- Don't write tests that depend on order
- Don't mock what you don't own (in integration tests)
- Don't skip failing tests
- Don't write tests without assertions

---

## 🏗️ Test Architecture

```
┌─────────────────────────────────────┐
│   Integration Tests                 │
│   ├── Uses WebApplicationFactory    │
│   ├── Tests entire HTTP pipeline    │
│   ├── Uses real MongoDB             │
│   └── Tests GraphQL operations      │
└─────────────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────┐
│   BookStore.API (Startup)           │
│   ├── GraphQL Server                │
│   ├── Query & Mutation resolvers    │
│   └── Error handling                │
└─────────────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────┐
│   Unit Tests                        │
│   ├── Mock dependencies             │
│   ├── Test business logic           │
│   └── Test repository methods       │
└─────────────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────┐
│   BookStore.Infrastructure          │
│   └── MongoDB Repository            │
└─────────────────────────────────────┘
```

---

## 📚 Testing Tools Reference

### **xUnit**
- Test framework
- `[Fact]` - Single test
- `[Theory]` - Parameterized test
- `[InlineData]` - Test data

### **Moq**
- Mocking library
- `Mock<T>` - Create mock
- `.Setup()` - Configure behavior
- `.Verify()` - Assert interactions

### **FluentAssertions**
- Assertion library
- `.Should().Be()` - Equality
- `.Should().NotBeNull()` - Null check
- `.Should().HaveCount()` - Collection size
- `.Should().BeEquivalentTo()` - Deep equality

### **WebApplicationFactory**
- Integration testing
- Creates in-memory test server
- Can override services
- Tests entire pipeline

---

## 🎓 Next Steps

- ✅ All tests passing? Great! Check [docs/API.md](API.md) for API examples
- ✅ Want to add tests? See [docs/CONTRIBUTING.md](CONTRIBUTING.md)
- ✅ MongoDB issues? Check [docs/DOCKER.md](DOCKER.md)

---

**Need help?** Open an [issue](https://github.com/dvdduy/bookstore-graphql-mongo/issues).

