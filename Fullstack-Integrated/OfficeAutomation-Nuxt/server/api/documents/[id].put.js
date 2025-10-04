import { Document } from '../../models/index.js'

export default defineEventHandler(async (event) => {
  const { id } = event.context.params
  const body = await readBody(event)
  
  try {
    const document = await Document.findByPk(id)
    
    if (!document) {
      throw createError({
        statusCode: 404,
        statusMessage: 'Document not found'
      })
    }
    
    await document.update(body)
    
    return {
      success: true,
      data: document
    }
    
  } catch (error) {
    throw createError({
      statusCode: 500,
      statusMessage: 'Failed to update document'
    })
  }
})