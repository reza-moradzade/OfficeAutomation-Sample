const pool = require("../config/database");

/**
 * Repository layer for interacting with the "users" table
 * Encapsulates all database queries related to users
 */
class UserRepository {
  /**
   * Find a user by username
   * @param {string} username - Username to search for
   * @returns {Promise<Object|null>} User object if found, otherwise null
   */
  static async findByUsername(username) {
    const query = `
      SELECT 
        user_id, 
        username, 
        password_hash, 
        salt, 
        is_active, 
        is_deleted,
        is_email_confirmed
      FROM users 
      WHERE username = $1
    `;
    
    const result = await pool.query(query, [username]);
    return result.rows[0]; // Return the first matching user or undefined
  }
}

module.exports = UserRepository;
