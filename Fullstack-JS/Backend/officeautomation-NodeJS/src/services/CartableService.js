const CartableRepository = require("../repositories/CartableRepository");

/**
 * Service layer for handling cartable-related business logic
 * Acts as an abstraction between controllers and repositories
 */
class CartableService {
  /**
   * Retrieve cartable items for a specific user
   * @param {number|string} userId - ID of the user
   * @returns {Promise<Array>} List of cartable items for the user
   */
  static async getCartable(userId) {
    return await CartableRepository.findByUserId(userId);
  }
}

module.exports = CartableService;
