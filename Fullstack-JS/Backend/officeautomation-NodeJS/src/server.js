// Load environment variables from .env file
require("dotenv").config();

// Import the main Express application
const app = require("./app");

// Define the server port (default: 3000 if not set in environment variables)
const port = process.env.PORT || 3000;

// Start the server and log useful information
app.listen(port, () => {
  console.log(`✅ Server running on http://localhost:${port}`);
  console.log(`📖 Swagger UI available at: http://localhost:${port}/api-docs`);
});
