import { User } from '../../models/index.js'

export default defineEventHandler(async (event) => {
  const body = await readBody(event)
  
  try {
    const { email, password } = body
    
    console.log('Login attempt for:', email) // Added logging
    
    // Find user by email
    const user = await User.findOne({ where: { email } })
    
    if (!user) {
      console.log('User not found:', email)
      throw createError({
        statusCode: 401,
        statusMessage: 'Invalid email or password'
      })
    }
    
    console.log('User found:', user.email)
    
    // Check password (note: in production, use proper password hashing)
    if (user.password !== password) {
      console.log('Invalid password for:', email)
      throw createError({
        statusCode: 401,
        statusMessage: 'Invalid email or password'
      })
    }
    
    // Create session object
    const session = {
      userId: user.id,
      email: user.email,
      fullName: user.fullName,
      department: user.department
    }
    
    // Set authentication cookie
    setCookie(event, 'auth-token', JSON.stringify(session), {
      httpOnly: true,
      secure: process.env.NODE_ENV === 'production',
      maxAge: 24 * 60 * 60 // 24 hours
    })
    
    console.log('Login successful for:', email)
    
    // Return success response
    return {
      success: true,
      user: {
        id: user.id,
        email: user.email,
        fullName: user.fullName,
        department: user.department
      }
    }
    
  } catch (error) {
    console.error('Login error:', error)
    throw createError({
      statusCode: error.statusCode || 500,
      statusMessage: error.statusMessage || 'Internal Server Error'
    })
  }
})