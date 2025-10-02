const AuthService = require("../services/AuthService");

/**
 * Controller layer for handling authentication-related HTTP requests
 * Handles request/response and delegates business logic to AuthService
 */
class AuthController {
  /**
   * POST /auth/login
   * Authenticate user with username and password
   * Returns a JWT token on success
   * @param {Object} req - Express request object
   * @param {Object} res - Express response object
   */
  static async login(req, res) {
    try {
      const { username, password } = req.body;

      // Delegate login logic to AuthService
      const result = await AuthService.login(username, password);

      // Send successful response with JWT token
      res.json({ message: "Login successful", token: result.token });
    } catch (err) {
      // Handle errors (e.g., authentication failure, server error)
      res.status(err.status || 500).json({ message: err.message || "Server error" });
    }
  }
}

module.exports = AuthController;
