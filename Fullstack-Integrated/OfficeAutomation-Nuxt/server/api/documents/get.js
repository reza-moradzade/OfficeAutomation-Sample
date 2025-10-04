import { Document, User } from '../../models/index.js'

export default defineEventHandler(async (event) => {
  try {
    const documents = await Document.findAll({
      include: [
        {
          model: User,
          as: 'assignee',
          attributes: ['id', 'fullName', 'email']
        },
        {
          model: User,
          as: 'creator',
          attributes: ['id', 'fullName', 'email']
        }
      ],
      order: [['createdAt', 'DESC']]
    })
    
    return {
      success: true,
      data: documents
    }
    
  } catch (error) {
    throw createError({
      statusCode: 500,
      statusMessage: 'Failed to fetch documents'
    })
  }
})