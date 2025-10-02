const pool = require("../config/database");

/**
 * Repository layer for interacting with the "cartable" table
 * Encapsulates all database queries related to cartable items
 */
class CartableRepository {
  /**
   * Find all cartable items for a specific user
   * @param {number|string} userId - ID of the user
   * @returns {Promise<Array>} List of cartable items, ordered by received date descending
   */
  static async findByUserId(userId) {
    const query = `
      SELECT 
        cartableid, 
        userid, 
        taskid, 
        isread, 
        receivedat
      FROM cartable
      WHERE userid = $1
      ORDER BY receivedat DESC
    `;
    
    const result = await pool.query(query, [userId]);
    return result.rows; // Return all cartable items for the user
  }
}

module.exports = CartableRepository;
