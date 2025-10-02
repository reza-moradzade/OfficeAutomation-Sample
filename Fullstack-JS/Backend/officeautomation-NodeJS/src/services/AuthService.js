const UserRepository = require("../repositories/UserRepository");
const { hashPassword } = require("../utils/cryptoUtil");
const { generateToken } = require("../utils/jwtUtil");

/**
 * Service layer for handling authentication-related business logic
 */
class AuthService {
  /**
   * Authenticate user with username and password
   * @param {string} username - User's username
   * @param {string} password - Plain text password
   * @returns {Promise<Object>} Authenticated user object with JWT token
   * @throws {Object} Error object with status and message if authentication fails
   */
  static async login(username, password) {
    // Fetch user from database by username
    const user = await UserRepository.findByUsername(username);
    if (!user) {
      throw { status: 401, message: "User not found" };
    }

    // Validate user account status
    if (!user.is_active || user.is_deleted || !user.is_email_confirmed) {
      throw { status: 403, message: "User deactivated or not confirmed" };
    }

    // Hash provided password with stored salt and compare with saved hash
    const newHash = hashPassword(password, user.salt);
    if (newHash !== user.password_hash) {
      throw { status: 401, message: "Wrong password" };
    }

    // Generate JWT token with user information
    const token = generateToken({ userId: user.user_id, username: user.username });

    return { token, user };
  }
}

module.exports = AuthService;
