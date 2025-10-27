# 📖 GraphQL API Reference

Complete reference for the BookStore GraphQL API.

---

## 🚀 Quick Access

**GraphQL Playground:** http://localhost:5000/graphql  
**Health Check:** http://localhost:5000/health  
**Ready Check:** http://localhost:5000/health/ready

---

## 📋 API Overview

The BookStore API provides:
- ✅ **Queries** - Retrieve book data
- ✅ **Mutations** - Create, update, delete books
- ✅ **Pagination** - Efficient data fetching
- ✅ **Validation** - Input validation with detailed errors
- ✅ **Error Handling** - Graceful error responses

---

## 🔍 Queries

### **1. Get All Books**

```graphql
query {
  books {
    id
    title
    description
    imageUrl
    publisher
    publishedDate
    length
    authors {
      id
      name
    }
    reviews {
      id
      rating
      title
      description
    }
    averageReview
  }
}
```

**Response:**
```json
{
  "data": {
    "books": [
      {
        "id": "60d5ec49f...",
        "title": "Clean Code",
        "description": "A Handbook of Agile Software Craftsmanship",
        "publisher": "Prentice Hall",
        "authors": [
          { "id": "1", "name": "Robert C. Martin" }
        ],
        "averageReview": 4.5
      }
    ]
  }
}
```

---

### **2. Get Paginated Books**

```graphql
query GetPagedBooks($page: Int!, $pageSize: Int!) {
  pagedBooks(page: $page, pageSize: $pageSize) {
    books {
      id
      title
      publisher
      averageReview
    }
    totalCount
    page
    pageSize
    totalPages
    hasNextPage
    hasPreviousPage
  }
}
```

**Variables:**
```json
{
  "page": 1,
  "pageSize": 10
}
```

**Response:**
```json
{
  "data": {
    "pagedBooks": {
      "books": [ /* ... */ ],
      "totalCount": 25,
      "page": 1,
      "pageSize": 10,
      "totalPages": 3,
      "hasNextPage": true,
      "hasPreviousPage": false
    }
  }
}
```

**Validation:**
- `page` must be >= 1
- `pageSize` must be between 1 and 100

---

### **3. Get Single Book**

```graphql
query GetBook($id: String!) {
  book(id: $id) {
    id
    title
    description
    imageUrl
    publisher
    publishedDate
    length
    authors {
      id
      name
    }
    reviews {
      id
      rating
      title
      description
    }
    averageReview
  }
}
```

**Variables:**
```json
{
  "id": "60d5ec49f1c4a123456789ab"
}
```

**Errors:**
- Invalid ID format: `"Invalid book ID format: 'abc'. Expected a valid MongoDB ObjectId"`
- Book not found: `"Book with ID '...' was not found"`
- Empty ID: `"Book ID is required and cannot be empty"`

---

## ✏️ Mutations

### **1. Add Book**

```graphql
mutation AddBook($input: AddBookInput!) {
  addBook(input: $input) {
    id
    title
    description
    publisher
    publishedDate
    length
    authors {
      id
      name
    }
  }
}
```

**Variables:**
```json
{
  "input": {
    "title": "Domain-Driven Design",
    "description": "Tackling Complexity in the Heart of Software",
    "imageUrl": "https://example.com/ddd.jpg",
    "publisher": "Addison-Wesley",
    "publishedDate": "2003-08-01T00:00:00Z",
    "length": 560,
    "authors": [
      { "name": "Eric Evans" }
    ]
  }
}
```

**Validation:**
- ✅ `title` - Required, non-empty
- ✅ `description` - Required, non-empty
- ✅ `publisher` - Required, non-empty
- ✅ `publishedDate` - Required, valid ISO 8601 date
- ✅ `length` - Required, 1-5000 pages
- ✅ `authors` - Required, at least one author

**Response:**
```json
{
  "data": {
    "addBook": {
      "id": "60d5ec49f...",
      "title": "Domain-Driven Design",
      // ... other fields
    }
  }
}
```

---

### **2. Update Book**

```graphql
mutation UpdateBook($input: UpdateBookInput!) {
  updateBook(input: $input) {
    id
    title
    description
    publisher
    updatedAt
  }
}
```

**Variables:**
```json
{
  "input": {
    "id": "60d5ec49f1c4a123456789ab",
    "title": "Clean Code (Updated)",
    "description": "Updated description",
    "imageUrl": "https://example.com/new-image.jpg",
    "publisher": "Prentice Hall",
    "publishedDate": "2008-08-11T00:00:00Z",
    "length": 464,
    "authors": [
      { "name": "Robert C. Martin" },
      { "name": "Co-Author" }
    ]
  }
}
```

**Validation:**
- Same as AddBook, plus:
- ✅ `id` - Required, valid MongoDB ObjectId
- ✅ Book must exist

**Errors:**
- Invalid ID: `"Invalid book ID format"`
- Not found: `"Book with ID '...' not found"`

---

### **3. Delete Book**

```graphql
mutation DeleteBook($id: String!) {
  deleteBook(id: $id)
}
```

**Variables:**
```json
{
  "id": "60d5ec49f1c4a123456789ab"
}
```

**Response:**
```json
{
  "data": {
    "deleteBook": true
  }
}
```

**Errors:**
- Invalid ID: `"Invalid book ID format"`
- Not found: `"Book with ID '...' not found"`

---

## 🧬 Types

### **Book**
```graphql
type Book {
  id: String!
  title: String!
  description: String!
  imageUrl: String
  publisher: String!
  publishedDate: DateTime!
  length: Int!
  authors: [Author!]!
  reviews: [Review!]!
  averageReview: Float
  createdAt: DateTime!
  updatedAt: DateTime!
}
```

### **Author**
```graphql
type Author {
  id: String!
  name: String!
}
```

### **Review**
```graphql
type Review {
  id: String!
  rating: Int!
  title: String!
  description: String!
}
```

### **PagedBooksResult**
```graphql
type PagedBooksResult {
  books: [Book!]!
  totalCount: Long!
  page: Int!
  pageSize: Int!
  totalPages: Int!
  hasNextPage: Boolean!
  hasPreviousPage: Boolean!
}
```

---

## 📝 Input Types

### **AddBookInput**
```graphql
input AddBookInput {
  title: String!              # Required
  description: String!        # Required
  imageUrl: String           # Optional
  publisher: String!          # Required
  publishedDate: DateTime!    # Required, ISO 8601 format
  length: Int!                # Required, 1-5000
  authors: [AuthorInput!]!    # Required, at least 1
}
```

### **UpdateBookInput**
```graphql
input UpdateBookInput {
  id: String!                 # Required, must be valid ObjectId
  title: String!              # Required
  description: String!        # Required
  imageUrl: String           # Optional
  publisher: String!          # Required
  publishedDate: DateTime!    # Required
  length: Int!                # Required, 1-5000
  authors: [AuthorInput!]!    # Required, at least 1
}
```

### **AuthorInput**
```graphql
input AuthorInput {
  name: String!               # Required
}
```

---

## ⚠️ Error Handling

### **Error Response Format**
```json
{
  "errors": [
    {
      "message": "Error message here",
      "locations": [{ "line": 2, "column": 3 }],
      "path": ["books"],
      "extensions": {
        "message": "Detailed error message",
        "stackTrace": "..." // Only in Development
      }
    }
  ],
  "data": null
}
```

### **Common Errors**

**Validation Errors:**
```json
{
  "errors": [
    {
      "message": "Title is required.",
      "path": ["addBook"]
    }
  ]
}
```

**Not Found Errors:**
```json
{
  "errors": [
    {
      "message": "Book with ID '...' was not found",
      "path": ["book"]
    }
  ]
}
```

**Invalid ID Errors:**
```json
{
  "errors": [
    {
      "message": "Invalid book ID format: 'abc'. Expected a valid MongoDB ObjectId",
      "path": ["book"]
    }
  ]
}
```

---

## 🧪 Testing with Banana Cake Pop

### **Access the IDE**
1. Start the API: `dotnet run`
2. Open: http://localhost:5000/graphql
3. Banana Cake Pop loads automatically

### **Sample Operations**

**1. Get all books:**
```graphql
{
  books {
    id
    title
    authors { name }
  }
}
```

**2. Add a book:**
```graphql
mutation {
  addBook(input: {
    title: "Test Book"
    description: "Test Description"
    publisher: "Test Publisher"
    publishedDate: "2024-01-01T00:00:00Z"
    length: 300
    authors: [{ name: "Test Author" }]
  }) {
    id
    title
  }
}
```

**3. Query with variables:**
- Click "Variables" panel
- Add JSON variables
- Run query

---

## 🔒 Future: Authentication

**Coming Soon:**
- JWT authentication
- Role-based authorization
- Mutation access control

**Example with Auth:**
```graphql
mutation {
  addBook(input: { ... }) {
    id
  }
}

# Headers:
# Authorization: Bearer <jwt-token>
```

---

## 📊 Performance Tips

1. **Request only needed fields:**
   ```graphql
   { books { id title } }  # ✅ Good
   { books { ... all fields } }  # ❌ Avoid if not needed
   ```

2. **Use pagination for large datasets:**
   ```graphql
   { pagedBooks(page: 1, pageSize: 20) { ... } }
   ```

3. **Leverage GraphQL batching** (handled automatically by Apollo)

4. **Monitor with health checks:**
   ```bash
   curl http://localhost:5000/health
   ```

---

## 📚 Additional Resources

- [GraphQL Official Docs](https://graphql.org/)
- [HotChocolate Docs](https://chillicream.com/docs/hotchocolate)
- [Banana Cake Pop Guide](https://chillicream.com/docs/bananacakepop)

---

**Need help?** Check [docs/SETUP.md](SETUP.md) or open an [issue](https://github.com/dvdduy/bookstore-graphql-mongo/issues).

