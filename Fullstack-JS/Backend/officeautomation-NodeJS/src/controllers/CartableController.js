const CartableService = require("../services/CartableService");

/**
 * Controller layer for handling cartable-related HTTP requests
 * Delegates business logic to CartableService
 */
class CartableController {
  /**
   * GET /cartable
   * Retrieve cartable items for the authenticated user
   * Requires JWT authentication middleware to have run before this
   * @param {Object} req - Express request object (req.user is set by auth middleware)
   * @param {Object} res - Express response object
   */
  static async getCartable(req, res) {
    try {
      // Extract authenticated user's ID from JWT decoded by middleware
      const userId = req.user.userId;

      // Fetch cartable items from the service layer
      const cartable = await CartableService.getCartable(userId);

      // Return cartable items as JSON
      return res.json({ cartable });
    } catch (err) {
      // Log server errors for debugging
      console.error(err);

      // Return generic server error response
      return res.status(500).json({ message: "Server error" });
    }
  }
}

module.exports = CartableController;
