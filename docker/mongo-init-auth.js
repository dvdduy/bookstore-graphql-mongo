// MongoDB Initialization Script WITH AUTHENTICATION
// This runs when the database is first created
// Creates app user with read/write permissions

print('📚 Initializing BookStore MongoDB with authentication...');

// Switch to admin database to create users
db = db.getSiblingDB('admin');

// Create application user with read/write access to all databases
db.createUser({
  user: 'bookstore_app',
  pwd: 'bookstore_dev_pass',
  roles: [
    { role: 'readWrite', db: 'BookStoreDB' },
    { role: 'readWrite', db: 'BookStoreTestDB' }
  ]
});
print('✅ Created bookstore_app user');

// Switch to main database
db = db.getSiblingDB('BookStoreDB');
db.createCollection('_init');
db._init.insertOne({ initialized: true, timestamp: new Date() });
print('✅ Initialized BookStoreDB');

// Switch to test database
db = db.getSiblingDB('BookStoreTestDB');
db.createCollection('_init');
db._init.insertOne({ initialized: true, timestamp: new Date() });
print('✅ Initialized BookStoreTestDB');

print('🎉 MongoDB initialization with auth complete!');
print('📝 Connection strings:');
print('   Main:  mongodb://bookstore_app:bookstore_dev_pass@localhost:27017/BookStoreDB');
print('   Tests: mongodb://bookstore_app:bookstore_dev_pass@localhost:27017/BookStoreTestDB');

