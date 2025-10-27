# 🤝 Contributing to BookStore

Thank you for considering contributing to the BookStore project! This guide will help you get started.

---

## 📋 Quick Links

- [Getting Started](#-getting-started)
- [Development Workflow](#-development-workflow)
- [Code Standards](#-code-standards)
- [Testing Requirements](#-testing-requirements)
- [Pull Request Process](#-pull-request-process)

---

## 🚀 Getting Started

### **1. Fork & Clone**
```bash
# Fork on GitHub, then:
git clone https://github.com/YOUR_USERNAME/bookstore-graphql-mongo.git
cd bookstore-graphql-mongo
```

### **2. Set Up Development Environment**
```bash
# Start MongoDB
docker compose -f docker-compose.dev.yml up -d

# Install dependencies
cd BookStore/BookStore.API
dotnet restore

cd ../client
npm install
```

### **3. Create a Branch**
```bash
git checkout -b feature/your-feature-name
# or
git checkout -b fix/issue-number-description
```

---

## 💻 Development Workflow

### **Daily Development**
```bash
# 1. Start MongoDB (if not running)
docker compose -f docker-compose.dev.yml up -d

# 2. Start backend with hot reload
cd BookStore/BookStore.API
dotnet watch run

# 3. Start frontend (new terminal)
cd BookStore/client
npm start
```

### **Before Committing**
```bash
# 1. Run all tests
cd BookStore
dotnet test

# 2. Check linter (frontend)
cd client
npm run lint

# 3. Build to verify
dotnet build -c Release
npm run build
```

---

## 📝 Code Standards

### **C# / .NET**

**Naming:**
- `PascalCase` for classes, methods, properties
- `camelCase` for local variables, parameters
- `_camelCase` for private fields

**Style:**
- Use `async/await` for asynchronous operations
- Prefer LINQ for collections
- Use `var` when type is obvious
- Keep methods small and focused

**Example:**
```csharp
public async Task<Book> GetByIdAsync(string id)
{
    if (string.IsNullOrWhiteSpace(id))
    {
        throw new ArgumentException("ID is required", nameof(id));
    }
    
    return await _collection
        .Find(x => x.Id == id)
        .FirstOrDefaultAsync();
}
```

### **TypeScript / Angular**

**Naming:**
- `PascalCase` for classes, interfaces, types
- `camelCase` for variables, functions, properties
- `UPPER_SNAKE_CASE` for constants

**Style:**
- Use `const` by default, `let` when needed
- Avoid `any`, use proper types
- Use async pipe in templates
- Unsubscribe from observables (or use async pipe)

**Example:**
```typescript
export class BookService {
  constructor(private apollo: Apollo) {}
  
  getBooks$(): Observable<Book[]> {
    return this.apollo
      .watchQuery<{ books: Book[] }>({
        query: GET_BOOKS_QUERY
      })
      .valueChanges
      .pipe(map(result => result.data.books));
  }
}
```

### **Testing**

**Test Naming:** `MethodName__Scenario__ExpectedResult`

**Example:**
```csharp
[Fact]
public async Task GetByIdAsync__ValidId__ReturnsBook()
{
    // Arrange
    var expectedBook = CreateTestBook();
    
    // Act
    var result = await _repository.GetByIdAsync(expectedBook.Id);
    
    // Assert
    result.Should().NotBeNull();
    result.Id.Should().Be(expectedBook.Id);
}
```

---

## 🧪 Testing Requirements

### **All Changes Must Include Tests**

**Backend:**
- Unit tests for new methods
- Integration tests for API endpoints
- Maintain 70%+ code coverage

**Frontend:**
- Component tests (when implemented)
- Service tests (when implemented)

### **Running Tests**
```bash
# All tests
cd BookStore
dotnet test

# Specific test
dotnet test --filter "TestName"

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

### **Test Checklist**
- [ ] Happy path tested
- [ ] Error cases tested
- [ ] Edge cases covered
- [ ] Validation tested
- [ ] All tests pass
- [ ] No warnings

---

## 🔀 Pull Request Process

### **1. Before Submitting**

- [ ] Code follows project standards
- [ ] All tests pass
- [ ] New tests added
- [ ] Documentation updated
- [ ] No linter warnings
- [ ] Commit messages are clear

### **2. Commit Message Format**

```
type(scope): subject

body

footer
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation only
- `style`: Code style (formatting, etc.)
- `refactor`: Code change that neither fixes a bug nor adds a feature
- `test`: Adding or updating tests
- `chore`: Build process or auxiliary tool changes

**Examples:**
```
feat(api): add pagination to books query

Implements pagination with page and pageSize parameters.
Returns PagedBooksResult with metadata.

Closes #123
```

```
fix(auth): resolve login redirect issue

Users were not redirected after successful login.
Added proper navigation handling.
```

### **3. Pull Request Template**

```markdown
## Description
Brief description of what this PR does

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Tests added/updated
- [ ] Documentation updated
- [ ] No new warnings
```

### **4. Review Process**

1. Submit PR with clear description
2. Wait for automated checks to pass
3. Address review comments
4. Squash commits if requested
5. Get approval
6. Maintainer will merge

---

## 🐛 Reporting Bugs

### **Before Reporting**
1. Check existing issues
2. Try latest version
3. Collect error logs

### **Bug Report Template**

```markdown
**Describe the Bug**
Clear and concise description

**To Reproduce**
Steps to reproduce:
1. Go to '...'
2. Click on '...'
3. See error

**Expected Behavior**
What you expected to happen

**Actual Behavior**
What actually happened

**Environment**
- OS: [e.g. Windows 11]
- .NET Version: [e.g. 9.0]
- Node Version: [e.g. 18.17.0]
- Browser: [e.g. Chrome 120]

**Logs**
```
Paste error logs here
```
```

---

## 💡 Feature Requests

### **Feature Request Template**

```markdown
**Is your feature request related to a problem?**
Clear description of the problem

**Describe the solution you'd like**
Clear description of desired behavior

**Describe alternatives you've considered**
Alternative solutions or features

**Additional context**
Screenshots, mockups, examples
```

---

## 📚 Development Resources

### **Documentation**
- [docs/SETUP.md](SETUP.md) - Setup guide
- [docs/DOCKER.md](DOCKER.md) - Docker guide
- [docs/TESTING.md](TESTING.md) - Testing guide
- [docs/API.md](API.md) - API reference

### **External Resources**
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [HotChocolate Docs](https://chillicream.com/docs/hotchocolate)
- [Angular Guide](https://angular.io/docs)
- [MongoDB Manual](https://docs.mongodb.com/manual/)

---

## 🎯 Areas Needing Help

Want to contribute but not sure where? Check these:

### **High Priority**
- [ ] Angular unit tests implementation
- [ ] E2E tests with Playwright/Cypress
- [ ] JWT authentication
- [ ] CI/CD pipeline setup

### **Medium Priority**
- [ ] MongoDB schema validation
- [ ] Filtering and sorting on queries
- [ ] Performance optimizations
- [ ] Accessibility improvements

### **Good First Issues**
- [ ] Documentation improvements
- [ ] Code cleanup
- [ ] Adding more test cases
- [ ] UI enhancements

---

## 🏆 Recognition

Contributors will be:
- Listed in project README
- Credited in release notes
- Thanked in commits

---

## 📞 Getting Help

- **Questions:** Open a GitHub issue
- **Discussions:** GitHub Discussions (when available)
- **Bug Reports:** GitHub Issues
- **Feature Requests:** GitHub Issues

---

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

**Thank you for contributing!** 🎉

