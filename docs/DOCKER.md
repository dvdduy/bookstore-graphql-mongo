# 🐳 Docker Guide

Complete guide to using Docker with the BookStore application.

---

## 📋 Quick Reference

```bash
# Start development MongoDB (no auth)
docker compose -f docker-compose.dev.yml up -d

# Start production-like MongoDB (with auth)
docker compose -f docker-compose.dev-auth.yml up -d

# Start full stack (all services)
docker compose up -d

# Stop services
docker compose down

# Stop and remove data
docker compose down -v
```

---

## 🎯 Three Docker Configurations

### **1. `docker-compose.dev.yml` - Development (Recommended)**

**Use for:** Testing, development, integration tests

**Features:**
- ✅ MongoDB **without** authentication
- ✅ Auto-creates `BookStoreDB` and `BookStoreTestDB`
- ✅ Health checks included
- ✅ Data persists in Docker volume

**Connection String:**
```
mongodb://localhost:27017
```

**Start:**
```bash
docker compose -f docker-compose.dev.yml up -d
```

---

### **2. `docker-compose.dev-auth.yml` - Production-like**

**Use for:** Security testing, production simulation

**Features:**
- ✅ MongoDB **with** authentication
- ✅ Creates admin and app users
- ✅ Production-like security setup

**Credentials:**
- Admin: `admin` / `admin123`
- App User: `bookstore_app` / `bookstore_dev_pass`

**Connection String:**
```
mongodb://bookstore_app:bookstore_dev_pass@localhost:27017
```

**Start:**
```bash
docker compose -f docker-compose.dev-auth.yml up -d
```

**Update your `appsettings.Development.json`:**
```json
{
  "MongoDbConfiguration": {
    "ConnectionString": "mongodb://bookstore_app:bookstore_dev_pass@localhost:27017",
    "Database": "BookStoreDB"
  }
}
```

---

### **3. `docker-compose.yml` - Full Stack**

**Use for:** Complete deployment, demos

**Features:**
- ✅ MongoDB + API + Frontend (all containerized)
- ✅ Nginx for frontend
- ✅ Internal Docker network

**Start:**
```bash
docker compose up -d
```

**Access:**
- Frontend: http://localhost
- API: http://localhost:5000
- GraphQL IDE: http://localhost:5000/graphql

---

## 🚀 Getting Started

### **First Time Setup**

1. **Install Docker Desktop**
   - Download: https://www.docker.com/products/docker-desktop/
   - Install and restart computer

2. **Verify Installation**
   ```bash
   docker --version
   docker compose version
   ```

3. **Start MongoDB**
   ```bash
   cd bookstore-graphql-mongo
   docker compose -f docker-compose.dev.yml up -d
   ```

4. **Wait for Initialization** (10 seconds)
   ```bash
   docker ps
   # Should show: bookstore-mongodb-dev (healthy)
   ```

5. **Verify**
   ```bash
   docker exec -it bookstore-mongodb-dev mongosh
   # In mongosh:
   show dbs
   # Should see: BookStoreDB, BookStoreTestDB
   ```

---

## 🔍 MongoDB Initialization

When MongoDB starts, it automatically:

1. Creates databases: `BookStoreDB` and `BookStoreTestDB`
2. Runs init scripts from `docker/` folder
3. Sets up health checks
4. For auth version: creates users with permissions

**Init Scripts:**
- `docker/mongo-init.js` - No auth version
- `docker/mongo-init-auth.js` - Auth version

---

## 📊 Container Management

### **Check Status**
```bash
# List running containers
docker ps

# List all containers (including stopped)
docker ps -a

# Filter by name
docker ps | grep mongodb
```

### **View Logs**
```bash
# Follow logs
docker logs -f bookstore-mongodb-dev

# Last 100 lines
docker logs --tail 100 bookstore-mongodb-dev

# Since timestamp
docker logs --since 10m bookstore-mongodb-dev
```

### **Restart Container**
```bash
docker compose -f docker-compose.dev.yml restart
```

### **Stop Container**
```bash
# Stop (data persists)
docker compose -f docker-compose.dev.yml stop

# Stop and remove containers
docker compose -f docker-compose.dev.yml down

# Stop, remove containers AND delete data
docker compose -f docker-compose.dev.yml down -v
```

---

## 🔧 Advanced Usage

### **Access MongoDB Shell**
```bash
# No auth
docker exec -it bookstore-mongodb-dev mongosh

# With auth
docker exec -it bookstore-mongodb-dev-auth mongosh -u admin -p admin123 --authenticationDatabase admin
```

### **Backup Database**
```bash
# Export data
docker exec bookstore-mongodb-dev mongodump --db BookStoreDB --out /tmp/backup

# Copy to host
docker cp bookstore-mongodb-dev:/tmp/backup ./mongodb-backup
```

### **Restore Database**
```bash
# Copy backup to container
docker cp ./mongodb-backup bookstore-mongodb-dev:/tmp/backup

# Restore
docker exec bookstore-mongodb-dev mongorestore --db BookStoreDB /tmp/backup/BookStoreDB
```

### **Run MongoDB on Different Port**
Edit `docker-compose.dev.yml`:
```yaml
ports:
  - "27018:27017"  # External:Internal
```

Then use: `mongodb://localhost:27018`

---

## 🧪 Integration Tests with Docker

### **Setup**
```bash
# 1. Start MongoDB
docker compose -f docker-compose.dev.yml up -d

# 2. Wait for health check
docker ps | grep healthy

# 3. Run tests
cd BookStore
dotnet test BookStore.IntegrationTests/BookStore.IntegrationTests.csproj
```

**Expected:** All 13 integration tests pass ✅

### **Troubleshooting Tests**

If tests fail with authentication errors:

1. **Verify using no-auth version:**
   ```bash
   docker compose -f docker-compose.dev-auth.yml down
   docker compose -f docker-compose.dev.yml up -d
   ```

2. **Check MongoDB is healthy:**
   ```bash
   docker ps
   # Status should show "healthy"
   ```

3. **Check logs for errors:**
   ```bash
   docker logs bookstore-mongodb-dev
   ```

4. **Recreate with fresh data:**
   ```bash
   docker compose -f docker-compose.dev.yml down -v
   docker compose -f docker-compose.dev.yml up -d
   timeout /t 10  # Wait 10 seconds
   ```

---

## 🚨 Troubleshooting

### **Port 27017 Already in Use**

**Problem:** Another MongoDB instance is running

**Solution:**
```bash
# Stop Windows MongoDB service
net stop MongoDB

# Or find and kill the process
netstat -ano | findstr :27017
taskkill /PID <PID> /F
```

### **Container Won't Start**

**Check logs:**
```bash
docker logs bookstore-mongodb-dev
```

**Common issues:**
- Port conflict (see above)
- Insufficient memory (allocate more in Docker Desktop)
- Corrupted volume (remove with `-v` flag)

**Solution:**
```bash
docker compose -f docker-compose.dev.yml down -v
docker compose -f docker-compose.dev.yml up -d
```

### **Health Check Failing**

**Symptoms:** Container status shows "unhealthy"

**Solutions:**
```bash
# 1. Check what the health check is doing
docker inspect bookstore-mongodb-dev | grep -A 10 Healthcheck

# 2. Manually run health check command
docker exec bookstore-mongodb-dev mongosh --eval "db.adminCommand('ping')"

# 3. Restart container
docker compose -f docker-compose.dev.yml restart

# 4. If still failing, recreate
docker compose -f docker-compose.dev.yml down -v
docker compose -f docker-compose.dev.yml up -d
```

### **Data Not Persisting**

**Problem:** Data disappears after `docker compose down`

**Cause:** Using `-v` flag removes volumes

**Solution:**
```bash
# Stop without removing volumes
docker compose -f docker-compose.dev.yml stop

# Or restart without recreating
docker compose -f docker-compose.dev.yml restart
```

### **Can't Connect from Host**

**Problem:** API can't reach MongoDB

**Solutions:**
```bash
# 1. Verify container is running
docker ps | grep mongodb

# 2. Check port mapping
docker port bookstore-mongodb-dev

# 3. Test connection
docker exec -it bookstore-mongodb-dev mongosh --eval "db.version()"

# 4. Use host.docker.internal for Mac/Windows
# Connection string: mongodb://host.docker.internal:27017
```

---

## 🔄 Development Workflow

### **Daily Development**
```bash
# Morning
docker compose -f docker-compose.dev.yml up -d

# Work...

# Evening
docker compose -f docker-compose.dev.yml stop
```

### **Switch Between Auth/No-Auth**
```bash
# Stop current
docker compose -f docker-compose.dev.yml down

# Start other
docker compose -f docker-compose.dev-auth.yml up -d

# Update connection string in appsettings.Development.json
```

### **Clean Slate**
```bash
# Remove everything (containers, networks, volumes)
docker compose -f docker-compose.dev.yml down -v
docker compose -f docker-compose.dev-auth.yml down -v
docker compose down -v

# Start fresh
docker compose -f docker-compose.dev.yml up -d
```

---

## 📈 Performance Tips

1. **Allocate More Memory**
   - Docker Desktop → Settings → Resources
   - Increase memory to 4GB+ for better performance

2. **Use Volumes**
   - Already configured in docker-compose files
   - Much faster than bind mounts

3. **Health Checks**
   - Already configured
   - Ensures MongoDB is ready before tests

4. **Keep Containers Running**
   - Use `stop` instead of `down` during development
   - Faster startup

---

## 🎓 Best Practices

✅ **Use `docker-compose.dev.yml` for development** - No auth hassle  
✅ **Test with `docker-compose.dev-auth.yml` before production** - Catch auth issues early  
✅ **Monitor logs regularly** - `docker logs -f bookstore-mongodb-dev`  
✅ **Clean up old images** - `docker system prune -a`  
✅ **Use health checks** - Already configured, wait for "healthy" status  
✅ **Backup data** - Use `mongodump` before major changes  

---

## 📚 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Reference](https://docs.docker.com/compose/compose-file/)
- [MongoDB Docker Hub](https://hub.docker.com/_/mongo)
- [MongoDB Connection Strings](https://docs.mongodb.com/manual/reference/connection-string/)

---

**Need more help?** Check [docs/SETUP.md](SETUP.md) or open an [issue](https://github.com/dvdduy/bookstore-graphql-mongo/issues).

