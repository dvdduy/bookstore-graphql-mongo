// MongoDB Initialization Script
// This runs when the database is first created
// Creates the BookStoreDB database and BookStoreTestDB for tests

print('📚 Initializing BookStore MongoDB databases...');

// Switch to main database
db = db.getSiblingDB('BookStoreDB');
print('✅ Created BookStoreDB database');

// Create a dummy collection to ensure DB exists
db.createCollection('_init');
db._init.insertOne({ initialized: true, timestamp: new Date() });
print('✅ Initialized BookStoreDB');

// Switch to test database
db = db.getSiblingDB('BookStoreTestDB');
print('✅ Created BookStoreTestDB database for integration tests');

// Create a dummy collection to ensure DB exists
db.createCollection('_init');
db._init.insertOne({ initialized: true, timestamp: new Date() });
print('✅ Initialized BookStoreTestDB');

print('🎉 MongoDB initialization complete!');

