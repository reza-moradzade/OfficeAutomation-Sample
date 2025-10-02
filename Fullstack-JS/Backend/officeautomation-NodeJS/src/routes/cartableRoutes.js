const express = require("express");
const router = express.Router();
const CartableController = require("../controllers/CartableController");
const authMiddleware = require("../middlewares/authMiddleware");

/**
 * @swagger
 * /cartable:
 *   get:
 *     summary: Get user cartable
 *     security:
 *       - bearerAuth: []
 *     responses:
 *       200:
 *         description: Cartable fetched successfully
 *         content:
 *           application/json:
 *             example:
 *               cartable: []
 *       401:
 *         description: Unauthorized (missing or invalid token)
 *       403:
 *         description: Forbidden (invalid or expired token)
 *       500:
 *         description: Internal server error
 */

/**
 * GET /cartable
 * Protected route to fetch the authenticated user's cartable items
 * Requires a valid JWT token in the Authorization header
 * Middleware: authMiddleware handles authentication
 * Controller: CartableController.getCartable handles business logic
 */
router.get("/cartable", authMiddleware, CartableController.getCartable);

module.exports = router;
