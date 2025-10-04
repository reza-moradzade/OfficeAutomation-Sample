export default defineEventHandler(async (event) => {
  // Get authentication token from cookie
  const authToken = getCookie(event, 'auth-token')
  
  console.log('Auth token from cookie:', authToken) // For debugging
  
  // Check if auth token exists
  if (!authToken) {
    console.log('No auth token found')
    throw createError({
      statusCode: 401,
      statusMessage: 'Not authenticated'
    })
  }
  
  try {
    // Parse session data from token
    const session = JSON.parse(authToken)
    console.log('Session data:', session) // For debugging
    
    // Return user session data
    return {
      user: session
    }
  } catch (error) {
    console.error('Error parsing auth token:', error)
    throw createError({
      statusCode: 401,
      statusMessage: 'Invalid session'
    })
  }
})