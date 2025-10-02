const jwt = require("jsonwebtoken");

/**
 * Generate a new JWT token
 * @param {Object} payload - Data to include in the token (e.g., user ID, role)
 * @returns {string} Signed JWT token (valid for 1 hour)
 */
function generateToken(payload) {
  return jwt.sign(
    payload, 
    process.env.JWT_SECRET || "SECRET_KEY", 
    { expiresIn: "1h" } // Token expiration time
  );
}

/**
 * Verify and decode a JWT token
 * @param {string} token - JWT token to verify
 * @returns {Object} Decoded token payload if valid
 * @throws {Error} If token is invalid or expired
 */
function verifyToken(token) {
  return jwt.verify(
    token, 
    process.env.JWT_SECRET || "SECRET_KEY"
  );
}

module.exports = { generateToken, verifyToken };
