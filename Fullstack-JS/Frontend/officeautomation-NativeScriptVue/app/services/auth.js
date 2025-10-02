// app/services/auth.js

import * as applicationSettings from "@nativescript/core/application-settings";

// Key used to store the authentication token in application settings
const TOKEN_KEY = "auth_token";

/**
 * Save JWT token to application settings
 * @param {string} token - Authentication token
 */
function saveToken(token) {
  applicationSettings.setString(TOKEN_KEY, token);
}

/**
 * Retrieve JWT token from application settings
 * @returns {string|null} - Stored token or null if not set
 */
function getToken() {
  return applicationSettings.getString(TOKEN_KEY, null);
}

/**
 * Remove JWT token from application settings
 */
function clearToken() {
  applicationSettings.remove(TOKEN_KEY);
}

/**
 * Check if the user is authenticated
 * @returns {boolean} - True if token exists, otherwise false
 */
function isAuthenticated() {
  return !!getToken();
}

/**
 * Simulate a login API request
 * In a real app, replace this with actual HTTP request
 * @param {string} username 
 * @param {string} password 
 * @returns {Promise<{token: string}>} - Resolves with fake token on success
 */
function loginRequest(username, password) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (username === "reza" && password === "123456") {
        // Simulate successful login
        resolve({ token: "fake-jwt-token-abc123" });
      } else {
        // Simulate login failure
        reject(new Error("Invalid credentials"));
      }
    }, 500); // Simulate network delay
  });
}

// Export all authentication utility functions
export default {
  saveToken,
  getToken,
  clearToken,
  isAuthenticated,
  loginRequest,
};
