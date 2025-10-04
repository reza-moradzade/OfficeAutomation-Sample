import { Document } from '../../models/index.js'

export default defineEventHandler(async (event) => {
  const body = await readBody(event)
  const authToken = getCookie(event, 'auth-token')
  
  if (!authToken) {
    throw createError({
      statusCode: 401,
      statusMessage: 'Not authenticated'
    })
  }
  
  try {
    const session = JSON.parse(authToken)
    
    const document = await Document.create({
      ...body,
      createdBy: session.userId
    })
    
    return {
      success: true,
      data: document
    }
    
  } catch (error) {
    throw createError({
      statusCode: 500,
      statusMessage: 'Failed to create document'
    })
  }
})