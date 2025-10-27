# BookStore GraphQL MongoDB - Setup Guide

## Project Overview
This is a full-stack bookstore application featuring:
- **Backend**: ASP.NET Core 5 GraphQL API with HotChocolate and MongoDB
- **Frontend**: Angular 13 with Apollo Angular client

## ✅ Initialization Complete

The project has been successfully initialized with all dependencies installed:

### Backend (ASP.NET Core)
- ✅ .NET packages restored
- ✅ Solution built successfully
- ✅ Three projects compiled:
  - `BookStore.Core` - Domain entities
  - `BookStore.Infrastructure` - Data access layer
  - `BookStore.API` - GraphQL API layer

### Frontend (Angular)
- ✅ Node packages installed (910 packages)
- ✅ Angular 13 application built successfully
- ✅ Apollo Angular client configured

## Prerequisites

Before running the application, ensure you have:

1. **.NET 5.0 SDK or later** (currently using .NET 9.0 SDK)
   - Note: .NET 5.0 is out of support. Consider upgrading to a supported version.

2. **Node.js and npm** (tested with packages for Angular 13)

3. **MongoDB** running locally:
   - Default connection: `mongodb://localhost:27017`
   - Database name: `bookstoredb`
   - Install MongoDB Community Server from: https://www.mongodb.com/try/download/community

## Running the Application

### 1. Start MongoDB
Ensure MongoDB is running on `localhost:27017`

```bash
# On Windows (if MongoDB is installed as a service)
net start MongoDB

# Or run mongod directly
mongod --dbpath C:\data\db
```

### 2. Start the Backend API

```bash
cd BookStore
dotnet run --project BookStore.API
```

The API will start on:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000
- GraphQL endpoint: https://localhost:5001/graphql

### 3. Start the Angular Frontend

Open a new terminal:

```bash
cd BookStore/client
npm start
```

The Angular app will start on:
- http://localhost:4200

The app is configured with a proxy to forward API requests to the backend.

## Configuration

### Backend Configuration

MongoDB connection settings are in `BookStore/BookStore.API/appsettings.json`:

```json
{
  "MongoDbConfiguration": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "bookstoredb"
  }
}
```

### Frontend Configuration

Proxy configuration for API calls is in `BookStore/client/proxy.conf.json`.

## Project Structure

```
bookstore-graphql-mongo/
├── BookStore/
│   ├── BookStore.API/          # GraphQL API with HotChocolate
│   ├── BookStore.Core/         # Domain entities (Book, Author, Review)
│   ├── BookStore.Infrastructure/ # MongoDB data access
│   ├── client/                 # Angular 13 application
│   └── BookStore.sln          # .NET solution file
├── README.md
└── SETUP.md                   # This file
```

## Known Issues & Notes

### Security Warnings
- **MongoDB.Driver 2.14.1** has a known high severity vulnerability. Consider upgrading to a newer version.
- **npm audit** reports 53 vulnerabilities in the Angular dependencies. Run `npm audit fix` to address non-breaking issues.

### Deprecated Packages
- `@fortawesome/fontawesome-free-webfonts` is deprecated
- Angular 13 is relatively old; consider upgrading to the latest Angular version

### .NET Version
- Project targets .NET 5.0 which is out of support
- Currently compiles with .NET 9.0 SDK but consider upgrading the target framework

## Development Commands

### Backend
```bash
cd BookStore

# Restore packages
dotnet restore

# Build solution
dotnet build

# Run API
dotnet run --project BookStore.API

# Run tests (if available)
dotnet test
```

### Frontend
```bash
cd BookStore/client

# Install packages
npm install

# Start dev server
npm start

# Build for production
npm run build

# Run tests
npm test

# Check for dependency issues
npm audit
```

## Next Steps (From Original README)

The following features are planned but not yet implemented:

- [ ] GraphQL `mutation` operations (currently only `query` is implemented)
- [ ] Containerize the application with Docker
- [ ] Upgrade to supported .NET version
- [ ] Update Angular to latest version
- [ ] Address security vulnerabilities

## Testing the API

Once the backend is running, you can access the GraphQL playground at:
https://localhost:5001/graphql

Try this sample query:
```graphql
query {
  books {
    id
    title
    authors {
      name
    }
  }
}
```

## Troubleshooting

### MongoDB Connection Issues
If the API can't connect to MongoDB:
1. Verify MongoDB is running: `mongo --version`
2. Check if MongoDB service is running
3. Verify the connection string in `appsettings.json`

### Port Already in Use
If ports 5000/5001 or 4200 are already in use:
- For .NET: Set environment variables `ASPNETCORE_URLS`
- For Angular: Use `ng serve --port <port-number>`

### Build Errors
If you encounter build errors:
1. Clear .NET build artifacts: `dotnet clean`
2. Clear node_modules: `rm -rf node_modules && npm install`
3. Restore packages again

## Additional Resources

- [HotChocolate Documentation](https://chillicream.com/docs/hotchocolate)
- [Apollo Angular Documentation](https://apollo-angular.com/)
- [MongoDB .NET Driver](https://www.mongodb.com/docs/drivers/csharp/)

