const { Pool } = require("pg");

/**
 * PostgreSQL database connection pool
 * Uses environment variables for configuration, with defaults if not set
 */
const pool = new Pool({
  user: process.env.DB_USER || "postgres",       // Database username
  host: process.env.DB_HOST || "localhost",      // Database host
  database: process.env.DB_NAME || "OfficeAutomationDB", // Database name
  password: process.env.DB_PASS || "MyNewPassword123",   // Database password
  port: process.env.DB_PORT || 5432,             // Database port (default PostgreSQL port)
});

module.exports = pool;
