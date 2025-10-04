import { Sequelize, DataTypes } from 'sequelize'

// Connect to SQLite database
const sequelize = new Sequelize({
  dialect: 'sqlite',
  storage: './database.sqlite',
  logging: false
})

// User model definition
const User = sequelize.define('User', {
  id: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true
  },
  email: {
    type: DataTypes.STRING,
    allowNull: false,
    unique: true
  },
  password: {
    type: DataTypes.STRING,
    allowNull: false
  },
  fullName: {
    type: DataTypes.STRING,
    allowNull: false
  },
  department: {
    type: DataTypes.STRING,
    defaultValue: 'General'
  }
})

// Document model definition
const Document = sequelize.define('Document', {
  id: {
    type: DataTypes.INTEGER,
    primaryKey: true,
    autoIncrement: true
  },
  title: {
    type: DataTypes.STRING,
    allowNull: false
  },
  description: {
    type: DataTypes.TEXT,
    allowNull: true
  },
  type: {
    type: DataTypes.STRING,
    allowNull: false,
    defaultValue: 'incoming'
  },
  priority: {
    type: DataTypes.STRING,
    defaultValue: 'medium'
  },
  status: {
    type: DataTypes.STRING,
    defaultValue: 'pending'
  },
  dueDate: {
    type: DataTypes.DATE,
    allowNull: true
  },
  assignedTo: {
    type: DataTypes.INTEGER,
    allowNull: true
  },
  createdBy: {
    type: DataTypes.INTEGER,
    allowNull: false
  }
})

// Define relationships between models
Document.belongsTo(User, { as: 'assignee', foreignKey: 'assignedTo' })
Document.belongsTo(User, { as: 'creator', foreignKey: 'createdBy' })

// Initialize and sync database
const initializeDatabase = async () => {
  try {
    await sequelize.authenticate()
    console.log('Database connection established successfully.')
    
    await sequelize.sync({ force: false })
    console.log('Database synchronized.')
    
    await createDefaultUser()
  } catch (error) {
    console.error('Unable to connect to the database:', error)
  }
}

// Create default user and sample data
const createDefaultUser = async () => {
  const userCount = await User.count()
  if (userCount === 0) {
    await User.create({
      email: 'admin@office.com',
      password: '123456',
      fullName: 'System Administrator',
      department: 'IT'
    })
    console.log('Default user created.')
    
    // Create sample document
    await Document.create({
      title: 'Financial Report Review',
      description: 'Review and approve Q1 financial report',
      type: 'incoming',
      priority: 'high',
      status: 'pending',
      dueDate: new Date('2024-03-05'),
      assignedTo: 1,
      createdBy: 1
    })
    
    console.log('Sample document created.')
  }
}

// Export all modules
export {
  User,
  Document,
  sequelize,
  initializeDatabase
}