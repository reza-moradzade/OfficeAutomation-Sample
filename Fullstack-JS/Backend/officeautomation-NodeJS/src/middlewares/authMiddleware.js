const { verifyToken } = require("../utils/jwtUtil");

/**
 * JWT Authentication Middleware
 * Checks the Authorization header for a Bearer token
 * Decodes the token and attaches user info to req.user
 * @param {Object} req - Express request object
 * @param {Object} res - Express response object
 * @param {Function} next - Next middleware function
 */
module.exports = (req, res, next) => {
  // Get Authorization header
  const authHeader = req.headers["authorization"];
  if (!authHeader) 
    return res.status(401).json({ message: "No token provided" });

  // Extract token from header (Bearer <token>)
  const token = authHeader.split(" ")[1];
  if (!token) 
    return res.status(401).json({ message: "No token provided" });

  try {
    // Verify and decode JWT token
    const decoded = verifyToken(token);

    // Attach decoded user information to request object
    req.user = decoded; // { userId, username }

    // Proceed to next middleware or route handler
    next();
  } catch (err) {
    // Invalid token
    return res.status(403).json({ message: "Invalid token" });
  }
};
