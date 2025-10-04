import { Document } from '../../models/index.js'

export default defineEventHandler(async (event) => {
  const { id } = event.context.params
  
  try {
    const document = await Document.findByPk(id)
    
    if (!document) {
      throw createError({
        statusCode: 404,
        statusMessage: 'Document not found'
      })
    }
    
    await document.destroy()
    
    return {
      success: true,
      message: 'Document deleted successfully'
    }
    
  } catch (error) {
    throw createError({
      statusCode: 500,
      statusMessage: 'Failed to delete document'
    })
  }
})