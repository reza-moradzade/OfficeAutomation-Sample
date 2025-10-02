const crypto = require("crypto");

/**
 * Hash a password using SHA-256 with a salt
 * @param {string} password - The plain text password
 * @param {string} salt - A unique salt value to strengthen the hash
 * @returns {string} Hashed password in hexadecimal format
 */
function hashPassword(password, salt) {
  return crypto
    .createHash("sha256")        // Use SHA-256 hashing algorithm
    .update(password + salt, "utf8") // Combine password with salt
    .digest("hex");              // Return hash as hex string
}

module.exports = { hashPassword };
