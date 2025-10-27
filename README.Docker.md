# Docker Setup for BookStore Application

This guide explains how to run the BookStore application using Docker Compose.

## 📋 Prerequisites

- **Docker Desktop** installed and running
  - [Download for Windows](https://docs.docker.com/desktop/install/windows-install/)
  - [Download for Mac](https://docs.docker.com/desktop/install/mac-install/)
  - [Download for Linux](https://docs.docker.com/desktop/install/linux-install/)

## 🚀 Quick Start

### Option 1: Run Everything in Containers (Recommended)

Start all services (MongoDB, API, Frontend):

```bash
docker-compose up -d
```

This will:
- Start MongoDB on port `27017`
- Start the API on port `5000`
- Start the Frontend on port `4200`

Access the application at: **http://localhost:4200**

### Option 2: Run Only MongoDB (For Local Development)

If you want to run MongoDB in Docker but develop the API and Frontend locally:

```bash
docker-compose -f docker-compose.dev.yml up -d
```

Then run your API and Frontend locally:

```bash
# Terminal 1 - Backend API
cd BookStore
dotnet run --project BookStore.API

# Terminal 2 - Frontend
cd BookStore/client
npm start
```

**Note:** Update `appsettings.Development.json` to use:
```json
{
  "MongoDbConfiguration": {
    "ConnectionString": "mongodb://admin:admin123@localhost:27017",
    "Database": "bookstoredb"
  }
}
```

## 📦 Docker Compose Services

### Production Setup (`docker-compose.yml`)

- **mongodb**: MongoDB 7.0 with persistent data
  - Username: `admin`
  - Password: `admin123`
  - Port: `27017`
  
- **api**: ASP.NET Core 9.0 GraphQL API
  - Port: `5000`
  - GraphQL Playground: http://localhost:5000/graphql
  
- **frontend**: Angular 13 with Nginx
  - Port: `4200`
  - URL: http://localhost:4200

### Development Setup (`docker-compose.dev.yml`)

- **mongodb**: MongoDB only
  - Use this when you want to develop locally with hot reload
  - MongoDB will be available at `localhost:27017`

## 🛠️ Common Commands

### Start Services
```bash
# Start all services in background
docker-compose up -d

# Start with logs visible
docker-compose up

# Start specific service
docker-compose up -d mongodb
```

### Stop Services
```bash
# Stop all services
docker-compose down

# Stop and remove volumes (⚠️ deletes database data)
docker-compose down -v
```

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api
docker-compose logs -f mongodb
docker-compose logs -f frontend
```

### Rebuild Services
```bash
# Rebuild all services
docker-compose build

# Rebuild specific service
docker-compose build api

# Rebuild and restart
docker-compose up -d --build
```

### Check Service Status
```bash
docker-compose ps
```

### Execute Commands in Containers
```bash
# Access MongoDB shell
docker-compose exec mongodb mongosh -u admin -p admin123

# Access API container bash
docker-compose exec api bash

# Check API health
curl http://localhost:5000/graphql
```

## 🗄️ MongoDB Access

### Using MongoDB Shell
```bash
# Connect to MongoDB
docker-compose exec mongodb mongosh -u admin -p admin123

# Inside mongosh
use bookstoredb
db.Book.find().pretty()
db.Book.countDocuments()
```

### Using MongoDB Compass (GUI)

Connection string: `mongodb://admin:admin123@localhost:27017`

### Data Persistence

Data is stored in Docker volumes:
- Production: `mongodb_data`
- Development: `mongodb_dev_data`

To view volumes:
```bash
docker volume ls
docker volume inspect bookstore_mongodb_data
```

## 🔧 Configuration

### Environment Variables

API configuration is done through environment variables in `docker-compose.yml`:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - MongoDbConfiguration__ConnectionString=mongodb://admin:admin123@mongodb:27017
  - MongoDbConfiguration__Database=bookstoredb
```

### Ports

Default ports can be changed in `docker-compose.yml`:

```yaml
ports:
  - "HOST_PORT:CONTAINER_PORT"
```

For example, to run frontend on port 8080:
```yaml
frontend:
  ports:
    - "8080:80"  # Change 4200 to 8080
```

## 🐛 Troubleshooting

### MongoDB Connection Failed

**Problem:** API can't connect to MongoDB

**Solution:**
1. Ensure MongoDB is healthy:
   ```bash
   docker-compose ps
   ```
2. Check MongoDB logs:
   ```bash
   docker-compose logs mongodb
   ```
3. Wait for MongoDB to be ready (health check takes ~30 seconds)

### Port Already in Use

**Problem:** `Error: port is already allocated`

**Solutions:**
1. Stop the conflicting service:
   ```bash
   # Find process using port 5000
   netstat -ano | findstr :5000
   # Kill the process (Windows - replace PID)
   taskkill /PID <PID> /F
   ```

2. Or change the port in `docker-compose.yml`

### API Can't Access MongoDB

**Problem:** Connection string error

**Solution:** Make sure you're using the service name `mongodb` in the connection string when running in containers:
- ✅ Correct: `mongodb://admin:admin123@mongodb:27017`
- ❌ Wrong: `mongodb://admin:admin123@localhost:27017`

### Build Fails

**Problem:** Docker build fails

**Solutions:**
1. Clean Docker cache:
   ```bash
   docker-compose down
   docker system prune -a
   docker-compose build --no-cache
   ```

2. Check disk space:
   ```bash
   docker system df
   ```

### Frontend Shows API Error

**Problem:** Frontend can't reach API

**Solution:**
1. Check if API is running:
   ```bash
   curl http://localhost:5000/graphql
   ```
2. Check nginx configuration in container:
   ```bash
   docker-compose exec frontend cat /etc/nginx/conf.d/default.conf
   ```
3. Verify CORS is enabled in the API

## 📝 Development Workflow

### Full Stack Development (All in Docker)

Best for testing production-like environment:

```bash
# 1. Make code changes
# 2. Rebuild and restart
docker-compose up -d --build

# 3. View logs
docker-compose logs -f api
```

### Hybrid Development (Only MongoDB in Docker)

Best for rapid development with hot reload:

```bash
# 1. Start MongoDB only
docker-compose -f docker-compose.dev.yml up -d

# 2. Run API locally
cd BookStore
dotnet watch run --project BookStore.API

# 3. Run Frontend locally (another terminal)
cd BookStore/client
npm start
```

## 🧹 Cleanup

### Remove Everything
```bash
# Stop and remove containers, networks
docker-compose down

# Also remove volumes (⚠️ deletes database)
docker-compose down -v

# Remove unused images
docker image prune -a
```

### Start Fresh
```bash
# Complete cleanup
docker-compose down -v
docker system prune -a -f

# Rebuild from scratch
docker-compose up -d --build
```

## 🔐 Security Notes

**⚠️ Important:** The credentials in this setup are for **DEVELOPMENT ONLY**:
- MongoDB User: `admin`
- MongoDB Password: `admin123`

For production:
1. Use strong passwords
2. Use Docker secrets or environment files
3. Don't commit credentials to Git
4. Use HTTPS/TLS
5. Implement proper authentication

## 📊 Monitoring

### Check Resource Usage
```bash
# Container stats
docker stats

# Specific service
docker stats bookstore-api
```

### Health Checks
All services have health checks configured. Check status:
```bash
docker-compose ps
```

Healthy services show `(healthy)` status.

## 🎯 Next Steps

1. **Access GraphQL Playground**: http://localhost:5000/graphql
2. **Access Application**: http://localhost:4200
3. **Test API**: Try sample GraphQL queries
4. **Connect MongoDB GUI**: Use MongoDB Compass with connection string

## 💡 Tips

- Use `docker-compose logs -f` to see live logs while developing
- Keep `docker-compose.dev.yml` for local development
- Use `docker-compose.yml` to test production builds
- MongoDB data persists in volumes between restarts
- First startup takes longer due to image downloads and builds

## 📚 Additional Resources

- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [MongoDB Docker Hub](https://hub.docker.com/_/mongo)
- [ASP.NET Core Docker](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/)
- [Angular Docker Guide](https://angular.io/guide/deployment#docker)

