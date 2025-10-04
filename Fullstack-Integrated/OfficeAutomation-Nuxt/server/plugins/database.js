import { initializeDatabase } from '../models/index.js'

export default defineNitroPlugin(async (nitroApp) => {
  console.log('Initializing database...')
  try {
    await initializeDatabase()
    console.log('Database initialized successfully')
  } catch (error) {
    console.error('Failed to initialize database:', error)
  }
})
