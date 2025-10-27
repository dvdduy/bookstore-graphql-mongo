# 🔧 Complete Setup Guide

This guide covers everything you need to get the BookStore application running.

---

## 📋 Prerequisites

### **Required**
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [MongoDB](https://www.mongodb.com/try/download/community) OR [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### **Recommended**
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [MongoDB Compass](https://www.mongodb.com/products/compass) (GUI for MongoDB)
- [Postman](https://www.postman.com/) or use Banana Cake Pop (built-in)

---

## 🚀 Option 1: Quick Start with Docker (Recommended)

### **Step 1: Clone & Start MongoDB**
```bash
cd C:\Personal\bookstore-graphql-mongo

# Start MongoDB (no authentication)
docker compose -f docker-compose.dev.yml up -d

# Verify it's running
docker ps
# Should show: bookstore-mongodb-dev (healthy)
```

### **Step 2: Start Backend**
```bash
cd BookStore/BookStore.API
dotnet restore
dotnet run
```

Backend runs at: http://localhost:5000  
GraphQL IDE: http://localhost:5000/graphql

### **Step 3: Start Frontend**
```bash
cd BookStore/client
npm install  # First time only
npm start
```

Frontend runs at: http://localhost:4200

### **Step 4: Verify**
- Open http://localhost:4200
- You should see the BookStore app
- Click on books to see details

---

## 🔨 Option 2: Manual Setup (Without Docker)

### **Step 1: Install MongoDB Locally**

**Windows:**
```powershell
# Download installer from: https://www.mongodb.com/try/download/community

# Install MongoDB as a service during installation

# Verify installation
mongosh --version

# Start MongoDB without authentication
net stop MongoDB
cd "C:\Program Files\MongoDB\Server\7.0\bin"
mongod.exe --dbpath C:\data\db --noauth
```

**Linux/Mac:**
```bash
# Install MongoDB (varies by distro)
# For Ubuntu:
sudo apt-get install mongodb

# Start MongoDB
sudo systemctl start mongod
```

### **Step 2: Configure Connection String**

Edit `BookStore/BookStore.API/appsettings.Development.json`:
```json
{
  "MongoDbConfiguration": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "BookStoreDB"
  }
}
```

### **Step 3: Start Backend**
```bash
cd BookStore/BookStore.API
dotnet restore
dotnet build
dotnet run
```

### **Step 4: Start Frontend**
```bash
cd BookStore/client
npm install
npm start
```

---

## 🧪 Verify Installation

### **1. Check MongoDB**
```bash
# If using Docker:
docker exec -it bookstore-mongodb-dev mongosh

# If using local MongoDB:
mongosh

# In mongosh:
show dbs
use BookStoreDB
show collections
```

### **2. Check Backend**
```bash
# Health check
curl http://localhost:5000/health

# GraphQL endpoint
curl -X POST http://localhost:5000/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"{ books { id title } }"}'
```

### **3. Check Frontend**
- Open http://localhost:4200
- Should see the book list
- Click a book to see details
- Check browser console for errors

---

## 🧪 Run Tests

### **All Tests**
```bash
cd BookStore
dotnet test
```

### **Unit Tests Only**
```bash
dotnet test BookStore.UnitTests/BookStore.UnitTests.csproj
```

### **Integration Tests** (requires MongoDB)
```bash
# Make sure MongoDB is running first!
dotnet test BookStore.IntegrationTests/BookStore.IntegrationTests.csproj
```

Expected: All 24 tests pass ✅

---

## 🔧 IDE Setup

### **Visual Studio 2022**
1. Open `BookStore/BookStore.sln`
2. Set `BookStore.API` as startup project
3. Press F5 to run
4. In another terminal: `cd BookStore/client && npm start`

### **VS Code**
1. Install extensions:
   - C# Dev Kit
   - Angular Language Service
   - Docker (optional)
2. Open workspace: `bookstore-graphql-mongo`
3. Use tasks.json for easy startup (create if needed)

---

## 🐳 Docker Setup Details

### **Development (No Auth)**
```bash
docker compose -f docker-compose.dev.yml up -d
```
- MongoDB without authentication
- Perfect for testing
- Databases auto-created: `BookStoreDB` and `BookStoreTestDB`

### **Production-like (With Auth)**
```bash
docker compose -f docker-compose.dev-auth.yml up -d
```
- MongoDB with authentication
- Credentials: `admin/admin123` or `bookstore_app/bookstore_dev_pass`
- Update connection string: `mongodb://bookstore_app:bookstore_dev_pass@localhost:27017`

### **Full Stack**
```bash
docker compose up -d
```
- All services containerized (MongoDB + API + Frontend)
- Visit: http://localhost

---

## 🚨 Troubleshooting

### **Port Already in Use**

**Backend (5000):**
```bash
# Find what's using it
netstat -ano | findstr :5000

# Kill the process (Windows)
taskkill /PID <PID> /F
```

**Frontend (4200):**
```bash
# Kill existing Angular dev server
taskkill /IM node.exe /F

# Or change port in angular.json
```

**MongoDB (27017):**
```bash
# Stop MongoDB service
net stop MongoDB

# Or change Docker port in docker-compose.dev.yml
ports: - "27018:27017"
```

### **MongoDB Connection Failed**

```bash
# Check if MongoDB is running
docker ps | findstr mongodb

# Check logs
docker logs bookstore-mongodb-dev

# Restart MongoDB
docker compose -f docker-compose.dev.yml restart

# If still failing, remove and recreate
docker compose -f docker-compose.dev.yml down -v
docker compose -f docker-compose.dev.yml up -d
```

### **Tests Failing**

```bash
# Make sure MongoDB is running
docker ps

# Wait 10 seconds after starting MongoDB
timeout /t 10

# Clean and rebuild
cd BookStore
dotnet clean
dotnet restore
dotnet build
dotnet test
```

### **Frontend Build Errors**

```bash
cd BookStore/client

# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install

# If still failing, check Node version
node --version  # Should be 18+
```

### **Missing Database/Collections**

MongoDB databases are created automatically when the app starts. If missing:

```bash
# With Docker (no auth)
docker exec -it bookstore-mongodb-dev mongosh

# Create manually
use BookStoreDB
db.createCollection("Book")

# Restart API to trigger seeding
cd BookStore/BookStore.API
dotnet run
```

---

## 📦 Package Updates

### **Backend**
```bash
cd BookStore/BookStore.API
dotnet list package --outdated
dotnet add package <PackageName>
```

### **Frontend**
```bash
cd BookStore/client
npm outdated
npm update
```

---

## 🔄 Development Workflow

### **Typical Day**
```bash
# Morning: Start services
docker compose -f docker-compose.dev.yml up -d
cd BookStore/BookStore.API && dotnet watch run &
cd BookStore/client && npm start &

# Work on features...

# Run tests frequently
cd BookStore && dotnet test

# Evening: Stop services
docker compose -f docker-compose.dev.yml stop
```

### **Before Committing**
```bash
# Run all tests
dotnet test

# Check for linter errors
cd BookStore/client
npm run lint

# Build for production
dotnet build -c Release
npm run build
```

---

## 🎓 Next Steps

- ✅ Setup complete? Try [docs/API.md](API.md) for GraphQL examples
- ✅ Want to contribute? See [docs/CONTRIBUTING.md](CONTRIBUTING.md)
- ✅ Docker questions? Check [docs/DOCKER.md](DOCKER.md)
- ✅ Testing help? See [docs/TESTING.md](TESTING.md)

---

**Need more help?** Open an [issue](https://github.com/dvdduy/bookstore-graphql-mongo/issues).

