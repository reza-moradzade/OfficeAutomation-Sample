export default defineEventHandler(async (event) => {
  // Skip auth for public routes
  if (event.path.startsWith('/api/auth/') || event.path === '/api/init') {
    return
  }
  
  // Only check auth for API routes
  if (!event.path.startsWith('/api/')) {
    return
  }
  
  const authToken = getCookie(event, 'auth-token')
  
  if (!authToken) {
    throw createError({
      statusCode: 401,
      statusMessage: 'Not authenticated'
    })
  }
  
  try {
    const session = JSON.parse(authToken)
    event.context.user = session
  } catch (error) {
    throw createError({
      statusCode: 401,
      statusMessage: 'Invalid session'
    })
  }
})