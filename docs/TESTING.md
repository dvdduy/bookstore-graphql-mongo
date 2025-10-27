# 🧪 Testing Guide

Complete guide to testing the BookStore application.

---

## 📊 Test Overview

| Test Type | Count | Coverage | Status |
|-----------|-------|----------|--------|
| **Backend Unit Tests** | 11 | 70%+ | ✅ Passing |
| **Backend Integration Tests** | 13 | API endpoints | ✅ Passing (with MongoDB) |
| **Frontend Unit Tests** | 79 | Components/Services | ✅ Passing |
| **Total** | **103** | - | **✅ 100%** |

---

## 🚀 Quick Start

### **Run All Backend Tests**
```bash
cd BookStore
dotnet test
```

### **Run All Frontend Tests**
```bash
cd BookStore/client
npm test -- --watch=false --browsers=ChromeHeadless
```

### **Run Specific Test Project**
```bash
# Backend unit tests (no dependencies needed)
cd BookStore
dotnet test BookStore.UnitTests/BookStore.UnitTests.csproj

# Backend integration tests (requires MongoDB)
cd BookStore
dotnet test BookStore.IntegrationTests/BookStore.IntegrationTests.csproj

# Frontend tests with coverage
cd BookStore/client
npm test -- --watch=false --code-coverage --browsers=ChromeHeadless
```

### **Run Single Backend Test**
```bash
cd BookStore
dotnet test --filter "GetAllAsync__ReturnsAllBooks"
```

### **Run Tests in Watch Mode (Frontend)**
```bash
cd BookStore/client
npm test
# Tests will re-run automatically on file changes
```

---

## 🎯 Backend Unit Tests

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

## 🔗 Backend Integration Tests

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

## 🎨 Frontend Unit Tests

**Location:** `BookStore/client/src/app/**/*.spec.ts`

**What They Test:**
- ✅ Angular components (inputs, outputs, rendering)
- ✅ Services (HTTP calls, GraphQL queries)
- ✅ Pipes (data transformations)
- ✅ User interactions (clicks, selections)
- ✅ Loading and error states
- ✅ Edge cases (null/undefined handling)

**Dependencies:**
- Jasmine (test framework)
- Karma (test runner)
- Angular Testing Utilities (`TestBed`, `ComponentFixture`)
- Apollo Angular Testing (`ApolloTestingModule`)

### **Test Structure**
```
BookStore/client/src/app/
├── app.component.spec.ts              # Root component (26 tests)
├── book-list/
│   └── book-list.component.spec.ts   # List component (10 tests)
├── book-item/
│   └── book-item.component.spec.ts   # Item component (15 tests)
├── book-detail/
│   └── book-detail.component.spec.ts # Detail component (16 tests)
├── book.service.spec.ts               # GraphQL service (8 tests)
└── pipes/
    └── author-names.pipe.spec.ts     # Pipe (4 tests)
```

### **Running Frontend Tests**

**Single run (CI mode):**
```bash
cd BookStore/client
npm test -- --watch=false --browsers=ChromeHeadless
```

**Watch mode (development):**
```bash
cd BookStore/client
npm test
# Opens Chrome browser, auto-reruns on changes
```

**With coverage report:**
```bash
cd BookStore/client
npm test -- --watch=false --code-coverage --browsers=ChromeHeadless
# Coverage report in: coverage/index.html
```

**Expected:** All 79 tests pass ✅

### **Test Breakdown**

#### **AppComponent Tests (26 tests)**
- ✅ Component creation and initialization
- ✅ Book loading on init (success/error cases)
- ✅ Book selection and detail loading
- ✅ Loading states (isLoading flag)
- ✅ Error handling and display
- ✅ Console error logging

**Example:**
```typescript
it('should load books on init', fakeAsync(() => {
  const mockBooks: BookItem[] = [/* test data */];
  mockBookService.getAllBookItems$.and.returnValue(of(mockBooks));
  
  component.ngOnInit();
  tick();
  
  expect(component.bookList).toEqual(mockBooks);
  expect(component.isLoading).toBe(false);
}));
```

#### **BookService Tests (8 tests)**
- ✅ `getAllBookItems$` returns book list
- ✅ `getAllBookItems$` handles empty list
- ✅ `getBookDetail$` returns book detail
- ✅ `getBookDetail$` uses variables (security)
- ✅ `getBookDetail$` handles book not found
- ✅ GraphQL query structure validation

**Security Test Example:**
```typescript
it('should use variables for getBookDetail$ (security)', fakeAsync(() => {
  const bookId = 'abc123';
  service.getBookDetail$(bookId).subscribe();
  tick();
  
  const op = controller.expectOne('GetBookDetail');
  expect(op.operation.variables['id']).toBe(bookId);
}));
```

#### **BookListComponent Tests (10 tests)**
- ✅ Accepts `books` input property
- ✅ Accepts `selectedBook` input property
- ✅ Emits `selected` event on book selection
- ✅ Handles null/empty book lists
- ✅ Passes correct data to child components

#### **BookItemComponent Tests (15 tests)**
- ✅ Accepts `book` input property
- ✅ Accepts `isActive` input property
- ✅ Displays book information correctly
- ✅ Emits `selected` event on click
- ✅ Handles edge cases (no reviews, single author)
- ✅ CSS class binding (`active` class)

#### **BookDetailComponent Tests (16 tests)**
- ✅ Accepts `book` input property
- ✅ Displays all book properties (title, category, publisher, etc.)
- ✅ Handles multiple/single/no authors
- ✅ Handles multiple/no reviews
- ✅ Calculates average review correctly
- ✅ Displays published date
- ✅ Uses `AuthorNamesPipe` correctly

#### **AuthorNamesPipe Tests (4 tests)**
- ✅ Transforms array of authors to comma-separated string
- ✅ Handles empty arrays
- ✅ Handles null/undefined input
- ✅ Handles special characters

**Example:**
```typescript
it('should transform array of authors to comma-separated string', () => {
  const authors: Author[] = [
    { name: 'John Doe' },
    { name: 'Jane Smith' }
  ];
  
  const result = pipe.transform(authors);
  
  expect(result).toBe('John Doe, Jane Smith');
});
```

### **Test Coverage**

**Current Coverage:**
- **Statements:** High coverage across all components
- **Branches:** All error paths tested
- **Functions:** All public methods tested
- **Lines:** 79 comprehensive tests

**View Coverage Report:**
```bash
cd BookStore/client
npm test -- --watch=false --code-coverage --browsers=ChromeHeadless
# Open: coverage/index.html in browser
```

### **Testing Tools & Patterns**

**Jasmine (Test Framework):**
- `describe()` - Test suite
- `it()` - Individual test
- `beforeEach()` - Setup before each test
- `expect()` - Assertions
- `spyOn()` - Function spies

**Angular Testing Utilities:**
- `TestBed.configureTestingModule()` - Configure test module
- `ComponentFixture<T>` - Component wrapper for testing
- `fixture.debugElement` - Access DOM elements
- `fixture.detectChanges()` - Trigger change detection

**Apollo Testing:**
- `ApolloTestingModule` - Mock Apollo Client
- `ApolloTestingController` - Control GraphQL operations
- `controller.expectOne()` - Assert GraphQL query

**Common Patterns:**
```typescript
// 1. Mock service
const mockService = jasmine.createSpyObj('BookService', [
  'getAllBookItems$',
  'getBookDetail$'
]);

// 2. Configure test module
TestBed.configureTestingModule({
  declarations: [MyComponent],
  imports: [ApolloTestingModule],
  providers: [{ provide: BookService, useValue: mockService }]
});

// 3. Create component
const fixture = TestBed.createComponent(MyComponent);
const component = fixture.componentInstance;

// 4. Mock observable response
mockService.getAllBookItems$.and.returnValue(of(mockData));

// 5. Trigger lifecycle
fixture.detectChanges();

// 6. Assert
expect(component.bookList).toEqual(mockData);
```

### **Troubleshooting Frontend Tests**

**Tests failing with "Error: Reduce of empty array":**
- **Solution:** The `AuthorNamesPipe` now handles empty arrays gracefully
- **Fixed in:** `author-names.pipe.ts`

**Tests failing with "Pipe 'authorNames' could not be found":**
- **Solution:** Add `AuthorNamesPipe` to `declarations` in `TestBed.configureTestingModule`

**Tests failing with TypeScript errors:**
- **Solution:** Ensure mock data matches model definitions (`BookItem`, `BookDetail`)
- **Common issues:** Missing `publishedDate`, `averageReview` type mismatch

**Chrome not launching:**
```bash
# Use headless mode
npm test -- --watch=false --browsers=ChromeHeadless
```

**Tests timing out:**
- Check for missing `fakeAsync()` / `tick()` in async tests
- Ensure all observables are properly subscribed/completed

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

### **Backend Unit Test Template**
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

### **Backend Integration Test Template**
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

### **Frontend Component Test Template**
```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyComponent } from './my.component';

describe('MyComponent', () => {
  let component: MyComponent;
  let fixture: ComponentFixture<MyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MyComponent],
      imports: [/* required modules */],
      providers: [/* mock services */]
    });
    
    fixture = TestBed.createComponent(MyComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display title', () => {
    component.title = 'Test Title';
    fixture.detectChanges();
    
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Test Title');
  });

  it('should emit event on button click', () => {
    spyOn(component.buttonClicked, 'emit');
    
    const button = fixture.nativeElement.querySelector('button');
    button?.click();
    
    expect(component.buttonClicked.emit).toHaveBeenCalled();
  });
});
```

### **Frontend Service Test Template**
```typescript
import { TestBed } from '@angular/core/testing';
import { ApolloTestingModule, ApolloTestingController } from 'apollo-angular/testing';
import { MyService } from './my.service';

describe('MyService', () => {
  let service: MyService;
  let controller: ApolloTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ApolloTestingModule],
      providers: [MyService]
    });
    
    service = TestBed.inject(MyService);
    controller = TestBed.inject(ApolloTestingController);
  });

  afterEach(() => {
    controller.verify(); // Ensure no outstanding requests
  });

  it('should fetch data', fakeAsync(() => {
    const mockData = { /* test data */ };
    
    service.getData$().subscribe(result => {
      expect(result).toEqual(mockData);
    });
    
    const op = controller.expectOne('MyQuery');
    op.flush({ data: { items: mockData } });
    tick();
  }));
});
```

### **Test Naming Convention**

**Backend Format:** `MethodName__Input__ExpectedOutput`

**Backend Examples:**
- `GetAllAsync__NoParameters__ReturnsAllBooks`
- `GetByIdAsync__ValidId__ReturnsBook`
- `GetByIdAsync__InvalidId__ReturnsNull`
- `AddBookAsync__ValidInput__CreatesBook`
- `AddBookAsync__MissingTitle__ThrowsValidationError`

**Frontend Format:** `should [action] [expected result]`

**Frontend Examples:**
- `should create`
- `should load books on init`
- `should display error message when loading fails`
- `should emit selected event when book is clicked`
- `should handle empty book list gracefully`

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
│   Frontend Unit Tests (79 tests)   │
│   ├── Angular components            │
│   ├── Services (GraphQL client)     │
│   ├── Pipes                         │
│   └── User interactions             │
└─────────────────────────────────────┘
                  │
                  ▼ (HTTP/GraphQL)
┌─────────────────────────────────────┐
│   Backend Integration Tests         │
│   (13 tests)                        │
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
│   Backend Unit Tests (11 tests)    │
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

**Test Coverage by Layer:**
- 🎨 **Frontend:** 79 tests (Components, Services, Pipes)
- 🔗 **API Integration:** 13 tests (GraphQL endpoints)
- 🧪 **Backend Unit:** 11 tests (Repository, Entities)
- 📊 **Total:** 103 tests ✅

---

## 📚 Testing Tools Reference

### **Backend Tools**

#### **xUnit**
- Test framework
- `[Fact]` - Single test
- `[Theory]` - Parameterized test
- `[InlineData]` - Test data

#### **Moq**
- Mocking library
- `Mock<T>` - Create mock
- `.Setup()` - Configure behavior
- `.Verify()` - Assert interactions

#### **FluentAssertions**
- Assertion library
- `.Should().Be()` - Equality
- `.Should().NotBeNull()` - Null check
- `.Should().HaveCount()` - Collection size
- `.Should().BeEquivalentTo()` - Deep equality

#### **WebApplicationFactory**
- Integration testing
- Creates in-memory test server
- Can override services
- Tests entire pipeline

### **Frontend Tools**

#### **Jasmine**
- Test framework for Angular
- `describe()` - Test suite
- `it()` - Individual test
- `expect()` - Assertions
- `beforeEach()` / `afterEach()` - Setup/teardown
- `spyOn()` - Mock functions

#### **Karma**
- Test runner
- Launches browsers (Chrome, ChromeHeadless)
- Watches files for changes
- Generates coverage reports
- Integrates with CI/CD

#### **Angular Testing Utilities**
- `TestBed` - Configure testing module
- `ComponentFixture` - Component test wrapper
- `DebugElement` - Access DOM elements
- `fakeAsync()` / `tick()` - Test async code
- `flush()` - Complete all pending async operations

#### **Apollo Angular Testing**
- `ApolloTestingModule` - Mock Apollo Client
- `ApolloTestingController` - Control GraphQL operations
- `expectOne()` - Assert single GraphQL query
- `flush()` - Send mock response

---

## 🎓 Next Steps

- ✅ **All 103 tests passing?** Great! Check [docs/API.md](API.md) for API examples
- ✅ **Want to add tests?** See [docs/CONTRIBUTING.md](CONTRIBUTING.md) for guidelines
- ✅ **MongoDB issues?** Check [docs/DOCKER.md](DOCKER.md) for setup help
- ✅ **Frontend coverage report?** Run `npm test -- --code-coverage` in `BookStore/client`
- ✅ **CI/CD pipeline?** See [docs/CICD.md](CICD.md) for automated testing setup

---

**Need help?** Open an [issue](https://github.com/dvdduy/bookstore-graphql-mongo/issues).

